using Fanex.Athena.PlayerAdmin.API.Extensions;
using Fanex.Athena.PlayerAdmin.Auth.Controllers;
using Fanex.Athena.PlayerAdmin.BatchUpdate;
using Fanex.Athena.PlayerAdmin.BetLimit.Extensions;
using Fanex.Athena.PlayerAdmin.Commission.Extensions;
using Fanex.Athena.PlayerAdmin.Infrastructure.ConsoleClient;
using Fanex.Athena.PlayerAdmin.Infrastructure.Constants;
using Fanex.Athena.PlayerAdmin.Infrastructure.Extensions;
using Fanex.Athena.PlayerAdmin.Infrastructure.JsonConverter;
using Fanex.Athena.PlayerAdmin.Infrastructure.Policies;
using Fanex.Athena.PlayerAdmin.Infrastructure.Settings;
using Fanex.Athena.PlayerAdmin.Infrastructure.Storages;
using Fanex.Athena.PlayerAdmin.MVC.Extensions;
using Fanex.Athena.PlayerAdmin.MVC.Services;
using Fanex.Athena.PlayerAdmin.MVC.Services.Interfaces;
using Fanex.Data;
using Fanex.Data.SqlServer;
using Fanex.RemotingRef;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.WebHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                              .RequireAuthenticatedUser()
                              .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddJsonOptions(jsonOptions =>
            {
                jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
                jsonOptions.JsonSerializerOptions.Converters.Add(new StringToDateTimeConverter());
                jsonOptions.JsonSerializerOptions.Converters.Add(new EmptyStringToNullConverter());
            });

            services.AddAuthorization(options =>
            {
                options.NotAllowSubsidiaryPolicy();
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("zh-CN")
                };

                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(context =>
                {
                    var lang = Languages.LanguageCodeMappers[context.User.FindFirst("Language")?.Value ?? "en"];
                    return Task.FromResult(new ProviderCultureResult(lang));
                }));
            });

            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(HostingEnvironment.ContentRootPath));
            services.AddSingleton<ITicketStore, RedisCacheTicketStore>();

            var identityServer = Configuration.GetSection("IdentityServer").Get<IdentityServerSetting>();
            var cookieSetting = new CookieSetting();
            Configuration.GetSection("Cookie").Bind(cookieSetting);

            services.AddAuthentication()
                .AddCookie(options =>
                {
                    options.Cookie = new CookieBuilder
                    {
                        HttpOnly = true,
                        SameSite = cookieSetting.SameSite,
                        SecurePolicy = cookieSetting.Secure,
                        IsEssential = true,
                        Name = cookieSetting.Name
                    };

                    options.ExpireTimeSpan = TimeSpan.FromMinutes(cookieSetting.ExpireTime);
                    options.SlidingExpiration = cookieSetting.SlidingExpiration;
                    options.LoginPath = cookieSetting.LoginPath;
                    options.AccessDeniedPath = cookieSetting.DeniedPath;
                    options.SessionStore = services.BuildServiceProvider().GetRequiredService<ITicketStore>();
                })
                .AddOAuth2Introspection(options =>
                {
                    options.Authority = identityServer.Host;
                    options.ClientId = identityServer.CredentialIntrospection.ClientId;
                    options.ClientSecret = identityServer.CredentialIntrospection.ClientSecret;
                    options.SaveToken = true;
                    options.SkipTokensWithDots = true;
                    options.EnableCaching = identityServer.CacheDuration > 0;
                    options.CacheKeyPrefix = "OAuth2Introspection:";
                    options.CacheDuration = TimeSpan.FromMinutes(identityServer.CacheDuration);
                    options.DiscoveryPolicy.RequireHttps = false;
                });

            services.AddAntiforgery(options =>
            {
                options.FormFieldName = "__RequestVerificationToken";
                options.HeaderName = "X-CSRF-TOKEN-PLAYERADMIN";
                options.Cookie.SameSite = cookieSetting.SameSite;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.Name = "lap.playeradmin.antiforgery";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.SuppressXFrameOptionsHeader = true;
            });

            var redisSetting = Configuration.GetSection("Redis").Get<RedisSetting>();
            ConfigDistributedCache(services, redisSetting);
            ConfigDataProtection(services, redisSetting);
            ConfigFanexData();

            RefDataTypeContainer
                .ConnectTo(new Uri(Configuration["RemotingRefUrl"]))
                .FromApplication("LAP.PlayerAdmin");

            SetupMediatR(services);

            services.AddInfrastructure();
            services.AddAPI();
            services.AddMVC();
            services.AddBetLimit();
            services.AddCommission();
            services.AddBatchUpdate();

            var appSettingSection = Configuration.GetSection("App");
            services.AddCorsPortalSite(appSettingSection.Get<AppSetting>());

            services.Configure<AppSetting>(appSettingSection);
            services.Configure<IdentityServerSetting>(Configuration.GetSection("IdentityServer"));
            services.Configure<BetLimitApiSetting>(Configuration.GetSection("BetLimitApi"));
            services.Configure<FunctionAllowedProductsSetting>(Configuration.GetSection("FunctionAllowedProducts"));

            services.AddHttpClient<AuthController>()
                .SetHandlerLifetime(TimeSpan.FromDays(1));

            var statement = Configuration.GetSection("Statement");
            services.Configure<StatementSetting>(statement);

            services.AddHttpClient<IPlayerStatementService, PlayerStatementService>(client =>
            {
                client.BaseAddress = new Uri(statement.Get<StatementSetting>().InternalHost);
            }).SetHandlerLifetime(TimeSpan.FromDays(1));

            services.AddHttpClient<IReportApiClient, ReportApiClient>(client =>
            {
                client.BaseAddress = new Uri(Configuration["ConsoleClient:Report:Host"]);
                client.DefaultRequestHeaders.Add("AUTH_TOKEN", Configuration["ConsoleClient:Report:AuthKey"]);
            }).SetHandlerLifetime(TimeSpan.FromDays(1));

            services.AddHttpClient<IBetListApiClient, BetListApiClient>(client =>
            {
                client.BaseAddress = new Uri(Configuration["ConsoleClient:BetList:Host"]);
                client.DefaultRequestHeaders.Add("BETLIST_TOKEN_KEY", Configuration["ConsoleClient:BetList:AuthKey"]);
            }).SetHandlerLifetime(TimeSpan.FromDays(1));

            services.AddHttpClient<ILgApiClient, LgApiClient>(client =>
            {
                client.BaseAddress = new Uri(Configuration["ConsoleClient:Lg:Host"]);
                client.DefaultRequestHeaders.Add("LGAPI_TOKEN_KEY", Configuration["ConsoleClient:Lg:AuthKey"]);
            }).SetHandlerLifetime(TimeSpan.FromDays(1));

            services.AddHttpClient();

            services.AddHealthCheck(Configuration);
            services.AddUnderMaintenance(Configuration);

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders();

            if (string.Equals(Configuration["UseErrorHandler"], "1"))
            {
                app.UseErrorHandler();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            if (!HostingEnvironment.IsDevelopment())
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseStaticResource(HostingEnvironment.IsDevelopment());
            app.UseHealthCheck(Configuration);
            app.UseRouting();
            app.UseCors();
            app.UseUnderMaintenance();
            app.UseAuthorization();
            app.UseRequestLocalization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        private static void ConfigDistributedCache(IServiceCollection services, RedisSetting redisSetting)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = new ConfigurationOptions
                {
                    KeepAlive = 180,
                    Password = redisSetting.Pass,
                    DefaultDatabase = redisSetting.Database,
                    EndPoints =
                    {
                        { redisSetting.Server, redisSetting.Port }
                    },
                };
                options.InstanceName = redisSetting.AppName;
            });
        }

        private static void ConfigDataProtection(IServiceCollection services, RedisSetting redisSetting)
        {
            var configRedix = new ConfigurationOptions
            {
                EndPoints =
                {
                    { redisSetting.Server, redisSetting.Port }
                },
                KeepAlive = redisSetting.KeepAlive,
                Password = redisSetting.Pass
            };
            var redis = ConnectionMultiplexer.Connect(configRedix);
            services.AddDataProtection()
                .PersistKeysToStackExchangeRedis(redis, redisSetting.ProtectionDataKey)
                .SetDefaultKeyLifetime(TimeSpan.FromDays(60))
                .SetApplicationName(redisSetting.AppName);
        }

        private static void SetupMediatR(IServiceCollection services)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("Fanex.Athena.PlayerAdmin"));
            services.AddMediatR(assemblies.ToArray());
        }

        private void ConfigFanexData()
        {
            var listConnectionName = Configuration.GetSection("ConnectionStrings").GetChildren().Select(_ => _.Key);
            var connectionStrings = new Dictionary<string, ConnectionConfiguration>(listConnectionName
                .Select(connectionName => new KeyValuePair<string, ConnectionConfiguration>(connectionName,
                    new ConnectionConfiguration(connectionName, Configuration.GetConnectionString(connectionName)))
                ));

            DbSettingProviderManager
                .StartNewSession()
                .WithSqlServer(resourcePath: "~/App_Data/SpConfig/FanexSpName.xml")
                .Use(connectionStrings)
                .Run();
        }
    }
}