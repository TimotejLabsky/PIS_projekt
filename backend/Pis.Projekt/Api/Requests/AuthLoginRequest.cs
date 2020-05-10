using Newtonsoft.Json;

namespace Pis.Projekt.Api.Requests
{
    [JsonObject]
    public class AuthLoginRequest
    {
        [JsonProperty("userName")]
        public string User { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}