# DotCommon使用说明

[![996.icu](https://img.shields.io/badge/link-996.icu-red.svg)](https://996.icu) [![GitHub](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/cocosip/DotCommon/blob/master/LICENSE) ![GitHub last commit](https://img.shields.io/github/last-commit/cocosip/DotCommon.svg) ![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/cocosip/DotCommon.svg)

[![build and publish](https://github.com/cocosip/DotCommon/actions/workflows/publish.yml/badge.svg)](https://github.com/cocosip/DotCommon/actions/workflows/publish.yml)

| Package  | Version | Downloads|
| -------- | ------- | -------- |
| `DotCommon` | [![NuGet](https://img.shields.io/nuget/v/DotCommon.svg)](https://www.nuget.org/packages/DotCommon) |![NuGet](https://img.shields.io/nuget/dt/DotCommon.svg)|
| `DotCommon.AutoMapper` | [![NuGet](https://img.shields.io/nuget/v/DotCommon.AutoMapper.svg)](https://www.nuget.org/packages/DotCommon.AutoMapper) |![NuGet](https://img.shields.io/nuget/dt/DotCommon.AutoMapper.svg)|
| `DotCommon.Caching` | [![NuGet](https://img.shields.io/nuget/v/DotCommon.Caching.svg)](https://www.nuget.org/packages/DotCommon.Caching) |![NuGet](https://img.shields.io/nuget/dt/DotCommon.Caching.svg)|
| `DotCommon.Json4Net` | [![NuGet](https://img.shields.io/nuget/v/DotCommon.Json4Net.svg)](https://www.nuget.org/packages/DotCommon.Json4Net) |![NuGet](https://img.shields.io/nuget/dt/DotCommon.Json4Net.svg)|
| `DotCommon.TextJson` | [![NuGet](https://img.shields.io/nuget/v/DotCommon.TextJson.svg)](https://www.nuget.org/packages/DotCommon.TextJson) |![NuGet](https://img.shields.io/nuget/dt/DotCommon.TextJson.svg)|
| `DotCommon.Log4Net` | [![NuGet](https://img.shields.io/nuget/v/DotCommon.Log4Net.svg)](https://www.nuget.org/packages/DotCommon.Log4Net) |![NuGet](https://img.shields.io/nuget/dt/DotCommon.Log4Net.svg)|
| `DotCommon.ProtoBuf` | [![NuGet](https://img.shields.io/nuget/v/DotCommon.ProtoBuf.svg)](https://www.nuget.org/packages/DotCommon.ProtoBuf) |![NuGet](https://img.shields.io/nuget/dt/DotCommon.ProtoBuf.svg)|
| `DotCommon.AspNetCore.Mvc` | [![NuGet](https://img.shields.io/nuget/v/DotCommon.AspNetCore.Mvc.svg)](https://www.nuget.org/packages/DotCommon.AspNetCore.Mvc) |![NuGet](https://img.shields.io/nuget/dt/DotCommon.AspNetCore.Mvc.svg)|
| `DotCommon.ImageUtility` | [![NuGet](https://img.shields.io/nuget/v/DotCommon.ImageUtility.svg)](https://www.nuget.org/packages/DotCommon.ImageUtility) |![NuGet](https://img.shields.io/nuget/dt/DotCommon.ImageUtility.svg)|
| `DotCommon.ImageResizer.AspNetCore.Mvc` | [![NuGet](https://img.shields.io/nuget/v/DotCommon.ImageResizer.AspNetCore.Mvc.svg)](https://www.nuget.org/packages/DotCommon.ImageResizer.AspNetCore.Mvc) |![NuGet](https://img.shields.io/nuget/dt/DotCommon.ImageResizer.AspNetCore.Mvc.svg)|

## DotCommon简介

> DotCommon是一个 `C#` 开发的工具类库,封装了一些基本功能,能够使用该工具类库快速的进行开发项目。里面封装了很多基础的功能,如:`Json,Xml,Binary` 序列化, 依赖注入, 日志功能, 定时器, `MD5,Rsa,Base64,Aes,Des`加密解密, 拼音, 进制转换, 模拟请求, 路径转换 等功能。

## 安装

> PM> `Install-Package DotCommon`

## 初始化

```c#
services.AddLogging(c =>
{
     c.AddLog4Net(new Log4NetProviderOptions());
})
.AddCommonComponents()
.AddGenericsMemoryCache()
.AddProtoBuf()
.AddJson4Net();
```

## 扩展包

- **Autofac依赖注入扩展包:** `DotCommon.Autofac`
- **缓存扩展包** `DotCommon.Caching`
- **AutoMapper自动映射扩展包** `DotCommon.AutoMapper`
- **Json4Net序列化扩展包** `DotCommon.Json4Net`
- **Log4Net日志扩展包** `DotCommon.Log4Net`
- **ProtoBuf二进制序列化扩展包** `DotCommon.ProtoBuf`
- **AspNetCore扩展** `DotCommon.AspNetCore.Mvc`
- **ImageUtility图片扩展** `DotCommon.ImageUtility`
- **ImageResizer图片缩放(Asp.Net Core)** `DotCommon.ImageResizer.AspNetCore.Mvc`

### 扩展包使用说明

- `缓存扩展`

```c#
IServiceCollection services = new ServiceCollection();
services
    .AddDotCommon()
    .AddGenericsMemoryCache();
var provider = services.BuildServiceProvider();
//获取缓存管理器
var personCache = provider.GetRequiredService<IDistributedCache<PersonCacheItem>>();
//根据key值获取缓存
var cacheItem = await personCache.GetAsync("key1");
//设置缓存
await personCache.SetAsync("key2", cacheItem);
```

> 配置缓存是基于内存:

```c#
services.AddGenericsMemoryCache();

```

- `AutoMapper`对象映射扩展。`DotCommon.AutoMapper`扩展中定义了一些进行快速映射的属性,在类上面添加了这些自动映射属性,并且初始化后,就能直接进行映射使用。

> 初始化自动映射:

```c#
//需要进行自动映射的程序集
var assemblies=new List<Assembly>();

IServiceCollection services = new ServiceCollection();
services
    .AddDotCommon()
    .AddDotCommonAutoMapper()
    .AddAssemblyAutoMaps(assemblies.ToArray())
    .AddAutoMapperConfigurator(c =>
    {
        //自定义的一些AutoMapper配置
    })
    .BuildAutoMapper();
var provider = services.BuildServiceProvider();

```

## 数据库参数与字段说明

> 数据库参数与字段说明 [详见](/docs/database.md)