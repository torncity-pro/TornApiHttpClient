using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using TornJson.CommonData;
using TornJson.CompanyData;
using TornJson.Exceptions;
using TornJson.FactionData;
using TornJson.ItemData;
using TornJson.KeyData;
using TornJson.PropertyData;
using TornJson.TornData;
using TornJson.UserData;

namespace TornApiHttpClient
{
    public class TornApiHttpClient : ITornApiHttpClient
    {
        private const string BaseUrl = "https://api.torn.com/";
        private HttpClient Client { get; }

        public TornApiHttpClient() : this(null)
        {

        }

        public TornApiHttpClient(HttpClient client)
        {
            client ??= new HttpClient(new HttpClientHandler() { AutomaticDecompression = System.Net.DecompressionMethods.GZip });
            Client = client;

            Client.BaseAddress = new Uri(BaseUrl);
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region Faction
        public async Task<FactionPropertyBag> GetFactionDataAsync(string ApiKey, string FactionId = default, string Selections = default, string Comment = default, CancellationToken CancellationToken = default)
        {
            return await GetTornDataAsync<FactionPropertyBag>("faction", ApiKey, FactionId, Selections, Comment, CancellationToken).ConfigureAwait(false);
        }
        #endregion

        #region User
        public async Task<UserPropertyBag> GetUserDataAsync(string ApiKey, string UserId = default, string Selections = default, string Comment = default, CancellationToken CancellationToken = default)
        {
            return await GetTornDataAsync<UserPropertyBag>("user", ApiKey, UserId, Selections, Comment, CancellationToken).ConfigureAwait(false);
        }
        #endregion

        #region Property
        public async Task<PropertyPropertyBag> GetPropertyDataAsync(string Apikey, string PropertyId = default, string Selections = default, string Comment = default, CancellationToken CancellationToken = default)
        {
            return await GetTornDataAsync<PropertyPropertyBag>("property", Apikey, PropertyId, Selections, Comment, CancellationToken).ConfigureAwait(false);
        }
        #endregion

        #region Company
        public async Task<CompanyPropertyBag> GetCompanyDataAsync(string ApiKey, string CompanyId = default, string Selections = default, string Comment = default, CancellationToken CancellationToken = default)
        {
            return await GetTornDataAsync<CompanyPropertyBag>("company", ApiKey, CompanyId, Selections, Comment, CancellationToken).ConfigureAwait(false);
        }
        #endregion

        #region Market
        public async Task<ItemMarketPropertyBag> GetItemDataAsync(string ApiKey, string ItemId = default, string Selections = default, string Comment = default, CancellationToken CancellationToken = default)
        {
            return await GetTornDataAsync<ItemMarketPropertyBag>("market", ApiKey, ItemId, Selections, Comment, CancellationToken).ConfigureAwait(false);
        }
        #endregion

        #region Torn
        public async Task<TornPropertyBag> GetTornDataAsync(string ApiKey, string Selections, string Comment = default, CancellationToken CancellationToken = default)
        {
            return await GetTornDataAsync(ApiKey, string.Empty, Selections, Comment, CancellationToken).ConfigureAwait(false);
        }

        public async Task<TornPropertyBag> GetTornDataAsync(string ApiKey, string ResourceId, string Selections, string Comment = default, CancellationToken CancellationToken = default)
        {
            return await GetTornDataAsync<TornPropertyBag>("torn", ApiKey, ResourceId, Selections, Comment, CancellationToken).ConfigureAwait(false);
        }
        #endregion

        #region Key
        public async Task<KeyDataPropertyBag> GetKeyDataAsync(string ApiKey, string Selections = default, string Comment = default, CancellationToken CancellationToken = default)
        {
            return await GetTornDataAsync<KeyDataPropertyBag>("key", ApiKey, string.Empty, Selections ?? "info", Comment, CancellationToken);
        }
        #endregion

        /// <summary>
        /// Gets data from the Torn endpoint
        /// </summary>
        /// <typeparam name="T">The type of the data to deserialize</typeparam>
        /// <param name="endpoint">One of User, Faction, Company, Torn, Property, Market</param>
        /// <param name="resource">The id of what you want to query</param>
        /// <param name="selections">The selections you want to make</param>
        /// <param name="apikey">The apikey to use for the request</param>
        /// <param name="comment">The comment to use for the request</param>
        /// <param name="cancellationToken">The cancellation token for the async</param>
        /// <returns></returns>
        public async Task<T> GetTornDataAsync<T>(string endpoint, string apikey, string resource, string selections, string comment = default, CancellationToken cancellationToken = default) where T : PropertyBagBase
        {
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                throw new ArgumentException("Endpoint cannot be null, empty, or whitespace", nameof(endpoint));
            }
            
            if (string.IsNullOrWhiteSpace(apikey))
            {
                throw new ArgumentException("ApiKey cannot be null, empty, or whitespace", nameof(apikey));
            }

            if (typeof(T) == typeof(TornPropertyBag) && string.IsNullOrWhiteSpace(selections))
            {
                throw new ArgumentException("Selections for Torn endpoint cannot be null or empty", nameof(selections));
            }
            
            if (string.IsNullOrWhiteSpace(resource))
            {
                resource = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(comment))
            {
                comment = string.Empty;
            }
            else
            {
                comment = $"&comment={comment}";
            }

            return await GetTornDataAsync<T>($"{endpoint}/{resource}?key={apikey}&selections={selections}{comment}", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the json response from the api endpoint and deserializes to the given type T
        /// </summary>
        /// <typeparam name="T">The type to deserialize</typeparam>
        /// <param name="resourceString">The web url to query</param>
        /// <param name="cancellationToken">The cancellation token for the async</param>
        /// <returns>A new data object will be returned on success or an error will be thrown</returns>
        public async Task<T> GetTornDataAsync<T>(string resourceString, CancellationToken cancellationToken = default) where T : PropertyBagBase
        {
            var request = new HttpRequestMessage(HttpMethod.Get, resourceString);
            var response = await Client.SendAsync(request, cancellationToken).ConfigureAwait(false);
            request.Dispose();

            if (!response.IsSuccessStatusCode) return default;
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var payload = JsonConvert.DeserializeObject<T>(content);

            if (payload.ErrorInfo != null)
            {
                throw payload.ErrorInfo.CreateExceptionFromExceptionInfo();
            }

            return payload;
        }
    }
}
