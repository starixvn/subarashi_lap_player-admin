namespace Fanex.Athena.PlayerAdmin.Core.ConsoleClient
{
    using System.Threading.Tasks;

    public interface ILgApiClient
    {
        Task<T> PostAsync<T>(string path, object value, string language) where T : class;
    }
}