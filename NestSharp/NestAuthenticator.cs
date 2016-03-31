using NestSharp.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NestSharp
{
    public class NestAuthenticator
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly HttpClient _httpClient;

        public NestAuthenticator(string clientId, string clientSecret, HttpMessageHandler messageHandler = null) {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _httpClient = messageHandler == null ? new HttpClient() : new HttpClient(messageHandler);

            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<AuthenticationResponse> GetAccessToken(string code) {
            if(string.IsNullOrEmpty(code)) {
                throw new ArgumentNullException("code");
            }

            var url = $"https://api.home.nest.com/oauth2/access_token?code={code}&client_id={_clientId}&client_secret={_clientSecret}&grant_type=authorization_code";

            var result = await _httpClient.PostAsync(url, new StringContent(null)).ConfigureAwait(false);
            if (result.IsSuccessStatusCode) {
                return JsonConvert.DeserializeObject<AuthenticationResponse>(await result.Content.ReadAsStringAsync().ConfigureAwait(false));
            } else {
                var error = JsonConvert.DeserializeObject<ErrorResponse>(await result.Content.ReadAsStringAsync().ConfigureAwait(false));

                throw new NestException(error);
            }
        }

        public string GetAuthorisationURL() {
            return $"https://home.nest.com/login/oauth2?client_id={_clientSecret}&state={Guid.NewGuid()}";
        }
    }
}
