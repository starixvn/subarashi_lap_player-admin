namespace Fanex.Athena.PlayerAdmin.Infrastructure.Services
{
    public interface IResourceService
    {
        string GetBettype(object bettypeId);

        string GetSportName(object sportId, string languageCode = null);
    }
}