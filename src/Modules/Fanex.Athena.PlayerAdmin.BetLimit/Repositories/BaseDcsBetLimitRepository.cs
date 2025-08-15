namespace Fanex.Athena.PlayerAdmin.BetLimit.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Core.Extensions;
    using Core.Helpers;
    using Core.Models;
    using Core.Repositories;
    using Data;
    using DTO;
    using MetaModel;
    using Microsoft.Extensions.Logging;

    public abstract class BaseDcsBetLimitRepository
    {
        protected readonly IDbFactory _db;
        protected readonly ILogger<BaseDcsBetLimitRepository> _logger;

        protected BaseDcsBetLimitRepository(IDbFactory dbFactory, ILogger<BaseDcsBetLimitRepository> logger)
        {
            _db = dbFactory;
            _logger = logger;
        }

        protected async Task<List<PlayerBetLimit>> ExecuteGetSettingQuery(string uspName, int custId, int productId, int? settingType = null)
        {
            var dataList = new List<PlayerBetLimit>();

            using (var db = _db.GetObjectDb(uspName))
            {
                db.SetParameterValue("custid", custId);
                db.SetParameterValue("productid", productId);
                
                if (settingType.HasValue)
                {
                    db.SetParameterValue("settingtype", settingType.Value);
                }

                using (var reader = await db.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        dataList.Add(CreatePlayerBetLimitFromReader(reader));
                    }
                }
            }

            return dataList;
        }

        protected virtual PlayerBetLimit CreatePlayerBetLimitFromReader(IDataReader reader)
        {
            return new PlayerBetLimit
            {
                MinBet = reader.ToDecimal("minbet"),
                BetValueMin = reader.ToDecimal("minbetmin"),
                MaxBet = reader.ToDecimal("maxbet"),
                BetValueMax = reader.ToDecimal("maxbetmax"),
                SportType = reader.ToString("sporttype"),
                BetType = reader.ToInt("bettype")
            };
        }

        protected async Task<IResultInfo> ExecuteUpdateSettingQuery(string uspName, PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel)
        {
            using (var db = _db.GetObjectDb(uspName))
            {
                AddUpdateParameters(db, metaModel);

                await db.ExecuteNonQueryAsync().ConfigureAwait(false);

                var errorCode = db.GetParameterValue("ErrCode");
                var errorMessage = db.GetParameterValue("ErrMsg");

                return new ResultInfo
                {
                    Code = Converter.ToInt(errorCode),
                    Message = GetErrorMessage(Converter.ToString(errorMessage))
                };
            }
        }

        protected virtual void AddUpdateParameters(IObjectDb db, PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel)
        {
            db.SetParameterValue("custid", metaModel.CustId);
            db.SetParameterValue("userid", metaModel.UserId);
            db.SetParameterValue("ip", metaModel.Ip);
            db.SetParameterValue("betlimit", BuildBetLimitParameter(metaModel.ProductId, metaModel.Setting));
            db.SetParameterValue("errcode", null);
            db.SetParameterValue("errmsg", null);
        }

        protected abstract string BuildBetLimitParameter(int productId, IEnumerable<PlayerBetLimitMetaModel> setting);

        protected string GetErrorMessage(string xmlErrorMessage)
        {
            return XmlErrorHelper.GetErrorMessage(xmlErrorMessage, _logger);
        }
    }
}
