using Fanex.Athena.PlayerAdmin.Core.Settings;
using Fanex.Athena.PlayerAdmin.MVC.DTO;
using Fanex.Athena.PlayerAdmin.MVC.MetaModel;
using Fanex.Athena.PlayerAdmin.MVC.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.MVC.Services
{
    public class PlayerStatementService : IPlayerStatementService
    {
        private const string FormatDate = "MM-dd-yyyy";

        private const int PageSize = 1000;

        private readonly StatementSetting _setting;

        private readonly HttpClient _client;

        public PlayerStatementService(IOptions<StatementSetting> setting, HttpClient client)
        {
            _setting = setting.Value;
            _client = client;
        }

        public async Task<string> GetUrl(PlayerStatementMetaModel metaModel)
        {
            const string GetStatementPath = @"{0}Statement/Summary?cid={1}&fd={2}&td={3}&ps={4}&sb={5}&lang={6}&access_token={7}&isDesktopViewOnly=true";

            var token = await GetToken(metaModel.SiteName).ConfigureAwait(false);

            return string.Format(
                GetStatementPath,
                _setting.ExternalHost,
                metaModel.CustId,
                metaModel.FromDate.ToString(FormatDate),
                metaModel.ToDate.ToString(FormatDate),
                PageSize,
                true,
                metaModel.Language,
                token);
        }

        private async Task<string> GetToken(string siteName)
        {
            const string GetTokenPath = "api/auth";
            using (var request = new HttpRequestMessage(HttpMethod.Post, new Uri(GetTokenPath, UriKind.Relative)))
            {
                request.Content = new StringContent(GetAuthenticationObjectAsString(siteName), Encoding.UTF8, "application/json");
                request.Headers.CacheControl = new CacheControlHeaderValue { NoCache = true, NoStore = true, MaxAge = new TimeSpan(0) };
                using (var response = await _client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    var result = response.StatusCode == HttpStatusCode.OK ? JsonConvert.DeserializeObject<StatementToken>(content) : default;
                    response.Content = null;
                    return result?.AccessToken;
                }
            }
        }

        private string GetAuthenticationObjectAsString(string siteName)
        {
            return JsonConvert.SerializeObject(new
            {
                UserName = $"{siteName}@{_setting.User}",
                Password = _setting.Password
            });
        }
    }
}