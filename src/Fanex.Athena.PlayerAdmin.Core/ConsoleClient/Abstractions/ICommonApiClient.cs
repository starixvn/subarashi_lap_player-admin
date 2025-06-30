using System.Net.Http;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.ConsoleClient
{
    public interface ICommonApiClient
    {
        Task<T> GetAsync<T>(string path, object value = null, int timeOut = 100);
    }
}