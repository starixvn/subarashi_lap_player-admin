namespace Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DTO;

    public interface IPlayerBetLimitRepository
    {
        Task<IEnumerable<PlayerBetLimitProduct>> GetEnableBetLimitProducts(string siteName, string environment);
    }
}