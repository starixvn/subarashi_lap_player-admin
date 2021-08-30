using Fanex.Athena.PlayerAdmin.Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly IDbFactory _dbFactory;
        private readonly ILogger<LogRepository> _logger;

        public LogRepository(IDbFactory dbFactory, ILogger<LogRepository> logger)
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
                _logger.LogCritical(ex, "WriteLog_UserLog_UserControl \n" + ex.Message);
            }
        }
    }
}