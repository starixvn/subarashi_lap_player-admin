using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.Infrastructure.Models;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.Services.Interfaces
{
    public interface IPlayerSpecialLevelService
    {
        Task<ResultViewModel> Update(SpecialLevelUpdatingMetaModel metaModel);
    }
}