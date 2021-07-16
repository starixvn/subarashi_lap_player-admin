using Fanex.Athena.PlayerAdmin.BatchUpdate.Repositories;
using Fanex.Athena.PlayerAdmin.BatchUpdate.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.PlayerAdmin.BatchUpdate
{
    public static class DefendencyInjectionExtension
    {
        public static IServiceCollection AddBatchUpdate(this IServiceCollection services)
        {
            return services
                .AddServices()
                .AddRepositories();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IPlayerBatchUpdateService, PlayerBatchUpdateService>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IPlayerBatchUpdateRepository, PlayerBatchUpdateRepository>();
            return services;
        }
    }
}