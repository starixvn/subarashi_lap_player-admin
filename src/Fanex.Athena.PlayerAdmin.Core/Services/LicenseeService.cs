using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Fanex.Athena.PlayerAdmin.Core.Helpers;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.Services
{
    public class LicenseeService : ILicenseeService
    {
        private readonly IMemoryCache _cache;
        private readonly IDbFactory _dbFactory;
        private readonly AppSetting _appSetting;
        private readonly FunctionAllowedProductsSetting _functionSetting;

        public LicenseeService(IMemoryCache cache, IDbFactory dbFactory, IOptions<AppSetting> appSetting, IOptions<FunctionAllowedProductsSetting> functionSetting)
        {
            _cache = cache;
            _dbFactory = dbFactory;
            _appSetting = appSetting.Value;
            _functionSetting = functionSetting.Value;
        }

        public IEnumerable<string> GetOddsGroups(string siteName)
            => _appSetting.OddsGroups
                .Where(_ => !_.OfferedBy.IsPresent() || _.OfferedBy.Any(s => string.Equals(s, siteName, StringComparison.InvariantCultureIgnoreCase)))
                .Select(_ => _.Value);

        public async Task<bool> IsAllowUpdateBA(int licenseeId, string userName, IEnumerable<string> specicalBAAdmins)
            => specicalBAAdmins.IsPresent() && specicalBAAdmins.Contains(userName, StringComparer.InvariantCultureIgnoreCase) || await IsAllowUpdateMemberBAStatus(licenseeId);

        public async Task<ProductAccess> GetProductAccess(int licenseeId, string function = "Default")
        {
            var productList = await GetAllowedProducts(licenseeId, function, null);
            return new ProductAccess(productList);
        }

        public async Task<ProductAccess> GetSingleWalletProductAccess(int licenseeId, string function = "Default")
        {
            var productList = await GetAllowedProducts(licenseeId, function, isOfferedSingleWallet: true);
            return new ProductAccess(productList);
        }

        public string[] GetAllowedProductsByFunction(string function)
        {
            var allowedProducts = _functionSetting.GetType()
               .GetProperty(function)?
               .GetValue(_functionSetting) as string[];
            return allowedProducts;
        }

        public async Task<bool> CheckProductBelongsTo(string licenseeName, int productId, int[] limitedProductIds = null)
        {
            if (limitedProductIds.IsPresent() && !limitedProductIds.Contains(productId))
            {
                return false;
            }

            var cacheKey = string.Format(CacheKey.CheckProductBelong, licenseeName, productId);
            var isValidProduct = await TryGetOrAddFromCache(cacheKey, TimeSpan.FromHours(1), () => CheckProductBelongsToFromDB(licenseeName, productId));
            return isValidProduct;
        }

        public async Task<bool> CheckCustomerBelongTo(IReadOnlyDictionary<int, int[]> agentList, int custId, int currencyId = 0)
        {
            if (custId == 0)
            {
                return true;
            }

            using (var db = _dbFactory.GetObjectDb("Lrf_CheckBelonging_GetAgentInfo"))
            {
                db.SetParameterValue("transid", 0);
                db.SetParameterValue("custid", custId);

                using (var reader = await db.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        var actualCurrencyId = Convert.ToInt32(reader["CurrencyId"]);
                        var agentId = Convert.ToInt32(reader["CustId"]);
                        return agentList.Any(i => i.Key == actualCurrencyId && i.Value.Contains(agentId) && (currencyId == 0 || actualCurrencyId == currencyId));
                    }
                }
            }
            return false;
        }

        public async Task<ICollection<int>> GetCustomersBelongTo(IReadOnlyDictionary<int, int[]> agentList, string custIds)
        {
            var result = new List<Customer>();

            using (var db = _dbFactory.GetObjectDb("Lrf_CheckBelongingMultiple_GetAgentInfo"))
            {
                db.SetParameterValue("custids", custIds);
                using (var reader = await db.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        result.Add(new Customer
                        {
                            Id = reader.ToInt("CustId"),
                            AgentId = reader.ToInt("agentid"),
                            CurrencyId = reader.ToInt("CurrencyId")
                        });
                    }
                }
            }

            return result.Where(cust => IsBelonging(agentList, cust))
                .Select(cust => cust.Id)
                .ToArray();
        }

        public async Task<BalanceUpData> GetBalanceUp()
        {
            var balanceUp = await TryGetOrAddFromCache(CacheKey.Balanceup, TimeSpan.FromSeconds(60), GetBalanceUpDayFromDb);
            return balanceUp;
        }

        public bool IsFakeLicensee(string licenseeName) => _appSetting.FakeLicensees.Any(_ => string.Equals(_, licenseeName, StringComparison.OrdinalIgnoreCase));

        public IEnumerable<ProductOption> GetAllowedSubProductOptions(IEnumerable<string> offeredSubProductList, IEnumerable<ProductOption> productOptions)
        {
            if (!offeredSubProductList.IsPresent() || !productOptions.Any(_ => _.Code == ProductCode.SB))
            {
                return productOptions;
            }

            var newProductOptions = new List<ProductOption>(productOptions);
            newProductOptions.AddRange(offeredSubProductList.Select(subProduct => new ProductOption
            {
                Code = subProduct,
                Status = true
            }));

            return newProductOptions;
        }

        public string GetAllowedSubProductCodeList(IEnumerable<string> offeredSubProductList, string productCodes)
        {
            if (!offeredSubProductList.IsPresent() || !productCodes.Split(',').Any(_ => _ == ProductCode.SB))
            {
                return productCodes;
            }

            return productCodes + "," + string.Join(",", offeredSubProductList);
        }

        private async Task<T> TryGetOrAddFromCache<T>(string cacheKey, TimeSpan cacheTime, Func<Task<T>> func)
        {
            if (!_cache.TryGetValue(cacheKey, out T data))
            {
                data = await func.Invoke();
                if (data is not null)
                {
                    _cache.Set(cacheKey, data, cacheTime);
                }
            }
            return data;
        }

        private async Task<bool> IsAllowUpdateMemberBAStatus(int licenseeId)
        {
            const string UspName = "Lrf_BATimeCheck";

            var isAllow = false;
            using (var db = _dbFactory.GetObjectDb(UspName))
            {
                db.SetParameterValue("LicenseeID", licenseeId);
                db.SetParameterValue("isallow", null);

                await db.ExecuteNonQueryAsync();

                isAllow = Convert.ToBoolean(db.GetParameterValue("isallow"));
                return isAllow;
            }
        }

        private async Task<IList<Product>> GetProductListFromDB(int licenseeId, bool isActiveCustomerSummaryReport = false)
        {
            var uspName = isActiveCustomerSummaryReport ? "Lrf_LicenseeProduct3rdSel_active" : "Lrf_LicenseeProduct3rdSel";

            // All licensees have product Sb
            var allowedProducts = new List<Product> { new Product { IsOfferedSingleWallet = true, ProductCode = ProductCode.SB, ProductName = "Sportsbook" } };

            using (var db = _dbFactory.GetObjectDb(uspName))
            {
                db.SetParameterValue("licenseeid", licenseeId);

                using (var reader = await db.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        allowedProducts.Add(new Product
                        {
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            ProductName = Convert.ToString(reader["productprefix"]), //DataHelper.GetProductName(Convert.ToString(reader["productprefix"])),
                            ProductCode = Convert.ToString(reader["productprefix"]),
                            AllowCommission = Convert.ToBoolean(reader["comm"]),
                            AllowWinloss = Convert.ToBoolean(reader["winloss"]),
                            IsOfferedSingleWallet = Convert.ToBoolean(reader["IsOfferedSingleWallet"])
                        });
                    }
                }

                return allowedProducts;
            }
        }

        private async Task<BalanceUpData> GetBalanceUpDayFromDb()
        {
            const string UspName = "Lrf_SelBalanceUpDay";

            using (var db = _dbFactory.GetObjectDb(UspName))
            {
                var balanceUp = new BalanceUpData();
                using (var reader = await db.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        balanceUp.BalanceUpDay = Convert.ToDateTime(reader["balanceupday"]);
                        balanceUp.MoveBetsDay = Convert.ToDateTime(reader["movebetsday"]);
                    }

                    return balanceUp;
                }
            }
        }

        private async Task<bool> CheckProductBelongsToFromDB(string licenseeName, int productId)
        {
            const string UspName = "Lrf_CheckBelonging_ValidateProduct";

            using (var db = _dbFactory.GetObjectDb(UspName))
            {
                db.SetParameterValue("productId", productId);
                db.SetParameterValue("licenseeName", licenseeName);

                return Convert.ToBoolean(await db.ExecuteScalarAsync());
            }
        }

        private async Task<IList<Product>> GetAllowedProducts(int licenseeId, string function, bool? isOfferedSingleWallet)
        {
            var allowedProductsByFunction = GetAllowedProductsByFunction(function);
            var offeredProductsByLicensee = await GetOfferedProductsByLicensee(licenseeId);

            var productList = allowedProductsByFunction != null && allowedProductsByFunction.Any() ? offeredProductsByLicensee.Where(i =>
            (!isOfferedSingleWallet.HasValue || isOfferedSingleWallet == i.IsOfferedSingleWallet)
            && allowedProductsByFunction.Contains(i.ProductCode)).ToList()
                : offeredProductsByLicensee;

            return productList;
        }

        private Task<IList<Product>> GetOfferedProductsByLicensee(int licenseeId)
        {
            var cacheKey = string.Format(CacheKey.ProductList, licenseeId);
            return LockHelper.LockAsync(cacheKey, () => TryGetOrAddFromCache(cacheKey, TimeSpan.FromDays(1), () => GetProductListFromDB(licenseeId)));
        }

        private static bool IsBelonging(IReadOnlyDictionary<int, int[]> currencyAgents, Customer cust)
                  => currencyAgents[cust.CurrencyId] is int[] agentList && agentList.Contains(cust.AgentId);
    }
}