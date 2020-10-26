using System.Threading;
using System.Threading.Tasks;
using TornJson.CommonData;

namespace TornApiHttpClient
{
    public interface ITornApiHttpClient
    {
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
        public Task<T> GetTornDataAsync<T>(string endpoint, string resource, string selections, string apikey, CancellationToken cancellationToken = default) where T : PropertyBagBase;

        /// <summary>
        /// Gets the json response from the api endpoint and deserializes to the given type T
        /// </summary>
        /// <typeparam name="T">The type to deserialize</typeparam>
        /// <param name="resource">The web url to query</param>
        /// <param name="cancellationToken">The cancellation token for the async</param>
        /// <returns>A new data object will be returned on success or an error will be thrown</returns>
        public Task<T> GetTornDataAsync<T>(string resource, CancellationToken cancellationToken = default) where T : PropertyBagBase;
    }
}
