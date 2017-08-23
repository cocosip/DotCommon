using Autofac;
using DotCommon.Autofac;
using DotCommon.Configurations;
using DotCommon.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
namespace DotCommon.Test
{
    public class BootStrapper
    {
        public void Run()
        {
            //Autofac Builder
            var builder= new ContainerBuilder();
            Configuration.Create()
                .UseAutofac(builder)
                .RegisterCommonComponent()
                .UseJson4Net()
                .UseLog4Net()
                .UseProtoBuf()
                .AutofacBuild();
            //var container = builder.Build();
            //Configuration.Instance.AutofacBuild(container);
                

             
        }
    }
}
