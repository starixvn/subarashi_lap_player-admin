using Fanex.Athena.PlayerAdmin.Infrastructure.Constants;
using Fanex.Athena.PlayerAdmin.Infrastructure.HealthCheck;
using Fanex.Athena.PlayerAdmin.Infrastructure.Middlewares;
using Fanex.Athena.PlayerAdmin.Infrastructure.Settings;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using ONEbook.UM.AspNetCore;
using ONEbook.UM.Configurations;
using ONEbook.UM.Helpers;
using ONEbook.UM.Services;
using ONEbook.UM.Transport;
using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Infrastructure.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void AddCorsPortalSite(this IServiceCollection services, AppSetting appSetting)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(Policy.FromPortalSite,
                                  builder =>
                                  {
                                      builder.SetIsOriginAllowed(domain =>
                                      {
                                          if (!appSetting.AllowedCrossDomains.IsPresent())
                                          {
                                              return true;
                                          }

                                          var clientDomain = new Uri(domain).Authority;
                                          var isValid = appSetting.AllowedCrossDomains.Any(allowedCrossDomain => clientDomain.EndsWith(allowedCrossDomain, StringComparison.OrdinalIgnoreCase));
                                          return isValid;
                                      })
                                      .AllowAnyMethod()
                                      .AllowAnyHeader();
                                  });
            });
        }

        public static void AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
               .AddCheck<WhoAmIHealthCheck>("WhoAmI")
               .AddCheck<MemoryHealthCheck>("Memory")
               .AddUrlGroup(new Uri($"{configuration["IdentityServer:Host"]}/{configuration["IdentityServer:HealthPath"]}"), "Identity Server")
               .AddUrlGroup(new Uri($"{configuration["Statement:InternalHost"]}{configuration["Statement:HealthPath"]}"), "Statement")
               .AddUrlGroup(new Uri($"{configuration["ConsoleClient:Report:Host"]}{configuration["ConsoleClient:Report:HealthPath"]}"), "Report API")
               .AddUrlGroup(new Uri($"{configuration["ConsoleClient:BetList:Host"]}{configuration["ConsoleClient:BetList:HealthPath"]}"), "BetList API")
               .AddUrlGroup(new Uri($"{configuration["ConsoleClient:Lg:Host"]}{configuration["ConsoleClient:Lg:HealthPath"]}"), "LG API")
               .AddRedis($"{configuration["Redis:Server"]}:{configuration["Redis:Port"]},password={configuration["Redis:Pass"]}", "Redis");

            if (bool.Parse(configuration["HealthCheck:EnableUI"]))
            {
                services.AddHealthChecksUI(opt =>
                {
                    opt.SetEvaluationTimeInSeconds(15); //time in seconds between check
                    opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks
                    opt.SetApiMaxActiveRequests(1); //api requests concurrency
                    opt.AddHealthCheckEndpoint("Self", "/health-api");
                }).AddInMemoryStorage();
            }
        }

        public static void UseHealthCheck(this IApplicationBuilder app, IConfiguration configuration)
        {
            Func<HttpContext, HealthReport, Task> customResponseWriter = async (context, report) =>
            {
                var result = JsonConvert.SerializeObject(
                    new
                    {
                        status = report.Status.ToString(),
                        monitors = report.Entries.Select(e => new { name = e.Key, status = Enum.GetName(typeof(HealthStatus), e.Value.Status), values = e.Value.Data })
                    }, Formatting.Indented);

                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsync(result);
            };

            var accessKey = configuration["HealthCheck:AccessKey"];
            app.UseWhen(context =>
                string.IsNullOrEmpty(accessKey) || (context.Request.Query.TryGetValue("AccessKey", out StringValues value) && accessKey.Equals(value)),
                     builder => builder.UseHealthChecks("/WhoAmI", options: new HealthCheckOptions
                     {
                         AllowCachingResponses = false,
                         Predicate = _ => _.Name.Equals("WhoAmI"),
                         ResponseWriter = customResponseWriter
                     }).UseHealthChecks("/health", options: new HealthCheckOptions
                     {
                         AllowCachingResponses = false,
                         Predicate = _ => !_.Name.Equals("WhoAmI"),
                         ResponseWriter = customResponseWriter
                     }));

            if (bool.Parse(configuration["HealthCheck:EnableUI"]))
            {
                app.UseHealthChecks("/health-api", new HealthCheckOptions
                {
                    Predicate = _ => !_.Name.Equals("WhoAmI"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                app.UseHealthChecksUI(option =>
                {
                    option.UIPath = "/health-ui";
                });
            }
        }

        public static void AddUnderMaintenance(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(new UMClientConfiguration().Initialize(configuration));
            services.AddSingleton<IUMCacheService, UMCacheService>();
            services.AddSingleton<IUMRestClient, UMRestClient>();
            services.AddSingleton<IHttpContextHelper, HttpContextHelper>();
            services.AddSingleton<IMaintenanceService, MaintenanceService>();
            services.AddSession();
        }

        public static void UseUnderMaintenance(this IApplicationBuilder app)
        {
            app.UseSession();
            app.UseMiddleware<UnderMaintenanceHandler>();
        }

        public static void UseErrorHandler(this IApplicationBuilder app)
        {
            //mvc
            app.UseWhen(context => !context.Request.IsAjaxRequest() && !context.Request.IsStaticRequest(), builder => builder.UseStatusCodePagesWithReExecute("/Home/Error", "?code={0}"));

            //api
            app.UseWhen(context => context.Request.IsAjaxRequest(), builder => builder.UseStatusCodePages(async context =>
            {
                var response = context.HttpContext.Response;
                response.ContentType = "application/json";
                var errorObject = new
                {
                    statusCode = response.StatusCode,
                    msg = "An error occurred while processing your request",
                    isSuccess = false
                };
                await response.WriteAsync(JsonConvert.SerializeObject(errorObject));
            }));

            //logging
            app.UseMiddleware<ErrorHandler>();
        }

        public static void UseStaticResource(this IApplicationBuilder app, bool isDev)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int OneYear = 60 * 60 * 24 * 365;
                    ctx.Context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.CacheControl] = isDev ?
                    "no-cache" : "public,max-age=" + OneYear;
                    ctx.Context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.AccessControlAllowOrigin] = "*";
                }
            });
        }
    }
}