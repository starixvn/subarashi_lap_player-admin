namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Settings
{
    using Core.Settings;

    public class LoggingSettingTests
    {
        [Fact]
        public void Properties_AreSetAndRetrieved()
        {
            var setting = new LoggingSetting();
            var category = "TestCategory";
            var sentryEngine = new SentryEngine { BackendDsn = "https://test@sentry.io/12345" };
            var rabbitEngine = new RabbitEngine { ConnectionString = "amqp://localhost:5672" };

            setting.Category = category;
            setting.SentryEngine = sentryEngine;
            setting.RabbitEngine = rabbitEngine;

            Assert.Equal(category, setting.Category);
            Assert.Equal(sentryEngine, setting.SentryEngine);
            Assert.Equal(rabbitEngine, setting.RabbitEngine);
        }

        [Fact]
        public void Properties_InitiallyNull()
        {
            var setting = new LoggingSetting();

            Assert.Null(setting.Category);
            Assert.Null(setting.SentryEngine);
            Assert.Null(setting.RabbitEngine);
        }

        [Fact]
        public void Properties_CanBeSetToNull()
        {
            var setting = new LoggingSetting();
            
            // Set to non-null values first
            setting.Category = "TestCategory";
            setting.SentryEngine = new SentryEngine();
            setting.RabbitEngine = new RabbitEngine();

            // Then set back to null
            setting.Category = null;
            setting.SentryEngine = null;
            setting.RabbitEngine = null;

            Assert.Null(setting.Category);
            Assert.Null(setting.SentryEngine);
            Assert.Null(setting.RabbitEngine);
        }

        [Fact]
        public void Category_HandlesEmptyAndWhitespaceStrings()
        {
            var setting = new LoggingSetting();

            setting.Category = "";
            Assert.Equal("", setting.Category);

            setting.Category = "   ";
            Assert.Equal("   ", setting.Category);

            setting.Category = string.Empty;
            Assert.Equal(string.Empty, setting.Category);
        }

        [Fact]
        public void Category_HandlesLongStrings()
        {
            var setting = new LoggingSetting();
            var longCategory = new string('A', 1000);

            setting.Category = longCategory;

            Assert.Equal(longCategory, setting.Category);
            Assert.Equal(1000, setting.Category.Length);
        }

        [Fact]
        public void SentryEngine_CanBeSetIndependently()
        {
            var setting = new LoggingSetting();
            var sentryEngine = new SentryEngine { BackendDsn = "https://test@sentry.io/12345" };

            setting.SentryEngine = sentryEngine;

            Assert.Equal(sentryEngine, setting.SentryEngine);
            Assert.Equal("https://test@sentry.io/12345", setting.SentryEngine.BackendDsn);
            Assert.Null(setting.RabbitEngine);
            Assert.Null(setting.Category);
        }

        [Fact]
        public void RabbitEngine_CanBeSetIndependently()
        {
            var setting = new LoggingSetting();
            var rabbitEngine = new RabbitEngine { ConnectionString = "amqp://localhost:5672" };

            setting.RabbitEngine = rabbitEngine;

            Assert.Equal(rabbitEngine, setting.RabbitEngine);
            Assert.Equal("amqp://localhost:5672", setting.RabbitEngine.ConnectionString);
            Assert.Null(setting.SentryEngine);
            Assert.Null(setting.Category);
        }
    }

    public class SentryEngineTests
    {
        [Fact]
        public void BackendDsn_AreSetAndRetrieved()
        {
            var sentryEngine = new SentryEngine();
            var dsn = "https://test@sentry.io/12345";

            sentryEngine.BackendDsn = dsn;

            Assert.Equal(dsn, sentryEngine.BackendDsn);
        }

        [Fact]
        public void BackendDsn_InitiallyNull()
        {
            var sentryEngine = new SentryEngine();

            Assert.Null(sentryEngine.BackendDsn);
        }

        [Fact]
        public void BackendDsn_CanBeSetToNull()
        {
            var sentryEngine = new SentryEngine();
            
            // Set to non-null value first
            sentryEngine.BackendDsn = "https://test@sentry.io/12345";

            // Then set back to null
            sentryEngine.BackendDsn = null;

            Assert.Null(sentryEngine.BackendDsn);
        }

        [Fact]
        public void BackendDsn_HandlesEmptyAndWhitespaceStrings()
        {
            var sentryEngine = new SentryEngine();

            sentryEngine.BackendDsn = "";
            Assert.Equal("", sentryEngine.BackendDsn);

            sentryEngine.BackendDsn = "   ";
            Assert.Equal("   ", sentryEngine.BackendDsn);

            sentryEngine.BackendDsn = string.Empty;
            Assert.Equal(string.Empty, sentryEngine.BackendDsn);
        }

        [Fact]
        public void BackendDsn_HandlesLongUrls()
        {
            var sentryEngine = new SentryEngine();
            var longDsn = "https://" + new string('a', 500) + "@sentry.io/12345";

            sentryEngine.BackendDsn = longDsn;

            Assert.Equal(longDsn, sentryEngine.BackendDsn);
            Assert.True(sentryEngine.BackendDsn.Length > 500);
        }

        [Fact]
        public void BackendDsn_HandlesVariousUrlFormats()
        {
            var sentryEngine = new SentryEngine();
            var validUrls = new[]
            {
                "https://test@sentry.io/12345",
                "http://user:pass@localhost:9000/123",
                "https://key@o123456.ingest.sentry.io/123456",
                "https://publickey@dsn.sentry.io/projectid"
            };

            foreach (var url in validUrls)
            {
                sentryEngine.BackendDsn = url;
                Assert.Equal(url, sentryEngine.BackendDsn);
            }
        }
    }

    public class RabbitEngineTests
    {
        [Fact]
        public void ConnectionString_AreSetAndRetrieved()
        {
            var rabbitEngine = new RabbitEngine();
            var connectionString = "amqp://localhost:5672";

            rabbitEngine.ConnectionString = connectionString;

            Assert.Equal(connectionString, rabbitEngine.ConnectionString);
        }

        [Fact]
        public void ConnectionString_InitiallyNull()
        {
            var rabbitEngine = new RabbitEngine();

            Assert.Null(rabbitEngine.ConnectionString);
        }

        [Fact]
        public void ConnectionString_CanBeSetToNull()
        {
            var rabbitEngine = new RabbitEngine();
            
            // Set to non-null value first
            rabbitEngine.ConnectionString = "amqp://localhost:5672";

            // Then set back to null
            rabbitEngine.ConnectionString = null;

            Assert.Null(rabbitEngine.ConnectionString);
        }

        [Fact]
        public void ConnectionString_HandlesEmptyAndWhitespaceStrings()
        {
            var rabbitEngine = new RabbitEngine();

            rabbitEngine.ConnectionString = "";
            Assert.Equal("", rabbitEngine.ConnectionString);

            rabbitEngine.ConnectionString = "   ";
            Assert.Equal("   ", rabbitEngine.ConnectionString);

            rabbitEngine.ConnectionString = string.Empty;
            Assert.Equal(string.Empty, rabbitEngine.ConnectionString);
        }

        [Fact]
        public void ConnectionString_HandlesLongConnectionStrings()
        {
            var rabbitEngine = new RabbitEngine();
            var longConnectionString = "amqp://user:" + new string('p', 500) + "@verylong" + new string('h', 100) + ".com:5672/vhost";

            rabbitEngine.ConnectionString = longConnectionString;

            Assert.Equal(longConnectionString, rabbitEngine.ConnectionString);
            Assert.True(rabbitEngine.ConnectionString.Length > 500);
        }

        [Fact]
        public void ConnectionString_HandlesVariousConnectionStringFormats()
        {
            var rabbitEngine = new RabbitEngine();
            var validConnectionStrings = new[]
            {
                "amqp://localhost",
                "amqp://localhost:5672",
                "amqp://user:password@localhost:5672",
                "amqp://user:password@localhost:5672/vhost",
                "amqps://user:password@rabbitmq.example.com:5671/production",
                "rabbitmq://guest:guest@localhost:5672/%2f"
            };

            foreach (var connectionString in validConnectionStrings)
            {
                rabbitEngine.ConnectionString = connectionString;
                Assert.Equal(connectionString, rabbitEngine.ConnectionString);
            }
        }
    }
}