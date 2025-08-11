namespace Fanex.Athena.PlayerAdmin.API.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Core.Extensions;
    using Core.Repositories;
    using Core.Resources;
    using Core.Settings;
    using DTO;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using ViewModels;

    public class DepositSettingService : IDepositSettingService
    {
        private static readonly TimeSpan FifteenMinutes = TimeSpan.FromMinutes(15);

        private readonly IDbFactory _dbFactory;
        private readonly IMemoryCache _cache;
        private readonly HttpClient _httpClient;
        private readonly AppSetting _appSetting;
        private readonly ILogger<DepositSettingService> _logger;

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
            var riskLevelFromAPI = await _cache.TryGetOrAddFromCache($"RiskLevelList_{language}", FifteenMinutes, GetRiskLevelFromApi);
            var riskLevel = _cache.TryGetOrAddFromCacheWithoutExpired($"RiskLevelList_{language}_backup", riskLevelFromAPI);

            var result = new List<RiskLevel> { new RiskLevel {
                Id = riskLevel.Count().ToString(),
                Key = RiskLevel.Uncategorized,
                Name = ViewResources.ResourceManager.GetString(RiskLevel.Uncategorized),
                CustomerClassList = new[] { "0" }
            }};
            result.AddRange(riskLevel);

            return result.OrderBy(_ => _, new RiskLevelComparer());
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
                            Id = reader.ToString("GroupDiscountID"),
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
                response.EnsureSuccessStatusCode();
                var customerClassList = JsonConvert.DeserializeObject<List<CustomerClassApiModel>>(await response.Content.ReadAsStringAsync());

                var riskLevelList = customerClassList.GroupBy(_ => _.SabaRiskLevel).Select((_, index) => new RiskLevel
                {
                    Id = index.ToString(),
                    Key = _.Key,
                    Name = ViewResources.ResourceManager.GetString(_.Key) ?? _.Key,
                    CustomerClassList = _.Select(_ => _.CustomerClass.ToString()).Distinct().ToList()
                });

                return riskLevelList;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Error occurred while calling CustomerClass api {_appSetting.CustomerClassUrl}");
                return Enumerable.Empty<RiskLevel>();
            }
        }
    }
}