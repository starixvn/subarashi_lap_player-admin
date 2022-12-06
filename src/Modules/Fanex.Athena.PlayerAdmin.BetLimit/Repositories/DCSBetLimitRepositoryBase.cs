using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Data;
using System.Collections.Generic;
using System.Data;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Repositories
{
    public abstract class DCSBetLimitRepositoryBase
    {
        protected static void ReadCommonField(IDataReader reader, PlayerBetLimit playerBetLimit)
        {
            playerBetLimit.MinBet = reader.ToDecimal("minbet");
            playerBetLimit.BetValueMin = reader.ToDecimal("minbetmin");
            playerBetLimit.MaxBet = reader.ToDecimal("maxbet");
            playerBetLimit.BetValueMax = reader.ToDecimal("maxbetmax");
            playerBetLimit.SportType = reader.ToString("sporttype");
            playerBetLimit.BetType = reader.ToInt("bettype");
        }

        protected void AddUpdateParameter(IObjectDb db, PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel)
        {
            db.SetParameterValue("custid", metaModel.CustId);
            db.SetParameterValue("userid", metaModel.UserId);
            db.SetParameterValue("ip", metaModel.Ip);
            db.SetParameterValue("betlimit", BuildBetLimitParameter(metaModel.ProductId, metaModel.Setting));
            db.SetParameterValue("errcode", null);
            db.SetParameterValue("errmsg", null);
        }

        protected ResultInfo GetActionResult(IObjectDb db)
        {
            var errorCode = db.GetParameterValue("ErrCode");
            var errorMessage = db.GetParameterValue("ErrMsg");
            return new ResultInfo
            {
                Code = Converter.ToInt(errorCode),
                Message = GetErrorMessage(Converter.ToString(errorMessage))
            };
        }

        protected abstract string BuildBetLimitParameter(int productId, IEnumerable<PlayerBetLimitMetaModel> setting);

        protected abstract string GetErrorMessage(string xmlErrorMessage);
    }
}
