using DotCommon.DependencyInjection;
using DotCommon.ObjectMapping;
using DotCommon.Scheduling;
using DotCommon.Serializing;
using DotCommon.Threading;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DotCommon.Test.DependencyInjection
{
    public class ServiceCollectionExtensionsTest
    {
        [Fact]
        public void AddDotCommon_Test()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddDotCommon();

            Assert.Contains(services, x => x.ServiceType == typeof(IJsonSerializer));
            Assert.Contains(services, x => x.ServiceType == typeof(IXmlSerializer));
            Assert.Contains(services, x => x.ServiceType == typeof(IBinarySerializer));
            Assert.Contains(services, x => x.ServiceType == typeof(IObjectSerializer));
            Assert.Contains(services, x => x.ServiceType == typeof(IScheduleService));
            Assert.Contains(services, x => x.ServiceType == typeof(IObjectMapper));
            Assert.Contains(services, x => x.ServiceType == typeof(ICancellationTokenProvider));
            Assert.Contains(services, x => x.ServiceType == typeof(IAmbientDataContext));
            Assert.Contains(services, x => x.ServiceType == typeof(IAmbientScopeProvider<>));

        }

    }
}
