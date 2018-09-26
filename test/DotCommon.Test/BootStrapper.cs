using Autofac;
 
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
 

namespace DotCommon.Test
{
    public class BootStrapper
    {
        private void Run()
        {
            //Autofac Builder
            
            //var container = builder.Build();
            //Configuration.Instance.AutofacBuild(container);


            //缓存设置
            //Configuration.Instance.UseMemoryCache();

            //Configuration.Instance.CacheConfigureAll(cache =>
            //{
            //    cache.DefaultAbsoluteExpireTime = TimeSpan.FromHours(2);
            //});

            //Configuration.Instance.CacheConfigure("Cache1", cache =>
            //{
            //    cache.DefaultAbsoluteExpireTime = TimeSpan.FromMinutes(5);
            //});

           
        }


 
    }
}
