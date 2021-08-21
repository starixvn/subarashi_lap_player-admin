using System.Net.Http;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.ConsoleClient
{
    public interface IBetListApiClient
    {
        Task<HttpResponseMessage> PostAsync(string path, object value, string language, int timeOut = 100);

        Task<T> PostAsync<T>(string path, object value, string language) where T : class;

        Task<string> PostString(string path, object value, string language, int timeOut = 100);
    }
}