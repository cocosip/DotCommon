using Autofac;
using DotCommon.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotCommon.Test
{
    public class TestBase
    {
        static TestBase()
        {

            var builder = new ContainerBuilder();
            Configurations.Configuration.Create()
                .UseAutofac(builder)
                .RegisterCommonComponent()
                .UseLog4Net()
                .UseJson4Net()
                .UseMemoryCache()
                .AutofacBuild();
        }

    }
}
