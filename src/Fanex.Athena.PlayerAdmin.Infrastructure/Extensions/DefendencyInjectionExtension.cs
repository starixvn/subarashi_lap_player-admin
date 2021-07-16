using Fanex.Athena.PlayerAdmin.Infrastructure.ConsoleClient;
using Fanex.Athena.PlayerAdmin.Infrastructure.Repositories;
using Fanex.Athena.PlayerAdmin.Infrastructure.Services;
using Fanex.Core.Utils;
using Fanex.RemotingRef;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Fanex.Athena.PlayerAdmin.Infrastructure.Extensions
{
    public static class DefendencyInjectionExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
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
            services.AddSingleton<IResourceService, ResourceService>();
            services.AddSingleton<IRemotingRefService, RemotingRefService>();

            services.AddSingleton<IMetaModelBindingService, BaseMetaModelBindingService>();

            services.AddHttpContextAccessor();
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