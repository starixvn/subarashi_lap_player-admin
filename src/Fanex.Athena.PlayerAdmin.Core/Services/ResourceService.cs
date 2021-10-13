using Fanex.RemotingRef;
using System.Globalization;
using System.Threading;

namespace Fanex.Athena.PlayerAdmin.Core.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IRemotingRefService _remotingRefService;

        public ResourceService(IRemotingRefService remotingRefService)
        {
            _remotingRefService = remotingRefService;
        }

        public string GetBettype(object bettypeId)
        {
            if (!IsValidId(bettypeId))
            {
                return string.Empty;
            }

            return _remotingRefService.GetBetTypeName(bettypeId.ToString(), GetCurrentIBCLanguageCode());
        }

        public string GetSportName(object sportId, string languageCode = null)
        {
            if (!IsValidId(sportId))
            {
                return string.Empty;
            }

            return _remotingRefService.GetSportName(sportId.ToString(), GetCurrentIBCLanguageCode(languageCode), string.Empty);
        }

        private static bool IsValidId(object id)
        {
            return id != null && id.ToString() != string.Empty && id.ToString() != "0";
        }

        private static string GetCurrentIBCLanguageCode(string languageCode = null)
        {
            return string.IsNullOrEmpty(languageCode) ? CultureInfo.CurrentUICulture.Name : languageCode;
        }
    }
}