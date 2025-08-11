namespace Fanex.Athena.PlayerAdmin.MVC.Services.Interfaces
{
    using System.Threading.Tasks;
    using MetaModel;

    public interface IPlayerStatementService
    {
        Task<string> GetUrl(PlayerStatementMetaModel metaModel);
    }
}
