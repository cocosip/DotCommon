using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotCommon.Threading;
using Xunit;

namespace DotCommon.Test.Threading
{
    public class AsyncLocalSimpleScopeExtensionsTest
    {
        [Fact]
        public void SetScoped_ShouldSetValueAndRestore()
        {
            var asyncLocal = new AsyncLocal<string>();
            asyncLocal.Value = "original";

            using (asyncLocal.SetScoped("modified"))
            {
                Assert.Equal("modified", asyncLocal.Value);
            }

            Assert.Equal("original", asyncLocal.Value);
        }

        [Fact]
        public void SetScoped_WithNullPreviousValue_ShouldRestoreNull()
        {
            var asyncLocal = new AsyncLocal<string>();

            using (asyncLocal.SetScoped("value"))
            {
                Assert.Equal("value", asyncLocal.Value);
            }

            Assert.Null(asyncLocal.Value);
        }

        [Fact]
        public void SetScoped_NestedScopes_ShouldRestoreCorrectly()
        {
            var asyncLocal = new AsyncLocal<string>();
            asyncLocal.Value = "level0";

            using (asyncLocal.SetScoped("level1"))
            {
                Assert.Equal("level1", asyncLocal.Value);

                using (asyncLocal.SetScoped("level2"))
                {
                    Assert.Equal("level2", asyncLocal.Value);
                }

                Assert.Equal("level1", asyncLocal.Value);
            }

            Assert.Equal("level0", asyncLocal.Value);
        }

        [Fact]
        public void SetScoped_WithReferenceType_ShouldWork()
        {
            var asyncLocal = new AsyncLocal<List<int>>();
            var originalList = new List<int> { 1, 2, 3 };
            asyncLocal.Value = originalList;

            var newList = new List<int> { 4, 5, 6 };
            using (asyncLocal.SetScoped(newList))
            {
                Assert.Same(newList, asyncLocal.Value);
            }

            Assert.Same(originalList, asyncLocal.Value);
        }

        [Fact]
        public async Task SetScoped_AcrossAsync_ShouldWork()
        {
            var asyncLocal = new AsyncLocal<string>();
            asyncLocal.Value = "original";

            await Task.Run(async () =>
            {
                using (asyncLocal.SetScoped("async"))
                {
                    await Task.Delay(10);
                    Assert.Equal("async", asyncLocal.Value);
                }
            });

            Assert.Equal("original", asyncLocal.Value);
        }
    }
}