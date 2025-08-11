namespace Fanex.Athena.PlayerAdmin.Core.Extensions
{
    using ConsoleClient;
    using Fanex.Core.Utils;
    using Microsoft.Extensions.DependencyInjection;
    using RemotingRef;
    using Repositories;
    using Services;

    public static class DefendencyInjectionExtension
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            return services
                .AddServices()
                .AddRepositories();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IReportApiClient, ReportApiClient>();
            services.AddTransient<IBetListApiClient, BetListApiClient>();
            services.AddTransient<ICommonApiClient, CommonApiClient>();

            services.AddScoped<IContextService, ContextService>();

            services.AddSingleton<ILicenseeService, LicenseeService>();
            services.AddSingleton<ISubsidiaryService, SubsidiaryService>();
            services.AddSingleton<IResourceService, ResourceService>();
            services.AddSingleton<IRemotingRefService, RemotingRefService>();
            services.AddSingleton<IMetaModelBindingService, BaseMetaModelBindingService>();
            services.AddSingleton<IFunctionUMService, FunctionUMService>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IDbFactory, DbFactory>();
            services.AddTransient<ILogRepository, LogRepository>();

            return services;
        }
    }
}