namespace Fanex.Athena.PlayerAdmin.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface ISubsidiaryService
    {
        Task<IEnumerable<SelectTreeItem>> GetSubsidiaryList(string siteName, IReadOnlyDictionary<int, int[]> agentList, int? whiteLabelID, byte subsidiaryViewType);

        Task<IDictionary<int, int[]>> GetAgentList(string siteName, IReadOnlyDictionary<int, int[]> agentList, string subsidiaryList);

        bool IsIgnoredSubsidiarySearch(bool subsidiarySearch, string subsidiaryList, byte subsidiaryViewType);
    }
}