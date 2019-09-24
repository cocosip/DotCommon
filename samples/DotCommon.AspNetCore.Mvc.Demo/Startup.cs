using DotCommon.AspNetCore.Mvc.Cors;
using DotCommon.AspNetCore.Mvc.Demo.Controllers;
using DotCommon.AspNetCore.Mvc.Demo.Services;
using DotCommon.AspNetCore.Mvc.Formatters;
using DotCommon.Caching;
using DotCommon.DependencyInjection;
using DotCommon.Json4Net;
using DotCommon.Log4Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DotCommon.AspNetCore.Mvc.Demo
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var mvcBuilder = services.AddControllersWithViews();
            mvcBuilder.AddControllersAsServices();

            services.AddTransient<HomeController>();
            services.AddTransient<UserService>();


            services.Configure<MvcOptions>(o => { });
            services
                .AddLogging(l =>
                {
                    l.AddLog4Net();
                })
                .AddDotCommon()
                .AddJson4Net()
                .AddGenericsMemoryCache() //自定义缓存
                .AddWildcardCors("http://*.cnblog.com,http://www.baidu.com") //通配符跨域
                .AddRawRequestBodyFormatter() //Get([Frombody]string name)
                .AddServiceControllers(o =>
                {
                    o.ConventionalControllers.Create(this.GetType().Assembly, c =>
                    {
                        c.RootPath = "services";
                        c.UrlActionNameNormalizer = f =>
                        {
                            return "";
                        };
                    });
                });

            // return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Cors

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.ConfigureDotCommon();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

    }
}
