namespace Fanex.Athena.PlayerAdmin.Commission.MetaModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class CommValidator
    {
        private const decimal MinValue = 0;
        private const decimal MaxValue = 0.15m;
        private const decimal RangValueKey = 0.0005m;
        private static IEnumerable<decimal> _commissions;

        private static void BuildCommValues()
        {
            var commissions = new List<decimal>();
            const int steps = (int)((MaxValue - MinValue) / RangValueKey) + 1;
            
            for (int i = 0; i < steps; i++)
            {
                var value = MinValue + (i * RangValueKey);
                commissions.Add(value);
            }

            _commissions = commissions;
        }

        public static bool IsValid(params decimal[] checkValues)
        {
            if (_commissions == null)
            {
                BuildCommValues();
            }

            return Array.TrueForAll(checkValues,val => _commissions.Contains(val));
        }
    }
}
