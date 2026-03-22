using System;
using System.Threading;
using System.Threading.Tasks;
using DotCommon.Threading;
using Xunit;

namespace DotCommon.Test.Threading
{
    public class AmbientDataContextAmbientScopeProviderTest
    {
        [Fact]
        public void GetValue_WithNoScope_ShouldReturnDefault()
        {
            var dataContext = new AsyncLocalAmbientDataContext();
            var provider = new AmbientDataContextAmbientScopeProvider<string>(dataContext);

            var result = provider.GetValue("testKey");

            Assert.Null(result);
        }

        [Fact]
        public void BeginScope_ShouldSetValue()
        {
            var dataContext = new AsyncLocalAmbientDataContext();
            var provider = new AmbientDataContextAmbientScopeProvider<string>(dataContext);

            using (provider.BeginScope("testKey", "testValue"))
            {
                var result = provider.GetValue("testKey");
                Assert.Equal("testValue", result);
            }
        }

        [Fact]
        public void BeginScope_AfterDispose_ShouldRestorePreviousValue()
        {
            var dataContext = new AsyncLocalAmbientDataContext();
            var provider = new AmbientDataContextAmbientScopeProvider<string>(dataContext);

            using (provider.BeginScope("testKey", "outer"))
            {
                using (provider.BeginScope("testKey", "inner"))
                {
                    Assert.Equal("inner", provider.GetValue("testKey"));
                }

                Assert.Equal("outer", provider.GetValue("testKey"));
            }

            Assert.Null(provider.GetValue("testKey"));
        }

        [Fact]
        public void BeginScope_MultipleKeys_ShouldWorkIndependently()
        {
            var dataContext = new AsyncLocalAmbientDataContext();
            var provider = new AmbientDataContextAmbientScopeProvider<string>(dataContext);

            using (provider.BeginScope("key1", "value1"))
            using (provider.BeginScope("key2", "value2"))
            {
                Assert.Equal("value1", provider.GetValue("key1"));
                Assert.Equal("value2", provider.GetValue("key2"));
            }

            Assert.Null(provider.GetValue("key1"));
            Assert.Null(provider.GetValue("key2"));
        }

        [Fact]
        public void BeginScope_WithNullDataContext_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => new AmbientDataContextAmbientScopeProvider<string>(null!));
        }

        [Fact]
        public async Task BeginScope_AcrossAsync_ShouldMaintainScope()
        {
            var dataContext = new AsyncLocalAmbientDataContext();
            var provider = new AmbientDataContextAmbientScopeProvider<string>(dataContext);

            using (provider.BeginScope("testKey", "asyncValue"))
            {
                await Task.Delay(10);
                Assert.Equal("asyncValue", provider.GetValue("testKey"));
            }
        }

        [Fact]
        public void BeginScope_NestedScopes_ShouldRestoreCorrectly()
        {
            var dataContext = new AsyncLocalAmbientDataContext();
            var provider = new AmbientDataContextAmbientScopeProvider<int>(dataContext);

            using (provider.BeginScope("key", 1))
            {
                Assert.Equal(1, provider.GetValue("key"));

                using (provider.BeginScope("key", 2))
                {
                    Assert.Equal(2, provider.GetValue("key"));

                    using (provider.BeginScope("key", 3))
                    {
                        Assert.Equal(3, provider.GetValue("key"));
                    }

                    Assert.Equal(2, provider.GetValue("key"));
                }

                Assert.Equal(1, provider.GetValue("key"));
            }

            Assert.Equal(0, provider.GetValue("key"));
        }
    }
}