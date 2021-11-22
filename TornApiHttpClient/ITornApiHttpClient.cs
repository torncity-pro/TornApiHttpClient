using System.Threading;
using System.Threading.Tasks;
using TornJson.CommonData;
using TornJson.CompanyData;
using TornJson.FactionData;
using TornJson.ItemData;
using TornJson.KeyData;
using TornJson.PropertyData;
using TornJson.TornData;
using TornJson.UserData;

namespace TornApiHttpClient
{
    public interface ITornApiHttpClient
    {
        Task<FactionPropertyBag> GetFactionDataAsync(string ApiKey, string FactionId = default, string Selections = default, string Comment = default, CancellationToken CancellationToken = default);
        Task<UserPropertyBag> GetUserDataAsync(string Apikey, string UserId = default, string Selections = default, string Comment = default, CancellationToken CancellationToken = default);
        Task<PropertyPropertyBag> GetPropertyDataAsync(string Apikey, string PropertyId = default, string Selections = default, string Comment = default, CancellationToken CancellationToken = default);
        Task<CompanyPropertyBag> GetCompanyDataAsync(string ApiKey, string CompanyId = default, string Selections = default, string Comment = default, CancellationToken CancellationToken = default);
        Task<ItemMarketPropertyBag> GetItemDataAsync(string ApiKey, string ItemId = default, string Selections = default, string Comment = default, CancellationToken CancellationToken = default);
        Task<KeyDataPropertyBag> GetKeyDataAsync(string ApiKey, string Selections = default, string Comment = default, CancellationToken CancellationToken = default);
        Task<TornPropertyBag> GetTornDataAsync(string ApiKey, string Selections, string Comment = default, CancellationToken cancellationToken = default);
        Task<TornPropertyBag> GetTornDataAsync(string ApiKey, string ResourceId, string Selections, string Comment = default, CancellationToken cancellationToken = default);

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
        Task<T> GetTornDataAsync<T>(string endpoint, string resource, string selections, string apikey, string comment = default, CancellationToken cancellationToken = default) where T : PropertyBagBase;

        /// <summary>
        /// Gets the json response from the api endpoint and deserializes to the given type T
        /// </summary>
        /// <typeparam name="T">The type to deserialize</typeparam>
        /// <param name="resourceString">The web url to query</param>
        /// <param name="cancellationToken">The cancellation token for the async</param>
        /// <returns>A new data object will be returned on success or an error will be thrown</returns>
        Task<T> GetTornDataAsync<T>(string resourceString, CancellationToken cancellationToken = default) where T : PropertyBagBase;
    }
}
