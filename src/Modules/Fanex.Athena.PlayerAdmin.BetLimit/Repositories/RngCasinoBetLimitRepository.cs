using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Data;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Repositories
{
    public class RngCasinoBetLimitRepository : IRngCasinoBetLimitRepository
    {
        private readonly IDbFactory _db;

        public RngCasinoBetLimitRepository(IDbFactory dbFactory)
        {
            _db = dbFactory;
        }

        public async Task<IResultInfo> UpdateStatusAsync(PlayerBetLimitUpdateStatusMetaModel metaModel, int sportId)
        {
            const string UspName = "Lrf_CustomerUpdCasinoStatus";

            using (var db = _db.GetObjectDb(UspName))
            {
                db.SetParameterValue("userid", metaModel.UserId);
                db.SetParameterValue("custid", metaModel.CustId);
                db.SetParameterValue("sportid", sportId);
                db.SetParameterValue("language", metaModel.Language);
                db.SetParameterValue("ip", metaModel.Ip);
                db.SetParameterValue("status", metaModel.Status ? 0 : 1);
                db.SetParameterValue("ErrCode", null);
                db.SetParameterValue("ErrMsg", null);

                await db.ExecuteNonQueryAsync();

                return GetResult(db);
            }
        }

        protected static IResultInfo GetResult(IObjectDb db)
        {
            var errorCode = db.GetParameterValue("ErrCode");
            var errorMessage = db.GetParameterValue("ErrMsg");

            return new ResultInfo
            {
                Code = Converter.ToInt(errorCode),
                Message = Converter.ToString(errorMessage)
            };
        }
    }
}