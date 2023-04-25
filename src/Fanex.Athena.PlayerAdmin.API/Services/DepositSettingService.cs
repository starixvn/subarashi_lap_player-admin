using Fanex.Athena.PlayerAdmin.API.DTO;
using Fanex.Athena.PlayerAdmin.API.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Microsoft.Extensions.Caching.Memory;
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
        private static readonly TimeSpan FifteenMinutes = TimeSpan.FromMinutes(15);

        private readonly IDbFactory _dbFactory;
        private readonly IMemoryCache _cache;
        private readonly HttpClient _httpClient;
        private readonly AppSetting _appSetting;
        private readonly ILogger<DepositSettingService> _logger;

        private readonly string[] RiskLevelKeyByOrder = new[] { "New", "Healthy", "Moderate", "Suspicious", "Dangerous", "Problems", "Robot", RiskLevel.Uncategorized };

        public DepositSettingService(IDbFactory dbFactory, HttpClient httpClient,
            IMemoryCache cache,
            IOptions<AppSetting> appSetting,
            ILogger<DepositSettingService> logger)
        {
            _dbFactory = dbFactory;
            _httpClient = httpClient;
            _cache = cache;
            _appSetting = appSetting.Value;
            _logger = logger;
        }

        public async Task<IEnumerable<RiskLevel>> GetRiskLevelList(string language)
        {
            return await _cache.TryGetOrAddFromCache($"RiskLevelList_{language}", FifteenMinutes, GetRiskLevelFromApi);
        }

        public async Task<IEnumerable<CommGroupViewModel>> GetCommGroupList(int licenseeId)
        {
            var commGroups = new List<CommGroupViewModel> { new CommGroupViewModel() };
            var commGroupsFromDB = await _cache.TryGetOrAddFromCache($"CommGroupList_{licenseeId}", FifteenMinutes, () => GetCommGroupFromDB(licenseeId));
            commGroups.AddRange(commGroupsFromDB);
            return commGroups;
        }

        public async Task<IEnumerable<CommGroupViewModel>> GetCommGroupFromDB(int licenseeId)
        {
            var commGroupsFromDB = new List<CommGroupViewModel>();

            using (var objectDb = _dbFactory.GetObjectDb("Lrf_LicenseeGroupCommSel"))
            {
                objectDb.SetParameterValue("LicenseeID", licenseeId);

                using (var reader = await objectDb.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        commGroupsFromDB.Add(new CommGroupViewModel
                        {
                            Id = reader.ToString("GroupCommId"),
                            Name = reader.ToString("GroupName")
                        });
                    }
                }
            }

            return commGroupsFromDB;
        }

        private async Task<IEnumerable<RiskLevel>> GetRiskLevelFromApi()
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(_appSetting.CustomerClassUrl));
                using var response = await _httpClient.SendAsync(request);
                var RiskLevelList = new List<RiskLevel>();
                response.EnsureSuccessStatusCode();
                var customerClassList = JsonConvert.DeserializeObject<IEnumerable<CustomerClassApiModel>>(await response.Content.ReadAsStringAsync());
                foreach (var key in RiskLevelKeyByOrder)
                {
                    RiskLevelList.Add(new RiskLevel
                    {
                        Id = RiskLevelList.Count.ToString(),
                        Key = key,
                        Name = ViewResources.ResourceManager.GetString(key),
                        CustomerClassList = key == RiskLevel.Uncategorized ? new[] { "0" }
                            : customerClassList.Where(_ => _.CustomerClassName.Contains(key, StringComparison.OrdinalIgnoreCase)).Select(_ => _.CustomerClass.ToString()).Distinct()
                    });
                }
                return RiskLevelList;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Error occurred while calling CustomerClass api {_appSetting.CustomerClassUrl}");
                return Enumerable.Empty<RiskLevel>();
            }
        }
    }
}