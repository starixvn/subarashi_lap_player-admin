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
            // Register the API clients as scoped services using IHttpClientFactory
            services.AddScoped<IReportApiClient, ReportApiClient>();
            services.AddScoped<IBetListApiClient, BetListApiClient>();
            services.AddScoped<ICommonApiClient, CommonApiClient>();
            services.AddScoped<ILgApiClient, LgApiClient>();

            services.AddScoped<IContextService, ContextService>();
            services.AddScoped<ILicenseeService, LicenseeService>();

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
            services.AddScoped<ILogRepository, LogRepository>();

            return services;
        }
    }
}