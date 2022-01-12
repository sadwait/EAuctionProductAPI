using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellerAPI.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task<T> Get<T>(string key) where T : class
        {
            var cacheResponse = await _cache.GetStringAsync(key);
            dynamic result = null;
            if (cacheResponse != null)
                result = JsonConvert.DeserializeObject<T>(cacheResponse);
            return result;
        }

        public async Task Set<T>(string key, string value) where T : class
        {
            await _cache.SetStringAsync(key, value);
        }
    }
}
