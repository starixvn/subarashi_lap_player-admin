using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.ConsoleClient
{
    public interface ILgApiClient
    {
        Task<T> PostAsync<T>(string path, object value, string language) where T : class;
    }
}