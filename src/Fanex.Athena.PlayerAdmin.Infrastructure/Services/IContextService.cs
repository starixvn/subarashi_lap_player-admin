using System.Collections.Generic;

namespace Fanex.Athena.PlayerAdmin.Infrastructure.Services
{
    public interface IContextService
    {
        int LicenseeId { get; }
        string SiteName { get; }
        int UserId { get; }
        string Username { get; }
        IReadOnlyDictionary<int, int[]> AgentList { get; }
        IReadOnlyDictionary<int, string> CurrencyList { get; }
        bool IsAllowedCustomerComm { get; }
        bool IsEnableBA { get; }
        public int UserLevel { get; }
        string ClientIP { get; }
        string Language { get; }
        bool IsInRoles(params string[] roles);
    }
}