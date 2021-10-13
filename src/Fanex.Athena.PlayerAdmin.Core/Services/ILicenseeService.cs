using Fanex.Athena.PlayerAdmin.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.Services
{
    public interface ILicenseeService
    {
        Task<bool> IsAllowUpdateBA(int licenseeId, string userName, IEnumerable<string> specicalBAAdmins);

        bool IsFakeLicensee(string licenseeName);

        Task<BalanceUpData> GetBalanceUp();

        Task<bool> CheckProductBelongsTo(string licenseeName, int productId, int[] limitedProductIds = null);

        Task<bool> CheckCustomerBelongTo(IReadOnlyDictionary<int, int[]> agentList, int custId, int currencyId = 0);

         Task<ICollection<int>> GetCustomersBelongTo(IReadOnlyDictionary<int, int[]> agentList, string custIds);

        Task<IList<Product>> GetProductList(int licenseeId, string languageCode);

        Task<ProductAccess> GetProductAccess(int licenseeId, string languageCode, string function = "Default");

        string[] GetAllowedProducts(string function);

        Task<ProductAccess> GetSingleWalletProductAccess(int licenseeId, string languageCode, string function = "Default");
    }
}