using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IDbFactory _dbFactory;

        public CustomerService(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public Task<CustomerSetting> GetCustomerSetting(int custId)
        {
            const string UspName = "Lrf_CustomerSelStatus";

            using (var db = _dbFactory.GetObjectDb(UspName))
            {
                db.SetParameterValue("custid", custId);

                return db.QueryEntityAsync<CustomerSetting>();
            }
        }
    }
}