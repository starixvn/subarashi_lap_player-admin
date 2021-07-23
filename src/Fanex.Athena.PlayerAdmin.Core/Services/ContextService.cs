using Fanex.Athena.PlayerAdmin.Core.Constants;
using Microsoft.AspNetCore.Authentication;
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

        public int LicenseeId => int.Parse(GetClaimValue("LicenseeId"));

        public bool IsAllowedCustomerComm => bool.Parse(GetClaimValue("IsAllowedCustomerComm"));

        public bool IsEnableBA => bool.Parse(GetClaimValue("IsEnableBA"));

        public int UserLevel => int.Parse(GetClaimValue("UserLevel"));

        public string SiteName => GetClaimValue("SiteName");

        public int UserId => int.Parse(GetClaimValue("UserSEQ"));

        public string Username => GetClaimValue("UserID");

        public string Language => Languages.LanguageCodeMappers[GetClaimValue("Language")];

        public bool IsShowIP => true;//bool.Parse(GetClaimValue("IsShowIP"));

        public bool IsShowIpVN => true;

        public Task<string> AccessToken => Context.GetTokenAsync(User.Identity.AuthenticationType, "access_token");

        public bool IsInRoles(params string[] roles) => roles?.Any(_ => User.IsInRole(_)) ?? false;

        private IReadOnlyDictionary<int, int[]> _agentList;

        public IReadOnlyDictionary<int, int[]> AgentList
        {
            get
            {
                if (_agentList is null)
                {
                    var currencyAgentList = GetCurrencyAgentList();
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
                    var currencyAgentList = GetCurrencyAgentList();
                    var dicClaims = currencyAgentList.ToDictionary(_ => _.CurrencyId, _ => _.Currency);
                    _currencyList = new ReadOnlyDictionary<int, string>(dicClaims);
                }
                return _currencyList;
            }
        }

        public string ClientIP => Context.Connection.RemoteIpAddress.ToString();

        private string GetClaimValue(string claimType) => User.FindFirst(claimType)?.Value ?? throw new InvalidOperationException($"{claimType} is not found");

        private IEnumerable<CurrencyAgentInfo> GetCurrencyAgentList()
        {
            var agentClaims = User.Claims.Where(_ => _.Type.Equals("CurrencyAgent", StringComparison.OrdinalIgnoreCase));
            var currencyAgentList = agentClaims.Select(agent => JsonConvert.DeserializeObject<CurrencyAgentInfo>(agent.Value));
            return currencyAgentList;
        }

        internal class CurrencyAgentInfo
        {
            public int[] AgentId { get; set; }
            public int CurrencyId { get; set; }
            public string Currency { get; set; }
        }
    }
}