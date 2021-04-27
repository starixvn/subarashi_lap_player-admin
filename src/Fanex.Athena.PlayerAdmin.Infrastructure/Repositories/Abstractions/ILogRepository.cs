using Fanex.Athena.PlayerAdmin.Infrastructure.Models;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Infrastructure.Repositories
{
    public interface ILogRepository
    {
        Task Write(LogData logData);
    }
}