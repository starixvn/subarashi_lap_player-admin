using Fanex.Athena.PlayerAdmin.Infrastructure.Filters;
using Fanex.Athena.PlayerAdmin.Infrastructure.Settings;
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

namespace Fanex.Athena.PlayerAdmin.Auth.Controllers
{
    public class AuthController : Controller
    {
        private const string TokenKey = "token";

        private readonly IdentityServerSetting _setting;
        private readonly HttpClient _client;

        public AuthController(IOptions<IdentityServerSetting> setting, HttpClient client)
        {
            _setting = setting.Value;
            _client = client;
        }

        [Route("api/auth/token")]
        [HttpPost]
        [ReferrerValidation]
        [EnableCors]
        [AllowAnonymous]
        [ModelStateValidation]
        public async Task<JsonResult> Token([FromBody] LapTokenRequest request)
        {
            var tokenResponse = await RequestToken(request.Token);
            return new JsonResult(new
            {
                IsSuccess = !tokenResponse.IsError,
                Msg = tokenResponse.ErrorDescription,
                AccessToken = tokenResponse.AccessToken
            });
        }

        [Route("auth/cookie")]
        [HttpGet]
        [ReferrerValidation]
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult> Cookie([FromQuery] string token)
        {
            var tokenResponse = await RequestToken(token);
            if(tokenResponse.IsError)
            {
                return Redirect("/Home/AccessDenied");
            }

            var userInfo = await _client.GetUserInfoAsync(new UserInfoRequest
            {
                Address = $"{_setting.Host}/{_setting.UserInfoEndpoint}",
                Token = tokenResponse.AccessToken
            });

            var claims = new List<Claim> { new Claim("AccessToken", tokenResponse.AccessToken) };
            claims.AddRange(userInfo.Claims);
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");
            var actionLinkClaim = userInfo.Claims.FirstOrDefault(_ => _.Type.Equals("ActionLink", StringComparison.OrdinalIgnoreCase));

            await TrySignOut();
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            if (!string.IsNullOrEmpty(actionLinkClaim?.Value))
            {
                return Redirect(Url.Content("~/" + actionLinkClaim?.Value));
            }

            return Redirect("/");
        }

        private async Task TrySignOut()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }

        private async Task<TokenResponse> RequestToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return ProtocolResponse.FromException<TokenResponse>(new ArgumentException($"token is not present"));
            }

            var tokenResponse = await _client.RequestTokenAsync(new TokenRequest
            {
                Address = $"{_setting.Host}/{_setting.TokenEndpoint}",
                ClientId = _setting.CredentialsClient.ClientId,
                ClientSecret = _setting.CredentialsClient.ClientSecret,
                GrantType = _setting.GrantType,
                Parameters = new Parameters(new Dictionary<string, string> { { TokenKey, token } })
            });

            return tokenResponse;
        }

        [HttpGet]
        [Route("auth/RedirectToLAP/{lapUrl}/{lapVer?}")]
        [AllowAnonymous]
        public ActionResult RedirectToLAP([FromRoute] string lapUrl, [FromRoute] string lapVer = "")
        {
            return Redirect(string.Format("http://{0}/{1}/12bet/Auth/Login?returnUrl=http://{2}/Home/Menu", lapUrl, lapVer, HttpContext.Request.Host));
        }
    }
}