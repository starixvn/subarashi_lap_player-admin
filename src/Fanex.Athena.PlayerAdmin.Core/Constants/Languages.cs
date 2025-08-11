namespace Fanex.Athena.PlayerAdmin.Core.Constants
{
    using System;
    using System.Collections.Generic;

    public static class Languages
    {
        public static readonly IReadOnlyDictionary<string, string> LanguageCodeMappers = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "en", "en-US" },
            { "cs", "zh-CN" },
            { "vn", "vi-VN" },
            { "th", "th-TH" },
            { "id", "id-ID" },
        };
    }
}