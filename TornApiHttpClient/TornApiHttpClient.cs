using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using TornJson.CommonData;
using TornJson.Exceptions;

namespace TornApiHttpClient
{
    public class TornApiHttpClient : ITornApiHttpClient
    {
        private const string _baseUrl = "https://api.torn.com/";
        private HttpClient _client { get; set; }

        public TornApiHttpClient() : this(null)
        {

        }

        public TornApiHttpClient(HttpClient client)
        {
            client ??= new HttpClient();
            _client = client;

            _client.BaseAddress = new Uri(_baseUrl);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Gets the json response from the api endpoint and deserializes to the given type T
        /// </summary>
        /// <typeparam name="T">The type to deserialize</typeparam>
        /// <param name="resource">The web url to query</param>
        /// <returns>A new data object will be returned on success or an error will be thrown</returns>
        public async Task<T> GetTornDataAsync<T>(string resource, CancellationToken cancellationToken = default) where T : PropertyBagBase
        {
            var request = new HttpRequestMessage(HttpMethod.Get, resource);
            var response = await this._client.SendAsync(request, cancellationToken).ConfigureAwait(false);
            request.Dispose();

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var payload = JsonConvert.DeserializeObject<T>(content);

                if (payload.ErrorInfo != null)
                {
                    throw ApiException.CreateExceptionFromExceptionInfo(payload.ErrorInfo);
                }

                return payload;
            }

            return default;
        }
    }
}
