namespace Fanex.Athena.PlayerAdmin.Core.Helpers
{
    using System;
    using System.Collections.Generic;

    public static class ValidationHelper
    {
        public static bool TryParseArray<T>(string input, out T[] result) where T : IParsable<T>
        {
            result = [];
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            var parts = input.Split(',');
            var list = new List<T>();

            foreach (var part in parts)
            {
                if (!T.TryParse(part.Trim(), null, out var value))
                    return false;

                list.Add(value);
            }

            result = [.. list];
            return true;
        }
    }
}