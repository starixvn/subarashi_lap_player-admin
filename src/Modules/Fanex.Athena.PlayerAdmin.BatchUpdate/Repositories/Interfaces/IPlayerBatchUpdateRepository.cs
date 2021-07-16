using Fanex.Athena.PlayerAdmin.BatchUpdate.Criteria;
using Fanex.Athena.PlayerAdmin.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BatchUpdate.Repositories
{
    public interface IPlayerBatchUpdateRepository
    {
        Task<IResultInfo> Update(BasePlayerBatchUpdateCriteria criteria);

        Task<IResultInfo> UpdateBAStatus(UpdateBAStatusCriteria criteria);

        Task<IDictionary<string, string>> GetOldValues(IEnumerable<int> custIds, int type);
    }
}