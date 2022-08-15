using Fanex.Athena.PlayerAdmin.Core.ConsoleClient;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Core.Utils;
using Fanex.RemotingRef;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.PlayerAdmin.Core.Extensions
{
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