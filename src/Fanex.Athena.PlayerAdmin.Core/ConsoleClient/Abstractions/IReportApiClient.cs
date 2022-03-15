using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.ConsoleClient
{
    public interface IReportApiClient
    {
        Task<string> PostStringAsync(string path, object value, string language, int timeOut = 100);
    }
}