# DotCommon使用说明

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

- **Autofac依赖注入扩展包:** PM> `Install-Package DotCommon.Autofac`
- **缓存扩展包** PM> `Install-Package DotCommon.Caching`
- **AutoMapper自动映射扩展包** PM> `Install-Package DotCommon.AutoMapper`
- **Json4Net序列化扩展包** PM> `Install-Package DotCommon.Json4Net`
- **Log4Net日志扩展包** PM> `Install-Package DotCommon.Log4Net`
- **ProtoBuf二进制序列化扩展包** PM> `Install-Package DotCommon.ProtoBuf`

### 扩展包使用说明

- `缓存扩展` 缓存写法参照 [Abp](https://github.com/aspnetboilerplate/aspnetboilerplate) 项目中缓存写法。
> 配置缓存是基于内存:

```c#
services.AddGenericsMemoryCache();
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
