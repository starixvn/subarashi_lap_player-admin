using Fanex.Athena.PlayerAdmin.Commission.Repositories;
using Fanex.Athena.PlayerAdmin.Commission.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.Commission.Services;
using Fanex.Athena.PlayerAdmin.Commission.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.PlayerAdmin.Commission.Extensions
{
    public static class DefendencyInjectionExtension
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