using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NestSharp.Responses;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace NestSharp
{
    public class NestApi
    {
        private const string BaseUrl = "https://developer-api.nest.com/";

        private readonly HttpClient _httpClient;

        public NestApi(HttpMessageHandler messageHandler = null) {
            _httpClient = messageHandler == null ? new HttpClient() : new HttpClient(messageHandler);

            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public string AccessToken { get; protected set; }

        public async Task<string> GetAccessToken(string code, string clientId, string clientSecret) {
            string url = $"https://api.home.nest.com/oauth2/access_token?code={code}&client_id={clientId}&client_secret={clientSecret}&grant_type=authorization_code";

            var result = await _httpClient.PostAsync(url, new StringContent(null)).ConfigureAwait(false);
            if (result.StatusCode == HttpStatusCode.OK) {
                AccessToken = JsonConvert.DeserializeObject<AuthenticationResponse>(await result.Content.ReadAsStringAsync().ConfigureAwait(false)).Token;
                return AccessToken;
            }
            else {
                var error = JsonConvert.DeserializeObject<ErrorResponse>(await result.Content.ReadAsStringAsync().ConfigureAwait(false));

                throw new Exception(error.Error);
            }
        }

        public async Task GetDevicesAsync() {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"devices?auth={AccessToken}");
            
            await _httpClient.SendAsync(request)
                .ContinueWith(r => return r.Result.Content.ReadAsStringAsync())
        }

        //public async Task<Devices> GetDevicesAsync() {
        //    CheckAuth();

        //    return await BaseUrl
        //        .AppendPathSegment("devices")
        //        .SetQueryParam("auth", AccessToken)
        //        .WithHeader("Accept", "application/json")
        //        .GetJsonAsync<Devices>();
        //}
    }
}
