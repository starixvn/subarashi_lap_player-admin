namespace Fanex.Athena.PlayerAdmin.API
{
    using Microsoft.Extensions.DependencyInjection;
    using Services;
    using Services.Interfaces;

    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddAPI(this IServiceCollection services)
        {
            return services
                .AddServices()
                .AddRepositories();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IDepositSettingService, DepositSettingService>();

            services.AddTransient<IPlayerActivityRecordService, PlayerActivityRecordService>();
            services.AddTransient<IPlayerBettingStatisticsService, PlayerBettingStatisticsService>();
            services.AddTransient<IPlayerListService, PlayerListService>();
            services.AddTransient<IPlayerOddsGroupService, PlayerOddsGroupService>();
            services.AddTransient<IPlayerSpecialLevelService, PlayerSpecialLevelService>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services;
        }
    }
}