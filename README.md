# DotCommon使用说明

## DotCommon简介

> DotCommon是一个 `C#` 开发的工具类库,封装了一些基本功能,能够使用该工具类库快速的进行开发项目。里面封装了很多基础的功能,如:`Json,Xml,Binary` 序列化, 依赖注入, 日志功能, 定时器, `MD5,Rsa,Base64,Aes,Des`加密解密, 拼音, 进制转换, 模拟请求, 路径转换 等功能。

## 安装

> PM> `Install-Package DotCommon`

## 初始化

```c#
//使用Autofac进行依赖注入
var builder= new ContainerBuilder();
Configuration.Create()
             .UseAutofac(builder)
             .RegisterCommonComponent()
             .UseJson4Net()
             .UseLog4Net()
             .UseProtoBuf()
             .AutofacBuild();

```

> 说明 : DotCommon可以不使用任何的扩展功能,只需要引用 `DotCommon` 包即可,也可以引用扩展功能进行扩展。比如依赖注入,日志,自动的映射功能。也可以自己定义扩展功能，对相应的日志或者序列化等功能进行额外的扩展。使用扩展的时候需要引用相应的扩展包。

## 定时任务

``` c#
Configurations.Configuration.Create()
                .UseAutofac(builder)
                .RegisterCommonComponent()
                .RegisterPeriodicBackgroundWorkers(new List<Assembly>() { typeof(TestBackgroundWorker).Assembly }) //按照程序集注册本地定时任务
                .AutofacBuild()
                .BackgroundWorkersAttechAndRun(); //运行定时任务
```

- 定义定时任务

``` c#
public class TestBackgroundWorker : PeriodicBackgroundWorkerBase
{
    public TestBackgroundWorker(DotCommonTimer timer) : base(timer)
    {
        timer.Period = 1000;
    }

    protected override void DoWork()
    {
        Logger.Info("Work");
    }
}
```

## 扩展包

- **Autofac依赖注入扩展包:** PM> `Install-Package DotCommon.Autofac`
- **缓存扩展包** PM> `Install-Package DotCommon.Caching`
- **AutoMapper自动映射扩展包** PM> `Install-Package DotCommon.AutoMapper`
- **Json4Net序列化扩展包** PM> `Install-Package DotCommon.Json4Net`
- **Log4Net日志扩展包** PM> `Install-Package DotCommon.Log4Net`
- **ProtoBuf二进制序列化扩展包** PM> `Install-Package DotCommon.ProtoBuf`
- **Quartz定时任务扩展包** PM> `Install-Package DotCommon.Quartz`

### 扩展包使用说明

- `缓存扩展` 缓存写法参照 [Abp](https://github.com/aspnetboilerplate/aspnetboilerplate) 项目中缓存写法。
> 配置缓存是基于内存:

```c#
//使用缓存时,不能省略该代码
Configuration.Instance.UseMemoryCache();
```

> 缓存过期时间设置:

```c#
//设置全部的缓存过期时间
Configuration.Instance.CacheConfigureAll(cache =>
{
    cache.DefaultAbsoluteExpireTime = TimeSpan.FromHours(2);
});

//设置某个缓存的过期时间
Configuration.Instance.CacheConfigure("Cache1", cache =>
{
    cache.DefaultAbsoluteExpireTime = TimeSpan.FromMinutes(5);
});
```

> 获取缓存信息:

```c#
var cacheManager = IocManager.GetContainer().Resolve<ICacheManager>();
var cache1 = cacheManager.GetCache("cache1");
ITypedCache<int, string> cache2 = cacheManager.GetCache<int, string>("cache2");

//获取具体某个cache值
var user = cacheManager.GetCache("user").Get<long, AutoMapper.TestUser>(1, x => null);
var order = cacheManager.GetCache("order").Get("100", x => null);

```

- `AutoMapper`对象映射扩展。`DotCommon.AutoMapper`扩展中定义了一些进行快速映射的属性,在类上面添加了这些自动映射属性,并且初始化后,就能直接进行映射使用。
> 初始化自动映射:

```c#
//需要进行自动映射的程序集
var assemblies=new List<Assembly>();
Mapper.Initialize(cfg =>
{
    //初始化自动映射
    AutoAttributeMapperHelper.CreateAutoAttributeMappings(assemblies, cfg);
    //指定的映射
    AutoAttributeMapperHelper.CreateMappings(cfg, x =>
    {
        //在此处定义需要添加的自定义映射
    });
});
```

- `Quartz`定时任务扩展。

```c#
//配置Quartz
 Configurations.Configuration.Create()
               .UseAutofac(builder)
               .RegisterCommonComponent()
               .UseQuartz()
               .RegisterQuartzJobs(new List<Assembly>() { typeof(TestQuartzJob).Assembly })
               .AutofacBuild()
               .AddQuartzListener() //添加Quartz定时任务监听
               .BackgroundWorkersAttechAndRun(); //无论是哪种定时任务,都需要运行此代码
```

```c#
//定义Job
public class TestQuartzJob : JobBase
{
    public override Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"Quartz:{DateTime.Now.ToLongTimeString()}");
        return Task.FromResult(0);
    }
}
```

```c#
//Quartz Job运行规则
async void Schedule()
{
    var manager = IocManager.GetContainer().Resolve<IQuartzScheduleJobManager>();
    await manager.ScheduleAsync<TestQuartzJob>(job =>
    {
        job.WithIdentity("MyLogJobIdentity", "MyGroup")
        .WithDescription("A job to simply write logs.");
    }, trigger =>
    {
        trigger.StartNow()
        .WithSimpleSchedule(schedule =>
        {
            schedule.RepeatForever()
            .WithIntervalInSeconds(1)
            .Build();
        });
    });
}
```