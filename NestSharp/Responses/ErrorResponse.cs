using System;
using Newtonsoft.Json;

namespace NestSharp.Responses
{
    public class ErrorResponse
    {
        public string Error { get; set; }
        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
    }
}
