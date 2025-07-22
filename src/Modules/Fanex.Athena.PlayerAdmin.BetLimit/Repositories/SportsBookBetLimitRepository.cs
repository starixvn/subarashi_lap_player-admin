using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces;
using Fanex.Data;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Repositories
{
    public class SportsBookBetLimitRepository : ISportsBookBetLimitRepository
    {
        private const string MixedSportId = "99MP";
        private const string SoccerParlay = "1MP";

        private static readonly string[] SportTypes =
        {
            SportType.Soccer.ToString(),
            SoccerParlay,
            SportType.BasketBall.ToString(),
            SportType.FootBall.ToString(),
            SportType.Tennis.ToString(),
            SportType.BaseBall.ToString(),
            SportType.Golf.ToString(),
            SportType.Motorsports.ToString(),
            SportType.OtherSports.ToString()
        };

        private readonly IDbFactory _db;

        public SportsBookBetLimitRepository(IDbFactory dbFactory)
        {
            _db = dbFactory;
        }

        public async Task<IEnumerable<PlayerBetLimit>> GetSettingAsync(int custId, bool isShowMixedSport)
        {
            const string UspName = "Lrf_CustomerSelEditCustomer";

            using (var db = _db.GetObjectDb(UspName))
            {
                db.SetParameterValue("CustID", custId);
                using (var reader = await db.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        return ReadSetting(reader, isShowMixedSport);
                    }
                }
            }

            return Enumerable.Empty<PlayerBetLimit>();
        }

        public async Task<IResultInfo> UpdateSettingAsync(PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel, bool isShowMixedSport)
        {
            const string UspName = "Lrf_CustomerUpdMember_All";

            using (var db = _db.GetObjectDb(UspName))
            {
                AddUpdateParameter(db, metaModel, isShowMixedSport);

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

        private static List<PlayerBetLimit> ReadSetting(IDataReader reader, bool isShowMixedSport)
        {
            var sportList = new List<PlayerBetLimit>();

            foreach (var type in SportTypes)
            {
                sportList.Add(ReadForSport(reader, type));
            }

            if (isShowMixedSport)
            {
                sportList.Add(ReadForSport(reader, MixedSportId));
            }

            return sportList;
        }

        private static PlayerBetLimit ReadForSport(IDataReader reader, string sportId)
        {
            var sportSuffix = GetSportSuffix(sportId);

            var sport = new PlayerBetLimit
            {
                MaxBet = reader.ToDecimal("maxbet" + sportSuffix),
                MinBet = reader.ToDecimal("minbet" + sportSuffix),
                MaxPerMatch = reader.ToDecimal("MaxPerMatch" + sportSuffix),
                MaxPerMatchMax = reader.ToDecimal("MaxPerMatch" + sportSuffix + "_Max"),
                MaxPerMatchMin = reader.ToDecimal("MaxPerMatch" + sportSuffix + "_Min"),
                BetValueMax = reader.ToDecimal("MaxBet" + sportSuffix + "_Min"),
                BetValueMin = reader.ToDecimal("MinBet" + sportSuffix + "_Max"),
            };

            return sport;
        }

        private static void AddUpdateParameter(IObjectDb db, PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel, bool isShowMixedSport)
        {
            db.SetParameterValue("licenseeName", metaModel.SiteName);
            db.SetParameterValue("ip", metaModel.Ip);
            db.SetParameterValue("language", metaModel.Language);
            db.SetParameterValue("userid", metaModel.UserId);
            db.SetParameterValue("custid", metaModel.CustId);
            AddUpdateSettingParameter(db, metaModel.Setting, isShowMixedSport);

            db.SetParameterValue("ErrCode", null);
            db.SetParameterValue("ErrMsg", null);
        }

        private static void AddUpdateSettingParameter(IObjectDb db, IEnumerable<PlayerBetLimitMetaModel> setting, bool isShowMixedSport)
        {
            foreach (var type in SportTypes)
            {
                var item = setting.FirstOrDefault(x => x.Id == type);
                var sportSuffix = GetSportSuffix(type);

                AddSportSettingParameter(db, item, sportSuffix);
            }

            var mixSport = isShowMixedSport ? setting.FirstOrDefault(x => x.Id == MixedSportId) : null;

            AddSportSettingParameter(db, mixSport, GetSportSuffix(MixedSportId));
        }

        private static void AddSportSettingParameter(IObjectDb db, PlayerBetLimitMetaModel setting, string sportSuffix)
        {
            db.SetParameterValue("minbet" + sportSuffix, setting?.MinBet);
            db.SetParameterValue("maxbet" + sportSuffix, setting?.MaxBet);
            db.SetParameterValue("maxpermatch" + sportSuffix, setting?.MaxPerMatch);
            db.SetParameterValue("updateminmaxbet" + sportSuffix, setting?.IsUpdated);
        }

        private static string GetSportSuffix(string sportId) => sportId != SportType.Soccer.ToString() ? "v" + sportId : string.Empty;
    }
}