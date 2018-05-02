using Bot_LineMessagingApi.SDK.Models;
using Line.Messaging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace Bot_LineMessagingApi.SDK.Managers
{
    public class TokenManager: ITokenManager
    {
        private BotCredential _botCredential;
        private IMemoryCache _memoryCache;
        private const string cacheKey = "ACCESSTOKEN_KEY";
        private const string accessTokenRootUrl = "https://api.line.me/v2";
        private JsonSerializerSettings _jsonSerializerSettings;
        private HttpClient _httpClient;

        public TokenManager (IOptions<BotCredential> botCredential, IMemoryCache memoryCache)
        {
            this._httpClient = new HttpClient();
            this._memoryCache = memoryCache;
            this._botCredential = botCredential.Value;
            this._jsonSerializerSettings = new CamelCaseJsonSerializerSettings();
        }

        /// <summary>
        /// Get Access Token for this Ap Id and App secret
        /// </summary>
        /// <returns></returns>
        public async Task<AccessToken> GetAccessTokenFromCache()
        {
            //Check Cache
            AccessToken accessToken = new AccessToken();
            if(_memoryCache.TryGetValue<AccessToken>(cacheKey, out accessToken))
            {
                return accessToken;
            }
            else
            {
                AccessToken accessTokenFromApi = await GetAccessTokenFromLine();
                accessToken = accessTokenFromApi;
                // Cache option
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(accessToken.expires_in));
                //.SetSlidingExpiration(accessToken.expires_in));
                _memoryCache.Set(cacheKey, accessToken, cacheEntryOptions);
            }
            return accessToken;
        }

        private async Task<AccessToken> GetAccessTokenFromLine()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{accessTokenRootUrl}/oauth/accessToken");
            var queryParams = new List<KeyValuePair<string, string>>();
            queryParams.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            queryParams.Add(new KeyValuePair<string, string>("client_id", _botCredential.AppId));
            queryParams.Add(new KeyValuePair<string, string>("client_secret", _botCredential.AppSecret));
            //string grant_type = "client_credentials";
            //string client_id = _botCredential.AppId;
            //string client_secret = _botCredential.AppSecret;
            //var content = JsonConvert.SerializeObject(new { grant_type, client_id, client_secret}, _jsonSerializerSettings);
            //request.Content = new StringContent("data=" + content, Encoding.UTF8, "application/x-www-form-urlencoded");
            request.Content = new FormUrlEncodedContent(queryParams);

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

            var accessTokenResponse= await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            AccessToken accessToken = JsonConvert.DeserializeObject<AccessToken>(accessTokenResponse, new CamelCaseJsonSerializerSettings());
            return accessToken;
        }
    }
}
