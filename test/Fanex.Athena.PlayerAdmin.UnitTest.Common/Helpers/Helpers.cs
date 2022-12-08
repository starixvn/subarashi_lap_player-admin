using NSubstitute.Core;

namespace Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers
{
    public static class Helpers
    {
        public static bool IsSubstitute(this object obj) => obj is ICallRouterProvider;
    }
}
