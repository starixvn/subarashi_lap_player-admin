using Fanex.Athena.PlayerAdmin.Core.DataDomains;

namespace Fanex.Athena.PlayerAdmin.Core.Helpers
{
    public static class SabaHelper
    {
        public static bool IsHideSaba(int specialPlatform)
        {
            if (specialPlatform == SpecialPlatform.Jollysports || specialPlatform == SpecialPlatform.JollysportCV)
            {
                return true;
            }
            return false;
        }
    }
}