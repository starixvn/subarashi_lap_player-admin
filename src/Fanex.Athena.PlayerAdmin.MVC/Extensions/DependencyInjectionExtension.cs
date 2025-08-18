namespace Fanex.Athena.PlayerAdmin.MVC.Extensions
{
    using Excel;
    using Microsoft.Extensions.DependencyInjection;
    using Repositories;
    using Repositories.Interfaces;
    using Services;
    using Services.Interfaces;

    public static class DependencyInjectionExtension
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