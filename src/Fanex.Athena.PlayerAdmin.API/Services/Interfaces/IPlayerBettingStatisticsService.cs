namespace Fanex.Athena.PlayerAdmin.API.Services.Interfaces
{
    using System.Threading.Tasks;
    using DTO;
    using MetaModel;

    public interface IPlayerBettingStatisticsService
    {
        Task<PlayerBettingStatistics> GetPlayerBettingStatistics(PlayerBettingStatisticsMetaModel metaModel);
    }
}