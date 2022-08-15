using System.Net.Http;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.ConsoleClient
{
    public interface ICTSApiClient
    {
        Task<HttpResponseMessage> PostAsync(string path, object value, string language, int timeOut = 100);
    }
}
