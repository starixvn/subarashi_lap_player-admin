namespace Fanex.Athena.PlayerAdmin.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Constants;
    using Extensions;
    using Models;
    using Repositories;
    using Resources;

    public class SubsidiaryService : ISubsidiaryService
    {
        const string OtherWhiteLabelID = "-1";

        private readonly IDbFactory _dbFactory;

        public SubsidiaryService(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public bool IsIgnoredSubsidiarySearch(bool subsidiarySearch, string subsidiaryList, byte subsidiaryViewType)
            => subsidiaryViewType == SubsidiaryViewType.NotManagement || !subsidiarySearch || string.IsNullOrWhiteSpace(subsidiaryList);

        public async Task<IDictionary<int, int[]>> GetAgentList(string siteName, IReadOnlyDictionary<int, int[]> agentList, string subsidiaryList)
        {
            var needSubsidiaryList = subsidiaryList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var userSubsidiaryAgents = await GetUserSubsidiaryAgents(agentList, siteName);
            return userSubsidiaryAgents
                .Where(_ => needSubsidiaryList.Contains(_.OperatorId, StringComparer.OrdinalIgnoreCase))
                .GroupBy(_ => _.CurrencyId, (key, group) => KeyValuePair.Create(key, group.Select(_ => _.AgentId).ToArray()))
                .ToDictionary(_ => _.Key, _ => _.Value);
        }

        public async Task<IEnumerable<SelectTreeItem>> GetSubsidiaryList(string siteName, IReadOnlyDictionary<int, int[]> agentList, int? whiteLabelID, byte subsidiaryViewType)
        {
            if (subsidiaryViewType == SubsidiaryViewType.NotManagement)
            {
                return Enumerable.Empty<SelectTreeItem>();
            }

            var userSubsidiaries = await GetUserSubsidiaries(agentList, siteName, whiteLabelID);

            return subsidiaryViewType == SubsidiaryViewType.MasterManagement
                ? BuildWhiteLabelTree(userSubsidiaries)
                : BuildSubsidiaryTree(userSubsidiaries);
        }

        private static IEnumerable<SelectTreeItem> BuildSubsidiaryTree(IEnumerable<Subsidiary> subsidiaries)
            => subsidiaries
                .GroupBy(x => x.OperatorId,
                (key, _) => new SelectTreeItem { Value = key, Name = key, Checked = true }, StringComparer.OrdinalIgnoreCase);

        private static IEnumerable<SelectTreeItem> BuildWhiteLabelTree(IEnumerable<Subsidiary> subsidiaries)
            => subsidiaries
                .OrderByDescending(x => x.WhiteLabelID)
                .GroupBy(
                    x => x.WhiteLabelID,
                    (key, group) =>
                    {
                        var whiteLabelName = group.First().WhiteLabelName;
                        return new SelectTreeItem
                        {
                            Value = key?.ToString() ?? OtherWhiteLabelID,
                            Name = string.IsNullOrEmpty(whiteLabelName) ? ViewResources.Others : whiteLabelName,
                            Checked = true,
                            Children = group.Select(x => new SelectTreeItem { Value = x.OperatorId, Name = x.OperatorId, Checked = true })
                        };
                    });

        private async Task<IEnumerable<Subsidiary>> GetUserSubsidiaries(IReadOnlyDictionary<int, int[]> agentList, string siteName, int? whitteLabelId)
        {
            var licenseeSubsidiaries = await GetLicenseeSubsidiariesFromDB(siteName);
            var userSubsidiaryAgents = await GetUserSubsidiaryAgents(agentList, siteName);

            return licenseeSubsidiaries
                .Where(x =>
                    (!whitteLabelId.HasValue || x.WhiteLabelID == whitteLabelId)
                    && userSubsidiaryAgents.Any(o => o.OperatorId.Equals(x.OperatorId, StringComparison.OrdinalIgnoreCase)));
        }

        private async Task<IEnumerable<SubsidiaryAgent>> GetUserSubsidiaryAgents(IReadOnlyDictionary<int, int[]> agentList, string siteName)
        {
            var subsidiaryList = await GetLicenseeSubsidiaryAgentsFromDB(siteName);
            var userAgentList = agentList.SelectMany(i => i.Value);
            return subsidiaryList.Where(i => userAgentList.Contains(i.AgentId));
        }

        private async Task<IEnumerable<Subsidiary>> GetLicenseeSubsidiariesFromDB(string siteName)
        {
            var result = new List<Subsidiary>();

            const string SpName = "Portal_Sub_GetSubsidiaryList";
            const int reportQueryMode = 0;

            using var objectDb = _dbFactory.GetObjectDb(SpName);
            using var dataTable = new DataTable();

            dataTable.Columns.Add("Val", typeof(int));
            int[] statusList = { 0, 1, 2, 3 };
            foreach (var status in statusList)
            {
                dataTable.Rows.Add(status);
            }

            var parameter = new
            {
                SiteName = siteName,
                StatusList = dataTable,
                ForMgm = reportQueryMode
            };

            using var reader = await objectDb.ExecuteReaderAsync(parameter);
            while (reader.Read())
            {
                result.Add(new Subsidiary
                {
                    WhiteLabelID = reader["WhiteLabelID"] as int?,
                    WhiteLabelName = reader.ToString("Name"),
                    OperatorId = reader.ToString("OperatorId")
                });
            }

            return result;
        }

        private async Task<IEnumerable<SubsidiaryAgent>> GetLicenseeSubsidiaryAgentsFromDB(string siteName)
        {
            var result = new List<SubsidiaryAgent>();

            const string SpName = "Lrf_SubsidiarySel";

            using var objectDb = _dbFactory.GetObjectDb(SpName);
            objectDb.SetParameterValue("LicenseeName", siteName);

            using var reader = await objectDb.ExecuteReaderAsync();
            while (reader.Read())
            {
                result.Add(new SubsidiaryAgent
                {
                    AgentId = reader.ToInt("AgentID"),
                    OperatorId = reader.ToString("OperatorID"),
                    CurrencyId = reader.ToInt("Currency")
                });
            }

            return result;
        }
    }
}