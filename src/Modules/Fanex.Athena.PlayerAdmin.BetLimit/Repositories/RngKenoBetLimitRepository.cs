using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces;
using Fanex.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Repositories
{
    public class RngKenoBetLimitRepository : IRngKenoBetLimitRepository
    {
        private readonly IDbFactory _db;

        public RngKenoBetLimitRepository(IDbFactory dbFactory)
        {
            _db = dbFactory;
        }

        public async Task<IEnumerable<PlayerBetLimit>> GetSettingAsync(int custId)
        {
            const string UspName = "Lrf_CustomerSelEditKN";
            var sportList = new List<PlayerBetLimit>();

            using (var db = _db.GetObjectDb(UspName))
            {
                db.SetParameterValue("CustID", custId);
                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (reader.Read())
                    {
                        sportList.Add(new PlayerBetLimit
                        {
                            MaxBet = reader.ToDecimal("MaxBet"),
                            MinBet = reader.ToDecimal("MinBet"),
                            MaxPerMatch = reader.ToDecimal("MaxPerMatch"),
                            MaxPerMatchMax = reader.ToDecimal("MaxPerMatch_Max"),
                            BetValueMax = reader.ToDecimal("MaxBet_Min"),
                            BetValueMin = reader.ToDecimal("MinBet_Max"),
                            BetSettingType = reader.ToString("IBCBetTypeID"),
                            Id = reader.ToString("GameTypeID")
                        });
                    }
                }
            }

            return sportList;
        }

        public async Task<IResultInfo> UpdateSettingAsync(PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel)
        {
            const string UspName = "Lrf_CustomerUpdMember_MinMaxBetKN";

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

        private static void AddUpdateParameter(IObjectDb db, PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel)
        {
            db.SetParameterValue("ip", metaModel.Ip);
            db.SetParameterValue("language", metaModel.Language);
            db.SetParameterValue("userid", metaModel.UserId);
            db.SetParameterValue("custid", metaModel.CustId);
            db.SetParameterValue("dataxml", BuildBetLimitParameter(metaModel.Setting));

            db.SetParameterValue("ErrCode", null);
            db.SetParameterValue("ErrMsg", null);
        }

        private static string BuildBetLimitParameter(IEnumerable<PlayerBetLimitMetaModel> setting)
        {
            const string XMLPattern = "<settings>{0}</settings>";
            const string XMLElement = "<setting GameTypeID=\"{0}\" minbet=\"{1}\" maxbet=\"{2}\" maxpermatch=\"{3}\" ></setting>";

            var xmlElementBuilder = new StringBuilder();

            foreach (var item in setting)
            {
                xmlElementBuilder.Append(string.Format(XMLElement, item.Id, item.MinBet, item.MaxBet, item.MaxPerMatch));
            }

            return string.Format(XMLPattern, xmlElementBuilder);
        }
    }
}