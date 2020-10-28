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
            client ??= new HttpClient();
            Client = client;

            Client.BaseAddress = new Uri(BaseUrl);
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region Faction
        public async Task<FactionPropertyBag> GetFactionDataAsync(string apikey, CancellationToken cancellationToken = default)
        {
            return await GetFactionDataAsync(apikey, string.Empty, string.Empty, cancellationToken).ConfigureAwait(false);
        }

        public async Task<FactionPropertyBag> GetFactionDataAsync(string apikey, string selections, CancellationToken cancellationToken = default)
        {
            return await GetFactionDataAsync(apikey, string.Empty, selections, cancellationToken).ConfigureAwait(false);
        }

        public async Task<FactionPropertyBag> GetFactionDataAsync(string apikey, string factionId, string selections, CancellationToken cancellationToken = default)
        {
            return await GetTornDataAsync<FactionPropertyBag>("faction", apikey, factionId, selections, cancellationToken).ConfigureAwait(false);
        }
        #endregion

        #region User
        public async Task<UserPropertyBag> GetUserDataAsync(string apikey, CancellationToken cancellationToken = default)
        {
            return await GetUserDataAsync(apikey, string.Empty, string.Empty, cancellationToken).ConfigureAwait(false);
        }

        public async Task<UserPropertyBag> GetUserDataAsync(string apikey, string selections, CancellationToken cancellationToken = default)
        {
            return await GetUserDataAsync(apikey, string.Empty, selections, cancellationToken).ConfigureAwait(false);
        }

        public async Task<UserPropertyBag> GetUserDataAsync(string apikey, string userId, string selections, CancellationToken cancellationToken = default)
        {
            return await GetTornDataAsync<UserPropertyBag>("user", apikey, userId, selections, cancellationToken).ConfigureAwait(false);
        }
        #endregion

        #region Property
        public async Task<PropertyPropertyBag> GetPropertyDataAsync(string apikey, CancellationToken cancellationToken = default)
        {
            return await GetPropertyDataAsync(apikey, string.Empty, string.Empty, cancellationToken).ConfigureAwait(false);
        }

        public async Task<PropertyPropertyBag> GetPropertyDataAsync(string apikey, string selections, CancellationToken cancellationToken = default)
        {
            return await GetPropertyDataAsync(apikey, string.Empty, selections, cancellationToken).ConfigureAwait(false);
        }

        public async Task<PropertyPropertyBag> GetPropertyDataAsync(string apikey, string propertyId, string selections, CancellationToken cancellationToken = default)
        {
            return await GetTornDataAsync<PropertyPropertyBag>("property", apikey, propertyId, selections, cancellationToken).ConfigureAwait(false);
        }
        #endregion

        #region Company
        public async Task<CompanyPropertyBag> GetCompanyDataAsync(string apikey, CancellationToken cancellationToken = default)
        {
            return await GetCompanyDataAsync(apikey, string.Empty, string.Empty, cancellationToken).ConfigureAwait(false);
        }

        public async Task<CompanyPropertyBag> GetCompanyDataAsync(string apikey, string selections, CancellationToken cancellationToken = default)
        {
            return await GetCompanyDataAsync(apikey, string.Empty, selections, cancellationToken).ConfigureAwait(false);
        }

        public async Task<CompanyPropertyBag> GetCompanyDataAsync(string apikey, string companyId, string selections, CancellationToken cancellationToken = default)
        {
            return await GetTornDataAsync<CompanyPropertyBag>("company", apikey, companyId, selections, cancellationToken).ConfigureAwait(false);
        }
        #endregion

        #region Market
        public async Task<ItemMarketPropertyBag> GetItemDataAsync(string apikey, CancellationToken cancellationToken = default)
        {
            return await GetItemDataAsync(apikey, string.Empty, string.Empty, cancellationToken).ConfigureAwait(false);
        }

        public async Task<ItemMarketPropertyBag> GetItemDataAsync(string apikey, string selections, CancellationToken cancellationToken = default)
        {
            return await GetItemDataAsync(apikey, string.Empty, selections, cancellationToken).ConfigureAwait(false);
        }

        public async Task<ItemMarketPropertyBag> GetItemDataAsync(string apikey, string itemId, string selections, CancellationToken cancellationToken = default)
        {
            return await GetTornDataAsync<ItemMarketPropertyBag>("market", apikey, itemId, selections, cancellationToken).ConfigureAwait(false);
        }
        #endregion

        #region Torn
        public async Task<TornPropertyBag> GetTornDataAsync(string apikey, string selections, CancellationToken cancellationToken = default)
        {
            return await GetTornDataAsync(apikey, string.Empty, selections, cancellationToken).ConfigureAwait(false);
        }

        public async Task<TornPropertyBag> GetTornDataAsync(string apikey, string resourceId, string selections, CancellationToken cancellationToken = default)
        {
            return await GetTornDataAsync<TornPropertyBag>("torn", apikey, resourceId, selections, cancellationToken).ConfigureAwait(false);
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
        /// <param name="cancellationToken">The cancellation token for the async</param>
        /// <returns></returns>
        public async Task<T> GetTornDataAsync<T>(string endpoint, string apikey, string resource, string selections, CancellationToken cancellationToken = default) where T : PropertyBagBase
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

            return await GetTornDataAsync<T>($"{endpoint}/{resource}?key={apikey}&selections={selections}", cancellationToken).ConfigureAwait(false);
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
