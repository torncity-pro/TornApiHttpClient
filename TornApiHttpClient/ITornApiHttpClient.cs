using System.Threading;
using System.Threading.Tasks;
using TornJson.CommonData;

namespace TornApiHttpClient
{
    public interface ITornApiHttpClient
    {
        public Task<T> GetTornDataAsync<T>(string resource, CancellationToken cancellationToken = default) where T : PropertyBagBase;
    }
}
