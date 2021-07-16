using Fanex.Athena.PlayerAdmin.Infrastructure.Constants;
using Fanex.Athena.PlayerAdmin.Infrastructure.Extensions;
using Fanex.Athena.PlayerAdmin.Infrastructure.Helpers;
using Fanex.Athena.PlayerAdmin.Infrastructure.Models;
using Fanex.Athena.PlayerAdmin.Infrastructure.Repositories;
using Fanex.Athena.PlayerAdmin.Infrastructure.Settings;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Infrastructure.Services
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

        public async Task<bool> IsAllowUpdateMemberBAStatus(int licenseeId)
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

        public Task<IList<Product>> GetProductList(int licenseeId, string languageCode)
        {
            var cacheKey = string.Format(CacheKey.ProductList, licenseeId, languageCode);
            return LockHelper.LockAsync(cacheKey, () => TryGetOrAddFromCache(cacheKey, TimeSpan.FromDays(1), () => GetProductListFromDB(licenseeId)));
        }

        public async Task<ProductAccess> GetProductAccess(int licenseeId, string languageCode, string function = "Default")
        {
            var allowedProducts = GetAllowedProducts(function);
            var originalProducts = await GetProductList(licenseeId, languageCode);

            var productList = allowedProducts != null && allowedProducts.Any() ? originalProducts.Where(i => allowedProducts.Contains(i.ProductCode)).ToList()
                : originalProducts;

            return new ProductAccess(productList);
        }

        public async Task<ProductAccess> GetSingleWalletProductAccess(int licenseeId, string languageCode, string function = "Default")
        {
            var allowedProducts = GetAllowedProducts(function);
            var originalProducts = await GetProductList(licenseeId, languageCode);

            var productList = allowedProducts != null && allowedProducts.Any() ? originalProducts.Where(i => i.IsOfferedSingleWallet && allowedProducts.Contains(i.ProductCode)).ToList()
                : originalProducts;

            return new ProductAccess(productList);
        }

        public string[] GetAllowedProducts(string function)
        {
            var allowedProducts = _functionSetting.GetType()
               .GetProperty(function)?
               .GetValue(_functionSetting) as string[];
            return allowedProducts;
        }

        public async Task<bool> CheckProductBelongsTo(string licenseeName, int productId, int[] limitedProductIds = null)
        {
            if (limitedProductIds != null && limitedProductIds.Any() && !limitedProductIds.Contains(productId))
            {
                return false;
            }

            var cacheKey = string.Format(CacheKey.CheckProductBelong, licenseeName, productId);
            var isValidProduct = await TryGetOrAddFromCache(cacheKey, TimeSpan.FromHours(1), () => CheckProductBelongsToFromDB(licenseeName, productId));
            return isValidProduct;
        }

        public async Task<bool> CheckCustomerBelongTo(IReadOnlyDictionary<int, int[]> agentList, int custId, int currencyId = 0)
        {
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

        private static bool IsBelonging(IReadOnlyDictionary<int, int[]> currencyAgents, Customer cust)
                  => currencyAgents[cust.CurrencyId] is int[] agentList && agentList.Contains(cust.AgentId);
    }
}