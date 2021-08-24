using Fanex.Athena.PlayerAdmin.API.DTO;
using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.API.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Fanex.Data;
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
    internal class CustomerClassApiModel
    {
        public int CustomerClass { get; set; }
    }

    public class PlayerListService : IPlayerListService
    {
        private readonly IDbFactory _dbFactory;
        private readonly HttpClient _httpClient;
        private readonly AppSetting _appSetting;

        public PlayerListService(IDbFactory dbFactory, HttpClient httpClient, IOptions<AppSetting> appSetting)
        {
            _dbFactory = dbFactory;
            _httpClient = httpClient;
            _appSetting = appSetting.Value;
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

        public async Task<IEnumerable<string>> GetCustomerClassList()
        {
            var customerClassFromApi = await GetCustomerClassFromApi();
            var result = new List<string> { "0" };
            result.AddRange(customerClassFromApi
                .OrderBy(_ => _.CustomerClass)
                .Select(_ => _.CustomerClass.ToString())
                );
            return result.Distinct();
        }

        public async Task<IEnumerable<CommGroupViewModel>> GetAllCommGroups(int licenseeId)
        {
            var result = new List<CommGroupViewModel> { new CommGroupViewModel() };

            using (var objectDb = _dbFactory.GetObjectDb("Lrf_LicenseeGroupCommSel"))
            {
                objectDb.SetParameterValue("LicenseeID", licenseeId);

                using (var reader = await objectDb.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        result.Add(new CommGroupViewModel
                        {
                            Id = reader.ToString("GroupCommId"),
                            Name = reader.ToString("GroupName")
                        });
                    }
                }
            }

            return result;
        }

        private async Task<IEnumerable<CustomerClassApiModel>> GetCustomerClassFromApi()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(_appSetting.CustomerClassUrl));
            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var customerClassList = JsonConvert.DeserializeObject<IEnumerable<CustomerClassApiModel>>(await response.Content.ReadAsStringAsync());
            return customerClassList;
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

        private static int GetTotalItem(IObjectDb objectDb)
            => Convert.ToInt32(objectDb.GetParameterValue("TotalRows"));

        private static string ToSqlTextSearch(string text)
            => string.IsNullOrEmpty(text) ? text : text.Trim().Replace("_", "[_]");
    }
}