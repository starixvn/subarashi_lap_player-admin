namespace Fanex.Athena.PlayerAdmin.Commission.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Repositories;
    using Repositories.Interfaces;
    using Services;
    using Services.Interfaces;

    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddCommission(this IServiceCollection services)
        {
            return services
                .AddServices()
                .AddRepositories();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IPlayerCommissionService, PlayerCommissionService>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IPlayerCommissionRepository, PlayerCommissionRepository>();

            return services;
        }
    }
}