using Fanex.Athena.PlayerAdmin.MVC.Excel;
using Fanex.Athena.PlayerAdmin.MVC.Repositories;
using Fanex.Athena.PlayerAdmin.MVC.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.MVC.Services;
using Fanex.Athena.PlayerAdmin.MVC.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.PlayerAdmin.MVC.Extensions
{
    public static class DefendencyInjectionExtension
    {
        public static IServiceCollection AddMVC(this IServiceCollection services)
        {
            return services
                .AddServices()
                .AddRepositories();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IPlayerDetailTransactionService, PlayerDetailTransactionService>();
            services.AddTransient<IPlayerDetailTransactionExcel, PlayerDetailTransactionExcel>();
            services.AddTransient<IPlayerStatementService, PlayerStatementService>();
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IPlayerDetailTransactionRepository, PlayerDetailTransactionRepository>();
            return services;
        }
    }
}