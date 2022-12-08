
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.MockClass;
using Fanex.Data;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Repositories
{
    public class LogRepositoryTests
    {
        private IDbFactory _dbFactory;
        private MockLogger<LogRepository> _logger;

        [SetUp]
        public void SetUp()
        {
            _dbFactory = Substitute.For<IDbFactory>();
            _logger = Substitute.For<MockLogger<LogRepository>>();
        }

        [Test]
        public async Task Write_WhenNotHaveError_CallRelevantMethod()
        {
            //Arrange
            const string UspName = "WriteLog_UserLog_UserControl";
            var objectDb = Substitute.For<IObjectDb>();
            _dbFactory.GetObjectDb(string.Empty).ReturnsForAnyArgs(objectDb);
            var logData = FakeHelper.Create<LogData>();
            var repo = CreateRepo();

            //Act
            await repo.Write(logData);

            //Assert
            Assert.Multiple(() =>
            {
                _dbFactory.Received(1).GetObjectDb(UspName);
                objectDb.Received(1).ExecuteNonQueryAsync(logData);
            });
        }

        [Test]
        public async Task Write_WhenHaveError_LoggerLogCritical()
        {
            //Arrange
            const string UspName = "WriteLog_UserLog_UserControl";
            _dbFactory.When(_ => _.GetObjectDb(UspName))
                .Throw(new System.Exception("Test"));
            var logData = FakeHelper.Create<LogData>();

            var repo = CreateRepo();

            //Act
            await repo.Write(logData);

            //Assert
            _logger.ReceivedWithAnyArgs(1).Log(LogLevel.Critical, Arg.Is<string>("Test"));
        }

        private LogRepository CreateRepo()
            => new LogRepository(_dbFactory, _logger);
    }
}
