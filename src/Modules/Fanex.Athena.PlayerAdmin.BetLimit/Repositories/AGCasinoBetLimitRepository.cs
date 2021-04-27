using Fanex.Athena.PlayerAdmin.Infrastructure.Extensions;
using Fanex.Athena.PlayerAdmin.Infrastructure.Models;
using Fanex.Athena.PlayerAdmin.Infrastructure.Repositories;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces;
using Fanex.Data;
using System;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Repositories
{
    public class AGCasinoBetLimitRepository : IAGCasinoBetLimitRepository
    {
        private readonly IDbFactory _db;

        public AGCasinoBetLimitRepository(IDbFactory dbFactory)
        {
            _db = dbFactory;
        }

        public async Task<AgCasinoBetSetting> GetBetSettingAsync(int custId)
        {
            const string UspName = "Lrf_AGC_SelCustSetting";

            using (var db = _db.GetObjectDb(UspName))
            {
                AddBetSettingParameter(db, custId);

                await db.ExecuteNonQueryAsync().ConfigureAwait(false);

                return ReadBetSetting(db);
            }
        }

        public async Task<IResultInfo> UpdateSettingAsync(PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel> metaModel)
        {
            const string UspName = "Lrf_AGC_UpdCustSetting";

            using (var db = _db.GetObjectDb(UspName))
            {
                AddUpdateParameter(db, metaModel);

                await db.ExecuteNonQueryAsync().ConfigureAwait(false);

                var errorCode = db.GetParameterValue("ErrCode");
                var errorMessage = db.GetParameterValue("ErrMsg");

                return new ResultInfo
                {
                    Code = Converter.ToInt(errorCode),
                    Message = Converter.ToString(errorMessage)
                };
            }
        }

        private static void AddUpdateParameter(IObjectDb db, PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel> metaModel)
        {
            db.SetParameterValue("aggroup", metaModel.Setting.Group);
            db.SetParameterValue("ip", metaModel.Ip);
            db.SetParameterValue("language", metaModel.Language);
            db.SetParameterValue("userid", metaModel.UserId);
            db.SetParameterValue("custid", metaModel.CustId);

            db.SetParameterValue("ErrCode", null);
            db.SetParameterValue("ErrMsg", null);
        }

        private static void AddBetSettingParameter(IObjectDb db, int custId)
        {
            db.SetParameterValue("CustID", custId);
            db.SetParameterValue("Currency", null);
            db.SetParameterValue("AGGroup", null);
            db.SetParameterValue("IsEnabled", null);
            db.SetParameterValue("IsLineEnabled", null);
            db.SetParameterValue("CheckedGroups", null);
        }

        private static AgCasinoBetSetting ReadBetSetting(IObjectDb db)
        {
            return new AgCasinoBetSetting
            {
                CurrencyId = Convert.ToInt32(db.GetParameterValue("Currency")),
                AgGroup = Convert.ToString(db.GetParameterValue("AGGroup")),
                IsEnabled = Convert.ToBoolean(db.GetParameterValue("IsEnabled")),
                IsLineEnabled = Convert.ToBoolean(db.GetParameterValue("IsLineEnabled")),
                CheckedGroups = Convert.ToString(db.GetParameterValue("CheckedGroups")).Split(',')
            };
        }
    }
}