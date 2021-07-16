using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Infrastructure.Extensions
{
    public static class GuardExtension
    {
        public static bool IsPresent<T>(this IEnumerable<T> source)
            => source != null && source.Any();
    }
}
