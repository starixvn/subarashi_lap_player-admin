using Fanex.Athena.PlayerAdmin.API.DTO;
using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.Infrastructure.Repositories;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.Services
{
    public class PlayerActivityRecordService : IPlayerActivityRecordService
    {
        private readonly IDbFactory _dbFactory;

        public PlayerActivityRecordService(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public Task<PlayerActivityRecord> GetActivityRecord(PlayerActivityRecordMetaModel metaModel)
        {
            const string UspName = "Lrf_CustomerLoginInfo";

            using (var db = _dbFactory.GetObjectDb(UspName))
            {
                db.SetParameterValue("custid", metaModel.IbcCustId);
                db.SetParameterValue("siteName", metaModel.SiteName);

                return db.QueryEntityAsync<PlayerActivityRecord>();
            }
        }
    }
}