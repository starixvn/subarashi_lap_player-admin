namespace Fanex.Athena.PlayerAdmin.BatchUpdate.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Models;
    using Criteria;

    public interface IPlayerBatchUpdateRepository
    {
        Task<IResultInfo> Update(BasePlayerBatchUpdateCriteria criteria);

        Task<IDictionary<string, string>> GetOldValues(IEnumerable<int> custIds, int type);
    }
}