using Newtonsoft.Json;

namespace Pis.Projekt.Api.Responses
{
    [JsonObject]
    public class AuthLoginResponse
    {
        [JsonProperty("userName")]
        public string User { get; set; }
    }
}