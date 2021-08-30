using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.HealthCheck;
using Fanex.Athena.PlayerAdmin.Core.Middlewares;
using Fanex.Athena.PlayerAdmin.Core.Settings;
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

namespace Fanex.Athena.PlayerAdmin.Core.Extensions
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
        }

        public static void UseHealthCheck(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.StartsWithSegments("/ping"))
                {
                    return;
                }
                await next();
            });

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
            app.UseWhen(context => !context.Request.IsAjaxRequest() && !context.Request.IsStaticRequest(),
                builder => builder.UseStatusCodePagesWithReExecute("/Home/Error", "?code={0}"));

            //api
            app.UseWhen(context => context.Request.IsAjaxRequest(),
                builder => builder.UseStatusCodePages(async context =>
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

        public static void UseCookieSameSitePolicy(this IApplicationBuilder app)
        {
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Unspecified,
                OnAppendCookie = cookieContext =>
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions),
                OnDeleteCookie = cookieContext =>
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions)
            });
        }

        private static void CheckSameSite(HttpContext httpContext, CookieOptions options)
        {
            if (options.SameSite == SameSiteMode.None)
            {
                var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
                if (DisallowsSameSiteNone(userAgent))
                {
                    options.SameSite = SameSiteMode.Unspecified;
                }
            }
        }

        public static bool DisallowsSameSiteNone(string userAgent)
        {
            // Check if a null or empty string has been passed in, since this
            // will cause further interrogation of the useragent to fail.
            // - Safari on iOS 12 for iPhone, iPod Touch, iPad
            // - WkWebview on iOS 12 for iPhone, iPod Touch, iPad
            // - Chrome on iOS 12 for iPhone, iPod Touch, iPad
            // All of which are broken by SameSite=None,
            if (string.IsNullOrWhiteSpace(userAgent))
            {
                return false;
            }

            // Cover all iOS based browsers here. This includes: because they use the iOS networking
            // stack.
            if (userAgent.Contains("CPU iPhone OS 12") ||
                userAgent.Contains("iPad; CPU OS 12"))
            {
                return true;
            }

            // Cover Mac OS X based browsers that use the Mac OS networking stack.
            // This includes:
            // - Safari on Mac OS X.
            // This does not include:
            // - Chrome on Mac OS X
            // Because they do not use the Mac OS networking stack.
            if (userAgent.Contains("Macintosh; Intel Mac OS X 10_14") &&
                userAgent.Contains("Version/") && userAgent.Contains("Safari"))
            {
                return true;
            }

            // Cover Chrome 50-69, because some versions are broken by SameSite=None,
            // and none in this range require it.
            // Note: this covers some pre-Chromium Edge versions,
            // but pre-Chromium Edge does not require SameSite=None.
            if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
            {
                return true;
            }

            return false;
        }
    }
}