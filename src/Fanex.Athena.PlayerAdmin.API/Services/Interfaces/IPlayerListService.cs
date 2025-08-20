namespace Fanex.Athena.PlayerAdmin.API.Services.Interfaces
{
    using System.Threading.Tasks;
    using MetaModel;
    using ViewModels;

    public interface IPlayerListService
    {
        Task<PlayerListResultViewModel> Search(PlayerSearchingMetaModel metaModel);

        Task<PlayerListResultViewModel> GetPlayerListById(PlayerListByIdMetaModel metaModel);

        Task<PlayerListResultViewModel> ArchivedSearch(PlayerArchivedSearchingMetaModel metaModel);
    }
}
