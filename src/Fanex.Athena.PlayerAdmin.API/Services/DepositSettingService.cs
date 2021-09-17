using Fanex.Athena.PlayerAdmin.API.DTO;
using Fanex.Athena.PlayerAdmin.API.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.Services
{
    public class DepositSettingService : IDepositSettingService
    {
        private readonly IDbFactory _dbFactory;
        private readonly HttpClient _httpClient;
        private readonly AppSetting _appSetting;
        private readonly ILogger<DepositSettingService> _logger;

        public DepositSettingService(IDbFactory dbFactory, HttpClient httpClient,
            IOptions<AppSetting> appSetting,
            ILogger<DepositSettingService> logger)
        {
            _dbFactory = dbFactory;
            _httpClient = httpClient;
            _appSetting = appSetting.Value;
            _logger = logger;
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

        public async Task<IEnumerable<CommGroupViewModel>> GetCommGroupList(int licenseeId)
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
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(_appSetting.CustomerClassUrl));
                using var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var customerClassList = JsonConvert.DeserializeObject<IEnumerable<CustomerClassApiModel>>(await response.Content.ReadAsStringAsync());
                return customerClassList;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Error occurred while calling CustomerClass api {_appSetting.CustomerClassUrl}");
                return Enumerable.Empty<CustomerClassApiModel>();
            }
        }
    }
}