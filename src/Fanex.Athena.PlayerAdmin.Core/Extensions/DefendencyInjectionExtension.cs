using Fanex.Athena.PlayerAdmin.Core.ConsoleClient;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Core.Utils;
using Fanex.RemotingRef;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Fanex.Athena.PlayerAdmin.Core.Extensions
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
            services.AddSingleton<ICustomerService, CustomerService>();
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