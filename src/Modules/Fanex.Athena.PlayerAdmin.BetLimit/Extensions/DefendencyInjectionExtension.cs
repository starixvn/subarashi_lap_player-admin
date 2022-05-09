using Fanex.Athena.PlayerAdmin.BetLimit.Repositories;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.BetLimit.Services;
using Fanex.Athena.PlayerAdmin.BetLimit.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Extensions
{
    public static class DefendencyInjectionExtension
    {
        public static IServiceCollection AddBetLimit(this IServiceCollection services)
        {
            return services
                .AddServices()
                .AddRepositories();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IAGCasinoBetLimitService, AGCasinoBetLimitService>();
            services.AddTransient<ISportsBookBetLimitService, SportsBookBetLimitService>();
            services.AddTransient<IDcsBetLimitService, DcsBetLimitService>();
            services.AddTransient<IRngKenoBetLimitService, RngKenoBetLimitService>();
            services.AddTransient<IThirdPartyBetLimitService, ThirdPartyBetLimitService>();
            services.AddTransient<IExchangeChargeCommService, ExchangeChargeCommService>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IPlayerBetLimitRepository, PlayerBetLimitRepository>();
            services.AddTransient<IAGCasinoBetLimitRepository, AGCasinoBetLimitRepository>();
            services.AddTransient<ISportsBookBetLimitRepository, SportsBookBetLimitRepository>();
            services.AddTransient<IDcsBetLimitRepository, DcsBetLimitRepository>();
            services.AddTransient<IRngKenoBetLimitRepository, RngKenoBetLimitRepository>();
            services.AddTransient<IThirdPartyBetLimitRepository, ThirdPartyBetLimitRepository>();
            services.AddTransient<INumberGameBetLimitService, NumberGameBetLimitService>();
            services.AddTransient<INewDcsBetLimitRepository, NewDcsBetLimitRepository>();
            services.AddTransient<INumberGameBetLimitRepository, NumberGameBetLimitRepository>();
            return services;
        }
    }
}