using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Data;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Repositories
{
    public class NumberGameBetLimitRepository : INumberGameBetLimitRepository
    {
        private static readonly string[] SportTypes =
        {
            SportType.NumberGame.ToString()
        };

        private readonly IDbFactory _db;

        public NumberGameBetLimitRepository(IDbFactory dbFactory)
        {
            _db = dbFactory;
        }

        public async Task<IEnumerable<PlayerBetLimit>> GetSettingAsync(int custId)
        {
            const string UspName = "Lrf_CustomerSelEditNG";

            using (var db = _db.GetObjectDb(UspName))
            {
                db.SetParameterValue("CustID", custId);
                using (var reader = await db.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        return ReadSetting(reader);
                    }
                }
            }

            return Enumerable.Empty<PlayerBetLimit>();
        }

        private static List<PlayerBetLimit> ReadSetting(IDataReader reader)
        {
            var sportList = new List<PlayerBetLimit>();

            foreach (var type in SportTypes)
            {
                sportList.Add(ReadForSport(reader, type));
            }

            return sportList;
        }

        private static PlayerBetLimit ReadForSport(IDataReader reader, string sportId)
        {
            var sportSuffix = "v" + sportId;

            var sport = new PlayerBetLimit
            {
                MaxBet = reader.ToDecimal("maxbet" + sportSuffix),
                MinBet = reader.ToDecimal("minbet" + sportSuffix),
                MaxPerMatch = reader.ToDecimal("MaxPerMatch" + sportSuffix),
                MaxPerMatchMax = reader.ToDecimal("MaxPerMatch" + sportSuffix + "_Max"),
                MaxPerMatchMin = reader.ToDecimal("MaxPerMatch" + sportSuffix + "_Min"),
                BetValueMax = reader.ToDecimal("MaxBet" + sportSuffix + "_Min"),
                BetValueMin = reader.ToDecimal("MinBet" + sportSuffix + "_Max"),
                MaxPerBall = reader.ToDecimal("maxperball" + sportSuffix),
                MaxPerBallMax = reader.ToDecimal("maxperball" + sportSuffix + "_Max"),
                Id = sportId
            };

            return sport;
        }

        public async Task<IResultInfo> UpdateSettingAsync(PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel)
        {
            const string UspName = "Lrf_CustomerUpdMember_MinMaxBet161";

            using (var db = _db.GetObjectDb(UspName))
            {
                AddUpdateParameter(db, metaModel);

                await db.ExecuteNonQueryAsync();

                var errorCode = db.GetParameterValue("ErrCode");
                var errorMessage = db.GetParameterValue("ErrMsg");

                return new ResultInfo
                {
                    Code = Converter.ToInt(errorCode),
                    Message = Converter.ToString(errorMessage)
                };
            }
        }

        private static void AddUpdateParameter(IObjectDb db, PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel)
        {
            db.SetParameterValue("ip", metaModel.Ip);
            db.SetParameterValue("language", metaModel.Language);
            db.SetParameterValue("userid", metaModel.UserId);
            db.SetParameterValue("custid", metaModel.CustId);
            AddUpdateSettingParameter(db, metaModel.Setting);

            db.SetParameterValue("ErrCode", null);
            db.SetParameterValue("ErrMsg", null);
        }

        private static void AddUpdateSettingParameter(IObjectDb db, IEnumerable<PlayerBetLimitMetaModel> setting)
        {
            foreach (var type in SportTypes)
            {
                var item = setting.FirstOrDefault(x => x.Id == type);

                db.SetParameterValue("minbetv" + type, item?.MinBet);
                db.SetParameterValue("maxbetv" + type, item?.MaxBet);
                db.SetParameterValue("maxpermatchv" + type, item?.MaxPerMatch);
                db.SetParameterValue("maxperballv" + type, item?.MaxPerBall);
            }
        }
    }
}