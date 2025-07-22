using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.PlayerAdmin.Commission.MetaModels
{
    public static class CommValidator
    {
        private const decimal MinValue = 0;
        private const decimal MaxValue = 0.15m;
        private const decimal RangValueKey = 0.0005m;
        private static IEnumerable<decimal> _commissions;

        private static void BuildCommValues()
        {
            var commissions = new List<decimal>();
            for (var value = MinValue; value <= MaxValue; value += RangValueKey)
            {
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

            return !checkValues.Any(val => !_commissions.Any(x => x == val));
        }
    }
}
