namespace Fanex.Athena.PlayerAdmin.Core.ConsoleClient
{
    using System.Threading.Tasks;

    public interface IReportApiClient
    {
        Task<string> PostStringAsync(string path, object value, string language, int timeOut = 100);
    }
}