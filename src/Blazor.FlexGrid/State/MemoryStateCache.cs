using Microsoft.Extensions.Caching.Memory;
using System;

namespace Blazor.FlexGrid.State
{
    public class MemoryStateCache : IStateCache
    {
        private readonly MemoryCache memoryCache;

        public int Count => memoryCache.Count;

        public MemoryStateCache(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache as MemoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public bool TryGetStateValue<T>(string key, out T value) => memoryCache.TryGetValue(key, out value);

        public void RemoveStateValue(string key) => memoryCache.Remove(key);

        public void SetStateValue<T>(string key, T value) => memoryCache.Set(key, value);
    }
}
