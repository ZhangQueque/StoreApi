using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
namespace Store.Api.RedisCache
{

    /// <summary>
    /// redis缓冲帮助类
    /// </summary>
    public class RedisCacheHelper
    {
        private readonly IDistributedCache distributedCache;

        public RedisCacheHelper(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }
        public async Task SetRedisCacheAsync<T>(string key, T t)
        {
            string json = JsonSerializer.Serialize<T>(t);
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddHours(1)
            };
            await distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(json), options);
        }

        public Task<T> GetRedisCacheAsync<T>(byte[] bytes)
        {
            return Task.FromResult(JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(bytes)));
        }
    }
}
