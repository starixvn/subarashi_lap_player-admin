using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Fanex.Athena.PlayerAdmin.WebHost.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.WebHost.Controllers
{
    public class AuthController : Controller
    {
        private readonly IdentityServerSetting _setting;
        private readonly HttpClient _client;

        public AuthController(IOptionsSnapshot<IdentityServerSetting> setting, HttpClient client)
        {
            _setting = setting.Value;
            _client = client;
        }

        [HttpPost]
        [Route("api/auth/token")]
        [ReferrerValidation]
        [EnableCors(Policy.FromPortalSite)]
        [AllowAnonymous]
        [ModelStateValidation]
        public async Task<JsonResult> Token([FromBody] LapTokenRequest request)
        {
            var tokenResponse = await RequestToken(request.Token, _setting.ExternalApiScope);
            return Json(new
            {
                IsSuccess = !tokenResponse.IsError,
                Msg = tokenResponse.ErrorDescription,
                AccessToken = tokenResponse.AccessToken
            });
        }

        [HttpGet]
        [Route("auth/cookie")]
        [ReferrerValidation]
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult> Cookie([FromQuery] string token)
        {
            var currentAccessToken = await HttpContext.GetTokenAsync(CookieAuthenticationDefaults.AuthenticationScheme, "access_token");
            var tokenResponse = await RequestToken(token, _setting.InternalApiScope, currentAccessToken);
            if (tokenResponse.IsError)
            {
                return Redirect("/Home/AccessDenied");
            }

            var userInfo = await GetUserInfo(tokenResponse.AccessToken);
            if (userInfo.IsError)
            {
                return Redirect("/Home/AccessDenied");
            }

            var claims = new List<Claim>(userInfo.Claims);
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");
            var actionLinkClaim = userInfo.Claims.FirstOrDefault(_ => _.Type.Equals("ActionLink", StringComparison.OrdinalIgnoreCase));

            var properties = new AuthenticationProperties();
            properties.StoreTokens(new[] {
                new AuthenticationToken()
                {
                  Name = "access_token",
                  Value = tokenResponse.AccessToken
                }});

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

            if (!string.IsNullOrEmpty(actionLinkClaim?.Value))
            {
                return Redirect(Url.Content("~/" + actionLinkClaim?.Value));
            }

            return Redirect("/");
        }

        private async Task<TokenResponse> RequestToken(string token, string scope, string currentAccessToken = null)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return ProtocolResponse.FromException<TokenResponse>(new ArgumentException($"token is not present"));
            }

            using var request = new TokenRequest
            {
                Address = $"{_setting.Host}/{_setting.TokenEndpoint}",
                ClientId = _setting.CredentialClient.ClientId,
                ClientSecret = _setting.CredentialClient.ClientSecret,
                GrantType = _setting.GrantType,
                Parameters = new Parameters(new Dictionary<string, string>
                {
                    { nameof(token), token },
                    { nameof(scope), scope },
                    { "access_token", currentAccessToken },
                    { "clien_ip", HttpContext.Connection.RemoteIpAddress.ToString() }
                })
            };

            var tokenResponse = await _client.RequestTokenAsync(request);
         
            return tokenResponse.Exception != null ? throw tokenResponse.Exception : tokenResponse;
        }

        private async Task<UserInfoResponse> GetUserInfo(string accesToken)
        {
            using var request = new UserInfoRequest
            {
                Address = $"{_setting.Host}/{_setting.UserInfoEndpoint}",
                Token = accesToken
            };

            var userInfo = await _client.GetUserInfoAsync(request);
            return userInfo;
        } 

        [HttpGet]
        [Route("auth/RedirectToLAP/{lapUrl}/{lapVer?}")]
        [AllowAnonymous]
        public ActionResult RedirectToLAP([FromRoute] string lapUrl, [FromRoute] string lapVer = "")
        {
            return Redirect(string.Format("http://{0}/{1}/12bet/Auth/Login?returnUrl=/PlayerAdminContainer/Index", lapUrl, lapVer));
        }
    }
}