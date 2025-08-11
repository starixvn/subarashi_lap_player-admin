namespace Fanex.Athena.PlayerAdmin.Core.ConsoleClient
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IBetListApiClient
    {
        Task<HttpResponseMessage> PostAsync(string path, object value, string language, int timeOut = 100);

        Task<string> PostStringAsync(string path, object value, string language, int timeOut = 100);
    }
}