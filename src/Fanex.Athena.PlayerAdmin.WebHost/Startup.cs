using Fanex.Athena.PlayerAdmin.Auth.Controllers;
using Fanex.Athena.PlayerAdmin.Infrastructure.Configs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Fanex.Athena.PlayerAdmin.WebHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                              .RequireAuthenticatedUser()
                              .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddRazorRuntimeCompilation();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie = new CookieBuilder
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.None,
                        SecurePolicy = CookieSecurePolicy.Always,
                        Name = "lap.playeradmin"
                    };
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.SlidingExpiration = true;
                    options.LoginPath = "/Auth/RedirectToLAP";
                })
                .AddOAuth2Introspection(options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.ClientId = "playeradmin_api";
                    options.ClientSecret = "playeradmin_api.secret";
                    options.SaveToken = true;
                });

            services.Configure<IdentityServerSetting>(Configuration.GetSection("IdentityServer"));
            services.AddHttpClient<AuthController>()
                .SetHandlerLifetime(new TimeSpan(TimeSpan.TicksPerDay));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var isDev = env.IsDevelopment();
            if (isDev)
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int OneYear = 60 * 60 * 24 * 365;
                    ctx.Context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.CacheControl] = isDev ?
                    "no-cache" : "public,max-age=" + OneYear;
                }
            });

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}