using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.Services
{
    public interface IContextService
    {
        int LicenseeId { get; }
        string SiteName { get; }
        int UserId { get; }
        string Username { get; }
        IReadOnlyDictionary<int, int[]> AgentList { get; }
        IReadOnlyDictionary<int, string> CurrencyList { get; }
        string SiteDBServer { get; }
        bool IsAllowedCustomerComm { get; }
        bool IsEnableBA { get; }
        bool IsEnableSingleWallet { get; }
        bool IsEnabledSubsidiary { get; }
        bool IsMasterLicensee { get; }
        int UserLevel { get; }
        int? WhiteLabelID { get; }
        string OperatorID { get; }
        byte SubsidiaryViewType { get; }
        string CurrentRequestClientIP { get; }
        string OwnerClientIP { get; }
        string Language { get; }
        bool IsShowIP { get; }
        bool IsShowIpVN { get; }
        bool IsShowSabaIntelligence { get; }
        bool IsFakeLicensee { get; }
        IEnumerable<string> OfferedSubProductList { get; }
        IEnumerable<string> OfferedOddsGroupList { get; }
        Task<string> AccessToken { get; }
        int SpecialPlatform { get; }
        bool IsInRoles(params string[] roles);
    }
}