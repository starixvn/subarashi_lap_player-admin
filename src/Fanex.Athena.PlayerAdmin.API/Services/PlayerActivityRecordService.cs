namespace Fanex.Athena.PlayerAdmin.API.Services
{
    using System.Threading.Tasks;
    using Core.Repositories;
    using DTO;
    using Interfaces;
    using MetaModel;

    public class PlayerActivityRecordService : IPlayerActivityRecordService
    {
        private readonly IDbFactory _dbFactory;

        public PlayerActivityRecordService(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<PlayerActivityRecord> GetActivityRecord(PlayerActivityRecordMetaModel metaModel)
        {
            const string UspName = "Lrf_CustomerLoginInfo";

            using (var db = _dbFactory.GetObjectDb(UspName))
            {
                db.SetParameterValue("custid", metaModel.CustId);
                db.SetParameterValue("siteName", metaModel.SiteName);

                return await db.QueryEntityAsync<PlayerActivityRecord>();
            }
        }
    }
}