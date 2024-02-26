namespace Fanex.Athena.PlayerAdmin.Core.Services
{
    public interface IResourceService
    {
        string GetBettype(object bettypeId);

        string GetSportName(object sportId, string languageCode = null);

        string GettResourceData(object refId, object resourceId);
    }
}