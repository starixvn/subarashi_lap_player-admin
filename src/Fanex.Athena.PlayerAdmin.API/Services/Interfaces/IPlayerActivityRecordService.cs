namespace Fanex.Athena.PlayerAdmin.API.Services.Interfaces
{
    using System.Threading.Tasks;
    using DTO;
    using MetaModel;

    public interface IPlayerActivityRecordService
    {
        Task<PlayerActivityRecord> GetActivityRecord(PlayerActivityRecordMetaModel metaModel);
    }
}