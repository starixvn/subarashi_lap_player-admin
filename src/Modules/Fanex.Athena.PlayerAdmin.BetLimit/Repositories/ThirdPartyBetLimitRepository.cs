using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Repositories
{
    public class ThirdPartyBetLimitRepository : IThirdPartyBetLimitRepository
    {
        private readonly IDbFactory _db;

        public ThirdPartyBetLimitRepository(IDbFactory dbFactory)
        {
            _db = dbFactory;
        }

        public async Task<ThirdPartyBetSetting> GetSettingAsync(int custId, int productId)
        {
            const string UspName = "Lrf_3rdProductSelCustStatus";
            using (var db = _db.GetObjectDb(UspName))
            {
                db.SetParameterValue("CustID", custId);
                db.SetParameterValue("ProductId", productId);
                db.SetParameterValue("IsEnabled", null);
                db.SetParameterValue("IsLineEnabled", null);
                db.SetParameterValue("Currency", null);

                await db.ExecuteNonQueryAsync();

                return new ThirdPartyBetSetting
                {
                    IsEnabled = Converter.ToBoolean(db.GetParameterValue("IsEnabled")),
                    IsLineEnabled = Converter.ToBoolean(db.GetParameterValue("IsLineEnabled")),
                    CurrencyId = Converter.ToInt(db.GetParameterValue("Currency"))
                };
            }
        }

        public async Task<IResultInfo> UpdateStatusAsync(PlayerBetLimitUpdateStatusMetaModel metaModel)
        {
            const string UspName = "Lrf_3rdProductUpdCustStatus";

            using (var db = _db.GetObjectDb(UspName))
            {
                db.SetParameterValue("custid", metaModel.CustId);
                db.SetParameterValue("userid", metaModel.UserId);
                db.SetParameterValue("productid", metaModel.ProductId);
                db.SetParameterValue("isenabled", metaModel.Status);
                db.SetParameterValue("ip", metaModel.Ip);
                db.SetParameterValue("language", metaModel.Language);

                db.SetParameterValue("ErrMsg", null);

                await db.ExecuteNonQueryAsync();

                var errorMessage = db.GetParameterValue("ErrMsg");

                return new ResultInfo
                {
                    Code = 0,
                    Message = Converter.ToString(errorMessage)
                };
            }
        }
    }
}