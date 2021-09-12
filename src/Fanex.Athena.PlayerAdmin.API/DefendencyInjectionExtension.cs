using Fanex.Athena.PlayerAdmin.API.Services;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.PlayerAdmin.API
{
    public static class DefendencyInjectionExtension
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
            services.AddTransient<IPlayerBaStatusService, PlayerBaStatusService>();
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