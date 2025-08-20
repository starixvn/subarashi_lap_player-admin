namespace Fanex.Athena.PlayerAdmin.Core.Repositories
{
    using System.Threading.Tasks;
    using Models;

    public interface ILogRepository
    {
        Task Write(LogData logData);
    }
}