using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace Fanex.Athena.PlayerAdmin.Auth.Controllers
{
    public class ExternalAuthController : Controller
    {
        public ExternalAuthController()
        {
        }


        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult> Session(string token, string redirectUrl)
        {
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");

            // request token
            
            //var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            //{
            //    Address = disco.TokenEndpoint,
            //    ClientId = "playeradmin.client1",
            //    ClientSecret = "playeradmin.secret1",
            //    UserName = "howard",
            //    Password = "1234aa",
            //    Parameters = new Dictionary<string, string> { { "login_token", "asd12312" } }
            //});
            var accesstoken = "190CE920318A3849F56989721DD62CF92D4F68D24BC09A644B5C938D2560C42F1a";

            var tokenResponse = await client.RequestTokenAsync(new TokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "playeradmin.client",
                ClientSecret = "playeradmin.secret",
                GrantType = "lrf_password",
                Parameters = new Dictionary<string, string> { { "username", "pgmalex" }, { "password", "1234aa" }, { "login_token", "asd12312" } }
            });

            

            var info = await client.GetUserInfoAsync(new UserInfoRequest
            {
                Address = disco.UserInfoEndpoint,
                Token = tokenResponse.AccessToken
            });

            //var a = await client.RevokeTokenAsync(new TokenRevocationRequest
            //{
            //    ClientId = "playeradmin.client",
            //    ClientSecret = "playeradmin.secret",
            //    Address = disco.RevocationEndpoint,
            //    Token = accesstoken
            //});

            var x = await client.IntrospectTokenAsync(new TokenIntrospectionRequest
            {
                 Token = tokenResponse.AccessToken,
                 Address = disco.IntrospectionEndpoint,
                ClientId = "playeradmin_api",
                ClientSecret = "playeradmin_api.secret",

            });

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(accesstoken);

            var response = await apiClient.GetAsync("http://localhost:5001/identity");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "howard"),
                new Claim("FullName", "howard nguyen"),
                new Claim(ClaimTypes.Role, "Administrator"),
                new Claim(ClaimTypes.UserData, "")
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity)
                );

            if (!string.IsNullOrEmpty(redirectUrl) && Url.IsLocalUrl(redirectUrl))
            {
                return Redirect(redirectUrl);
            }

            return new NoContentResult();
        }
    }
}