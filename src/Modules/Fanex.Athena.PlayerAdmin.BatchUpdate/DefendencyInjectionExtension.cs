namespace Fanex.Athena.PlayerAdmin.BatchUpdate
{
    using Microsoft.Extensions.DependencyInjection;
    using Repositories;
    using Services;

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