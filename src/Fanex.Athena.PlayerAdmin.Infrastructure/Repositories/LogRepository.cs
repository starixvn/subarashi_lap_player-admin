using Fanex.Athena.PlayerAdmin.Infrastructure.Models;
using Fanex.Logging;
using System;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Infrastructure.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly IDbFactory _dbFactory;
        private readonly ILogger _logger;

        public LogRepository(IDbFactory dbFactory, ILogger logger)
        {
            _dbFactory = dbFactory;
            _logger = logger;
        }

        public async Task Write(LogData logData)
        {
            const string UspName = "WriteLog_UserLog_UserControl";

            try
            {
                using (var db = _dbFactory.GetObjectDb(UspName))
                {
                    await db.ExecuteNonQueryAsync(logData);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("WriteLog_UserLog_UserControl \n" + ex.Message, ex);
            }
        }
    }
}