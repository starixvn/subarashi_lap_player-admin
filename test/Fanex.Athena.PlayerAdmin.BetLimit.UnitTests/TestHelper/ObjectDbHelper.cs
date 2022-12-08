

using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Data;
using NSubstitute;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.TestHelper
{
    public static class ObjectDbHelper
    {
        public static void AssignResultToParameter(this IObjectDb objectDb, ResultInfo expectedResult)
        {
            objectDb.GetParameterValue("ErrCode").Returns(expectedResult.Code);
            objectDb.GetParameterValue("ErrMsg").Returns(expectedResult.Message);
        }
    }
}
