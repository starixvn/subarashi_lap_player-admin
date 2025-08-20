namespace Fanex.Athena.PlayerAdmin.Core.Repositories
{
    using Data;

    public interface IDbFactory
    {
        IObjectDb GetObjectDb(string spName, int timeout = 120);
    }
}