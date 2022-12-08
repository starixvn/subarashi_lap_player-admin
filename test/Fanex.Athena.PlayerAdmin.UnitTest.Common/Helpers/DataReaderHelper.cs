using NSubstitute;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers
{
    public static class DataReaderHelper
    {
        public static void FakeNumberOfRead<T>(this IDataReader reader, IEnumerable<T> values)
        {
            if (values == null || !values.Any())
            {
                reader.Read().Returns(false);
                return;
            }
            var remainValues = values.Skip(1);
            var returnResults = new List<bool>(remainValues.Count() + 1);
            foreach (var _ in remainValues)
            {
                returnResults.Add(true);
            }
            returnResults.Add(false);
            reader.Read().Returns(true, returnResults.ToArray());
        }
    }
}
