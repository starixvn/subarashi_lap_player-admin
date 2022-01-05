using Fanex.Athena.PlayerAdmin.API.DTO;
using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.API.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Fanex.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.Services
{
    public class PlayerListService : IPlayerListService
    {
        private readonly IDbFactory _dbFactory;
        private readonly IContextService _contextService;

        public PlayerListService(IDbFactory dbFactory, IContextService contextService)
        {
            _dbFactory = dbFactory;
            _contextService = contextService;
        }

        public async Task<PlayerListResultViewModel> Search(PlayerSearchingMetaModel metaModel)
        {
            const string SpKey = "Lrf_CustomerSelByUsername";
            var result = new PlayerListResultViewModel { DataList = new List<Player>() };
            if (metaModel.IsDeposited == 0)
            {
                return result;
            }

            using (var objectDb = _dbFactory.GetObjectDb(SpKey))
            {
                AssignCommonParam(objectDb, metaModel);
                AssignParamByUserName(objectDb, metaModel);

                result.DataList = await ReadPlayerList(objectDb);
                result.TotalItem = GetTotalItem(objectDb);
            }

            return result;
        }

        public async Task<PlayerListResultViewModel> GetPlayerListById(PlayerListByIdMetaModel metaModel)
        {
            const string SpKey = "Lrf_CustomerSelByCustid";
            var result = new PlayerListResultViewModel();

            using (var objectDb = _dbFactory.GetObjectDb(SpKey))
            {
                AssignCommonParam(objectDb, metaModel);
                AssignParamByCustId(objectDb, metaModel);

                result.DataList = await ReadPlayerList(objectDb);
                result.TotalItem = GetTotalItem(objectDb);
            }

            return result;
        }

        public async Task<PlayerListResultViewModel> ArchivedSearch(PlayerArchivedSearchingMetaModel metaModel)
        {
            const string SpKey = "Lrf_DormantCustomerSel";
            var result = new PlayerListResultViewModel();

            using (var objectDb = _dbFactory.GetObjectDb(SpKey))
            {
                objectDb.SetParameterValue("AgentList", metaModel.AgentListXml);

                var usernameTextSearch = ToSqlTextSearch(metaModel.UserName);
                if (!string.IsNullOrWhiteSpace(usernameTextSearch))
                {
                    objectDb.SetParameterValue("UserName", usernameTextSearch);
                    objectDb.SetParameterValue("SearchOption", metaModel.SearchOption);
                }

                objectDb.SetParameterValue("BaseCurrency", string.Empty);
                objectDb.SetParameterValue("PageSize", metaModel.PageSize);
                objectDb.SetParameterValue("PageIndex", metaModel.PageIndex);

                result.DataList = await ReadArchivedPlayerList(objectDb);
                result.TotalItem = GetTotalItem(objectDb);
            }

            return result;
        }

        private static void AssignCommonParam(IObjectDb objectDb, PlayerListBaseMetaModel metaModel)
        {
            objectDb.SetParameterValue("LicenseeName", metaModel.SiteName);
            objectDb.SetParameterValue("AgentList", metaModel.AgentListXml);
            objectDb.SetParameterValue("CustomerClass", metaModel.CustomerClass);
            objectDb.SetParameterValue("CommGroups", metaModel.CommGroups);
        }

        private static void AssignParamByUserName(IObjectDb objectDb, PlayerSearchingMetaModel metaModel)
        {
            objectDb.SetParameterValue("UserName", ToSqlTextSearch(metaModel.UserName));
            objectDb.SetParameterValue("SearchOption", metaModel.SearchOption);
            objectDb.SetParameterValue("BaFilter", metaModel.BaFilter);
            objectDb.SetParameterValue("SpecialLevel", metaModel.SpecialLevel);
            objectDb.SetParameterValue("OddsGroups", metaModel.OddsGroup);
            objectDb.SetParameterValue("PageSize", metaModel.PageSize);
            objectDb.SetParameterValue("PageIndex", metaModel.PageIndex);

            objectDb.SetParameterValue("TotalRows", 0);
        }

        private static void AssignParamByCustId(IObjectDb objectDb, PlayerListByIdMetaModel metaModel)
        {
            objectDb.SetParameterValue("CustIds", metaModel.IdList);
            objectDb.SetParameterValue("BaFilter", metaModel.BaFilter);
            objectDb.SetParameterValue("SpecialLevel", metaModel.SpecialLevel);
            objectDb.SetParameterValue("OddsGroups", metaModel.OddsGroup);
            objectDb.SetParameterValue("PageSize", metaModel.PageSize);
            objectDb.SetParameterValue("PageIndex", metaModel.PageIndex);

            objectDb.SetParameterValue("TotalRows", 0);
        }

        private static async Task<IEnumerable<Player>> ReadPlayerList(IObjectDb objectDb)
        {
            var playerList = new List<Player>();

            using (var reader = await objectDb.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    playerList.Add(new Player
                    {
                        IbcCustId = Convert.ToInt32(reader["CustId"]),
                        CurrencyId = Convert.ToByte(reader["ExchangeId"]),
                        Currency = reader["Currency"].ToString(),
                        SystemId = reader["UserName"].ToString(),
                        UserName = reader["NickName"].ToString(),
                        BaStatus = Convert.ToBoolean(reader["BAStatus"]),
                        SpecialLevel = Convert.ToInt32(reader["SpecialLevel"]),
                        OddsGroup = reader["OddsGroup"].ToString().ToUpperInvariant(),
                        CurrentBalance = Convert.ToDecimal(reader["TotalBalance"]),
                        OutstandingBalance = Convert.ToDecimal(reader["Outstanding"]),
                        CommissionGroup = Convert.ToString(reader["GroupName"]),
                        CustomerClass = Convert.ToString(reader["CustomerClass"])
                    });
                }
            }

            return playerList;
        }

        private async Task<IEnumerable<Player>> ReadArchivedPlayerList(IObjectDb objectDb)
        {
            var playerList = new List<Player>();
            var currencyDic = _contextService.CurrencyList;

            using (var reader = await objectDb.ExecuteReaderAsync())
            {                
                while (reader.Read())
                {
                    var currencyId = Convert.ToByte(reader["currency"]);
                    var currency = currencyDic.ContainsKey(currencyId) ? currencyDic[currencyId] : string.Empty;
                    const int ArchivedStatus = 3;

                    playerList.Add(new Player
                    {
                        IbcCustId = reader.ToInt("custid"),
                        SystemId = reader.ToString("username"),
                        UserName = reader.ToString("nickname"),
                        CurrentBalance = reader.ToDecimal("balance"),
                        CurrencyId = currencyId,
                        Currency = currency,
                        Status = ArchivedStatus
                    });
                }
            }

            return playerList;
        }

        private static int GetTotalItem(IObjectDb objectDb)
            => Convert.ToInt32(objectDb.GetParameterValue("TotalRows"));

        private static string ToSqlTextSearch(string text)
            => string.IsNullOrEmpty(text) ? text : text.Trim().Replace("_", "[_]");
    }
}