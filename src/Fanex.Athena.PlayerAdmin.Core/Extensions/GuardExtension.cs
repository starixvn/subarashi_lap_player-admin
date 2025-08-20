namespace Fanex.Athena.PlayerAdmin.Core.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    public static class GuardExtension
    {
        public static bool IsPresent<T>(this IEnumerable<T> source)
            => source != null && source.Any();
    }
}