using System.Threading.Tasks;

namespace iCloud.Apis.Util.Store
{
    public interface IDataStore
    {
        Task ClearAsync();
        Task DeleteAsync<T>(string key);
        Task<T> GetAsync<T>(string key);
        Task StoreAsync<T>(string key, T value);
    }
}
