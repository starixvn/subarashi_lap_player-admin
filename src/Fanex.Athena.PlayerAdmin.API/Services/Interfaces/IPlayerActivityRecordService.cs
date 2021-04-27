using Fanex.Athena.PlayerAdmin.API.DTO;
using Fanex.Athena.PlayerAdmin.API.MetaModel;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.Services.Interfaces
{
    public interface IPlayerActivityRecordService
    {
        Task<PlayerActivityRecord> GetActivityRecord(PlayerActivityRecordMetaModel metaModel);
    }
}