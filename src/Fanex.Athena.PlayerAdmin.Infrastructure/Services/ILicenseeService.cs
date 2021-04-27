using Fanex.Athena.PlayerAdmin.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Infrastructure.Services
{
    public interface ILicenseeService
    {
        bool IsFakeLicensee(string licenseeName);

        Task<BalanceUpData> GetBalanceUp();

        Task<bool> CheckProductBelongsTo(string licenseeName, int productId, int[] limitedProductIds = null);

        Task<bool> CheckCustomerBelongsTo(IReadOnlyDictionary<int, int[]> agentList, int custId, int currencyId = 0);

        Task<IList<Product>> GetProductList(int licenseeId, string languageCode);

        Task<ProductAccess> GetProductAccess(int licenseeId, string languageCode, string function = "Default");

        string[] GetAllowedProducts(string function);

        Task<ProductAccess> GetSingleWalletProductAccess(int licenseeId, string languageCode, string function = "Default");
    }
}