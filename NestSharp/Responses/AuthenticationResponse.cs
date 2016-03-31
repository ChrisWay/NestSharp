using System;
using Newtonsoft.Json;

namespace NestSharp.Responses
{
    public class AuthenticationResponse
    {
        [JsonProperty(PropertyName = "access_token")]
        public string Token { get; set; }
        [JsonProperty(PropertyName = "expires_in")]
        public long Expires { get; set; }
    }
}
