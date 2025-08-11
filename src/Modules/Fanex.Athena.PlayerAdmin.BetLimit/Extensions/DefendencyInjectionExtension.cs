namespace Fanex.Athena.PlayerAdmin.BetLimit.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Repositories;
    using Repositories.Interfaces;
    using Services;
    using Services.Interfaces;

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
            services.AddTransient<IThirdPartyBetLimitRepository, ThirdPartyBetLimitRepository>();
            services.AddTransient<INewDcsBetLimitRepository, NewDcsBetLimitRepository>();
            services.AddTransient<IDcsGameBetLimitRepository, DcsGameBetLimitRepository>();
            return services;
        }
    }
}