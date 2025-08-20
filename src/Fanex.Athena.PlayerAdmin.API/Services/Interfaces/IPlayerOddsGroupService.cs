namespace Fanex.Athena.PlayerAdmin.API.Services.Interfaces
{
    using System.Threading.Tasks;
    using Core.Models;
    using MetaModel;

    public interface IPlayerOddsGroupService
    {
        Task<ResultViewModel> Update(OddsGroupUpdatingMetaModel metaModel);
    }
}