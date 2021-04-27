using Fanex.Athena.PlayerAdmin.API.Services;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Core.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.PlayerAdmin.API.Extensions
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
            services.AddTransient<IPlayerActivityRecordService, PlayerActivityRecordService>();
            services.AddTransient<IPlayerBaStatusService, PlayerBaStatusService>();
            services.AddTransient<IPlayerBettingStatisticsService, PlayerBettingStatisticsService>();
            services.AddTransient<IPlayerListService, PlayerListService>();
            services.AddTransient<IPlayerOddsGroupService, PlayerOddsGroupService>();
            services.AddTransient<IPlayerSpecialLevelService, PlayerSpecialLevelService>();

            services.AddSingleton<IMetaModelBindingService, BaseMetaModelBindingService>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services;
        }
    }
}