using Fanex.Athena.PlayerAdmin.Core.Models;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.Services
{
    public interface ICustomerService
    {
        Task<CustomerSetting> GetCustomerSetting(int custId);
    }
}