using Autofac;
using DotCommon.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotCommon.Test
{
    public class TestBase
    {
        public static bool IsInit = false;
        private static object syncObject = new object();
        static TestBase()
        {
            if (!IsInit)
            {
                lock (syncObject)
                {
                    var builder = new ContainerBuilder();
                    Configurations.Configuration.Create()
                        .UseAutofac(builder)
                        .RegisterCommonComponent()
                        .UseLog4Net()
                        .UseJson4Net()
                        .UseMemoryCache()
                        .AutofacBuild();
                    IsInit = true;
                }
            }

          
        }

    }
}
