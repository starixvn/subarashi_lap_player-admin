using Fanex.Athena.PlayerAdmin.Infrastructure.Configs;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> Token([FromBody]string token)
        {
            var tokenResponse = await _client.RequestTokenAsync(new TokenRequest
            {
                Address = $"{_setting.Host}/{_setting.TokenEndpoint}",
                ClientId = _setting.ClientId,
                ClientSecret = _setting.ClientSecret,
                GrantType = _setting.GrantType,
                Parameters = new Parameters(new Dictionary<string, string> { { TokenKey, token } })
            });

            return new JsonResult(new
            {
                IsSuccess = !tokenResponse.IsError && !string.IsNullOrEmpty(tokenResponse.AccessToken),
                AccessToken = tokenResponse.AccessToken
            });
        }

        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult> Cookie([FromQuery]string token)
        {
            var tokenResponse = await _client.RequestTokenAsync(new TokenRequest
            {
                Address = $"{_setting.Host}/{_setting.TokenEndpoint}",
                ClientId = _setting.ClientId,
                ClientSecret = _setting.ClientSecret,
                GrantType = _setting.GrantType,
                Parameters = new Parameters(new Dictionary<string, string> { { TokenKey, token } })
            });

            var userInfo = await _client.GetUserInfoAsync(new UserInfoRequest
            {
                Address = $"{_setting.Host}/{_setting.UserInfoEndpoint}",
                Token = tokenResponse.AccessToken
            });

            var claimsIdentity = new ClaimsIdentity(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");
            var actionLinkClaim = userInfo.Claims.FirstOrDefault(_ => _.Type.Equals("ActionLink", System.StringComparison.OrdinalIgnoreCase));

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            if (!string.IsNullOrEmpty(actionLinkClaim?.Value))
            {
                return Redirect(Url.Content("~/" + actionLinkClaim?.Value));
            }

            return Redirect("/home/info");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult RedirectToLAP()
        {
            return Redirect("http://localhost:62570/12bet/Auth/Login?returnUrl=http://localhost:51006/Home/Menu");
        }
    }
}