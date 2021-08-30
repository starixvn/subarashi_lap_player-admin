using Fanex.Athena.PlayerAdmin.Core.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.Services
{
    public class ContextService : IContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected HttpContext Context => _httpContextAccessor.HttpContext;
        protected ClaimsPrincipal User => Context.User;

        public ContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int LicenseeId => GetClaimValue<int>("LicenseeId");

        public bool IsAllowedCustomerComm => GetClaimValue<bool>("IsAllowedCustomerComm");

        public bool IsEnableBA => GetClaimValue<bool>("IsEnableBA");

        public bool IsEnableSingleWallet => GetClaimValue<bool>("IsEnableSingleWallet");

        public int UserLevel => GetClaimValue<int>("UserLevel");

        public string SiteName => GetClaimValue("SiteName");

        public int UserId => GetClaimValue<int>("UserSEQ");

        public string Username => GetClaimValue("UserID");

        public string Language => Languages.LanguageCodeMappers[GetClaimValue("Language")];

        public bool IsShowIP => GetClaimValue<bool>("IsShowIP");

        public bool IsShowIpVN => IsInRoles("super_admin", "bl_view_ipvn");

        public Task<string> AccessToken => Context.GetTokenAsync(CookieAuthenticationDefaults.AuthenticationScheme, "access_token");

        public bool IsInRoles(params string[] roles) => roles?.Any(_ => User.IsInRole(_)) ?? false;

        public string ClientIP => Context.Connection.RemoteIpAddress.ToString();

        private IReadOnlyDictionary<int, int[]> _agentList;

        public IReadOnlyDictionary<int, int[]> AgentList
        {
            get
            {
                if (_agentList is null)
                {
                    var currencyAgentList = RawCurrencyAgentList;
                    var dicClaims = currencyAgentList.ToDictionary(_ => _.CurrencyId, _ => _.AgentId);
                    _agentList = new ReadOnlyDictionary<int, int[]>(dicClaims);
                }
                return _agentList;
            }
        }

        private IReadOnlyDictionary<int, string> _currencyList;

        public IReadOnlyDictionary<int, string> CurrencyList
        {
            get
            {
                if (_currencyList is null)
                {
                    var currencyAgentList = RawCurrencyAgentList;
                    var dicClaims = currencyAgentList.ToDictionary(_ => _.CurrencyId, _ => _.Currency);
                    _currencyList = new ReadOnlyDictionary<int, string>(dicClaims);
                }
                return _currencyList;
            }
        }

        private IEnumerable<CurrencyAgentInfo> _rawCurrencyAgentList;

        protected internal IEnumerable<CurrencyAgentInfo> RawCurrencyAgentList
        {
            get
            {
                if (_rawCurrencyAgentList is null)
                {
                    var agentClaims = User.Claims.Where(_ => _.Type.Equals("CurrencyAgent", StringComparison.OrdinalIgnoreCase));
                    _rawCurrencyAgentList = agentClaims.Select(agent => JsonConvert.DeserializeObject<CurrencyAgentInfo>(agent.Value));
                }

                return _rawCurrencyAgentList;
            }
        }

        private T GetClaimValue<T>(string claimType)
            => (T)Convert.ChangeType(GetClaimValue(claimType), typeof(T));

        private string GetClaimValue(string claimType)
            => User.FindFirst(claimType)?.Value ?? throw new InvalidOperationException($"The {claimType} claim was not found");

        protected internal class CurrencyAgentInfo
        {
            public int[] AgentId { get; set; }
            public int CurrencyId { get; set; }
            public string Currency { get; set; }
        }
    }
}