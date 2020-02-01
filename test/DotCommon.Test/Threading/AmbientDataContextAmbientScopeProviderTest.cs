using DotCommon.Threading;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Threading
{
    public class AmbientDataContextAmbientScopeProviderTest
    {
        [Fact]
        public void GetValue_BeginScope_Test()
        {
            IAmbientDataContext dataContext = new AsyncLocalAmbientDataContext();

            IAmbientScopeProvider<string> scopeProvider = new AmbientDataContextAmbientScopeProvider<string>(dataContext);

            var contextKey = "contextKey1";
            var value1 = scopeProvider.GetValue(contextKey);
            Assert.Null(value1);

            using (scopeProvider.BeginScope(contextKey, "123456"))
            {
                var value2 = scopeProvider.GetValue(contextKey);
                Assert.Equal("123456", value2);
            }

            var value3 = scopeProvider.GetValue(contextKey);
            Assert.Null(value3);

 

        }
    }
}
