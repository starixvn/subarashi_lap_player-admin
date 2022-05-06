using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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

        public bool IsEnabledSubsidiary => GetClaimValue<bool>("IsEnabledSubsidiary");

        public bool IsMasterLicensee => GetClaimValue<bool>("IsMasterLicensee");

        public int UserLevel => GetClaimValue<int>("UserLevel");

        public string OperatorID => GetClaimValue("OperatorID");

        public int? WhiteLabelID
        {
            get
            {
                var value = GetClaimValue<int>("WhiteLabelID");
                if (value < 0)
                {
                    return null;
                }
                return value;
            }
        }

        public byte SubsidiaryViewType
        {
            get
            {
                if (IsEnabledSubsidiary && UserLevel < Constants.UserLevelType.Subsidiary)
                {
                    if (IsMasterLicensee && !WhiteLabelID.HasValue)
                    {
                        return Constants.SubsidiaryViewType.MasterManagement;
                    }

                    return Constants.SubsidiaryViewType.NormalManagement;
                }
                else
                {
                    return Constants.SubsidiaryViewType.NotManagement;
                }
            }
        }

        public string SiteName => GetClaimValue("SiteName");

        public int UserId => GetClaimValue<int>("UserSEQ");

        public string Username => GetClaimValue("UserID");

        public string Language => CultureInfo.CurrentUICulture.Name;

        public bool IsShowIP => GetClaimValue<bool>("IsShowIP");

        public bool IsShowIpVN => IsInRoles("super_admin", "bl_view_ipvn");

        public bool IsFakeLicensee => GetClaimValue("IsFakeLicensee", false);

        public IEnumerable<string> OfferedSubProductList
        {
            get
            {
                var offeredSubProductList = GetClaimValue("OfferedSubProducts");
                return string.IsNullOrEmpty(offeredSubProductList) ? Enumerable.Empty<string>() : offeredSubProductList.Split(',');
            }
        }

        public IEnumerable<string> OfferedOddsGroupList
        {
            get
            {
                var offeredOddsGroupList = GetClaimValue("OfferedOddsGroups");
                return string.IsNullOrEmpty(offeredOddsGroupList) ? Enumerable.Empty<string>() : offeredOddsGroupList.Split(',');
            }
        }

        public Task<string> AccessToken => Context.GetTokenAsync(CookieAuthenticationDefaults.AuthenticationScheme, "access_token");

        public bool IsInRoles(params string[] roles) => roles?.Any(_ => User.IsInRole(_)) ?? false;

        public string CurrentRequestClientIP => Context.Connection.RemoteIpAddress.ToString();

        public string OwnerClientIP => GetClaimValue("ClientIP");

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

        private T GetClaimValue<T>(string claimType, T _default)
        {
            var claimValue = GetClaimValue(claimType);
            if(string.IsNullOrEmpty(claimValue))
            {
                return _default;
            }
            return (T)Convert.ChangeType(claimValue, typeof(T));
        }

        private string GetClaimValue(string claimType)
            => User.FindFirst(claimType)?.Value;

        protected internal class CurrencyAgentInfo
        {
            public int[] AgentId { get; set; }
            public int CurrencyId { get; set; }
            public string Currency { get; set; }
        }
    }
}