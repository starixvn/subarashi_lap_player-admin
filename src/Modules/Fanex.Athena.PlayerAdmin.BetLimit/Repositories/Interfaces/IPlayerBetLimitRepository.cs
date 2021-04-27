using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces
{
    public interface IPlayerBetLimitRepository
    {
        Task<IEnumerable<PlayerBetLimitProduct>> GetEnableBetLimitProducts(string siteName);
    }
}