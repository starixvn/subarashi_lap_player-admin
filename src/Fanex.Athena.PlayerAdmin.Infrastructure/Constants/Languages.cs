using System;
using System.Collections.Generic;

namespace Fanex.Athena.PlayerAdmin.Infrastructure.Constants
{
    public static class Languages
    {
        public static readonly IReadOnlyDictionary<string, string> LanguageCodeMappers = new Dictionary<string, string> (StringComparer.InvariantCultureIgnoreCase)
        {
            { "en", "en-US" },
            { "cs", "zh-CN" }
        };
    }
}