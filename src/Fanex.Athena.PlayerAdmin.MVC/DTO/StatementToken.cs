using Newtonsoft.Json;

namespace Fanex.Athena.PlayerAdmin.MVC.DTO
{
    [JsonObject("token")]
    public class StatementToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}