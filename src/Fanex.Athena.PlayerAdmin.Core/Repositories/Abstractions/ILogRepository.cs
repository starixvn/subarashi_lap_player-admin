using Fanex.Athena.PlayerAdmin.Core.Models;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.Repositories
{
    public interface ILogRepository
    {
        Task Write(LogData logData);
    }
}