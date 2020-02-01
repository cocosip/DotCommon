using DotCommon.Caching;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DotCommon.Test.Caching
{
    public class CachingTest : CacheTestProvider
    {


        [Fact]
        public async Task Should_Set_Get_And_Remove_Cache_Items()
        {
            var personCache = Provider.GetRequiredService<IDistributedCache<PersonCacheItem>>();

            var cacheKey = Guid.NewGuid().ToString();
            const string personName = "john nash";

            //Get (not exists yet)
            var cacheItem = await personCache.GetAsync(cacheKey);
            Assert.Null(cacheItem);
            var cacheItem1 = personCache.Get(cacheKey);
            Assert.Equal(cacheItem, cacheItem1);

            //Set
            cacheItem = new PersonCacheItem(personName);
            await personCache.SetAsync(cacheKey, cacheItem);

            //Get (it should be available now
            cacheItem = await personCache.GetAsync(cacheKey);
            Assert.NotNull(cacheItem);
            Assert.Equal(personName, cacheItem.Name);

            //Remove 
            await personCache.RemoveAsync(cacheKey);

            //Get (not exists since removed)
            cacheItem = await personCache.GetAsync(cacheKey);
            Assert.Null(cacheItem);

        }

        [Fact]
        public async Task GetOrAddAsync()
        {
            var personCache = Provider.GetRequiredService<IDistributedCache<PersonCacheItem>>();

            var cacheKey = Guid.NewGuid().ToString();
            const string personName = "john nash";

            //Will execute the factory method to create the cache item

            bool factoryExecuted = false;

            var cacheItem = await personCache.GetOrAddAsync(cacheKey,
                async () =>
                {
                    factoryExecuted = true;
                    return await Task.FromResult(new PersonCacheItem(personName));
                });
            Assert.True(factoryExecuted);
            Assert.Equal(personName, cacheItem.Name);

            //This time, it will not execute the factory

            factoryExecuted = false;

            cacheItem = await personCache.GetOrAddAsync(cacheKey,
                async () =>
                {
                    factoryExecuted = true;
                    return await Task.FromResult(new PersonCacheItem(personName));
                });
            Assert.False(factoryExecuted);
            Assert.Equal(personName, cacheItem.Name);
        }

    }
}
