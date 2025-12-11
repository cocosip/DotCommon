# DotCommon

[![996.icu](https://img.shields.io/badge/link-996.icu-red.svg)](https://996.icu) [![GitHub](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/cocosip/DotCommon/blob/master/LICENSE) ![GitHub last commit](https://img.shields.io/github/last-commit/cocosip/DotCommon.svg) ![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/cocosip/DotCommon.svg)

[![CI/CD Pipeline](https://github.com/cocosip/DotCommon/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/cocosip/DotCommon/actions/workflows/ci-cd.yml)

| Package  | Version | Downloads|
| -------- | ------- | -------- |
| `DotCommon` | [![NuGet](https://img.shields.io/nuget/v/DotCommon.svg)](https://www.nuget.org/packages/DotCommon) |![NuGet](https://img.shields.io/nuget/dt/DotCommon.svg)|
| `DotCommon.AutoMapper` | [![NuGet](https://img.shields.io/nuget/v/DotCommon.AutoMapper.svg)](https://www.nuget.org/packages/DotCommon.AutoMapper) |![NuGet](https://img.shields.io/nuget/dt/DotCommon.AutoMapper.svg)|
| `DotCommon.Caching` | [![NuGet](https://img.shields.io/nuget/v/DotCommon.Caching.svg)](https://www.nuget.org/packages/DotCommon.Caching) |![NuGet](https://img.shields.io/nuget/dt/DotCommon.Caching.svg)|
| `DotCommon.Crypto` | [![NuGet](https://img.shields.io/nuget/v/DotCommon.Crypto.svg)](https://www.nuget.org/packages/DotCommon.Crypto) |![NuGet](https://img.shields.io/nuget/dt/DotCommon.Crypto.svg)|
| `DotCommon.AspNetCore.Mvc` | [![NuGet](https://img.shields.io/nuget/v/DotCommon.AspNetCore.Mvc.svg)](https://www.nuget.org/packages/DotCommon.AspNetCore.Mvc) |![NuGet](https://img.shields.io/nuget/dt/DotCommon.AspNetCore.Mvc.svg)|

## 项目简介

DotCommon 是一个 C# 工具类库，为 .NET 应用提供核心抽象和实用工具。它采用**可插拔的提供者模式**，支持序列化、对象映射、缓存和调度的可替换实现。

### 主要特性

- **高性能反射** - 使用 Expression Trees 编译为 IL，实现快速的对象与字典转换
- **可插拔架构** - 基于接口的提供者模式，支持依赖注入
- **JSON 序列化** - 基于 System.Text.Json，支持自定义转换器和日期时间规范化
- **对象映射** - 三级映射系统（类型特定映射器 → 通用映射器 → 自动映射提供者）
- **多级缓存** - 支持分布式缓存和混合缓存（L1 内存 + L2 分布式）
- **后台调度** - 基于 TPL 的任务调度服务，支持限制并发级别
- **时区支持** - 多时区抽象，支持时区转换和时间提供者
- **加密工具** - 支持 MD5、RSA、AES、DES、SM2/SM3/SM4 等加密算法

### 目标框架

- Core library: `netstandard2.0`, `netstandard2.1`
- Extension packages: `net10.0`

## 安装

```bash
dotnet add package DotCommon
```

## 快速开始

### 基础配置

```csharp
services
    .AddDotCommon()                           // 核心服务
    .AddDotCommonSystemTextJsonSerializer()   // System.Text.Json 序列化
    .AddDotCommonObjectMapper()               // 对象映射
    .AddDotCommonSchedule();                  // 后台调度
```

### 使用 AutoMapper

```csharp
// 需要进行自动映射的程序集
var assemblies = new List<Assembly> { typeof(Program).Assembly };

services
    .AddDotCommon()
    .AddDotCommonAutoMapper()
    .AddAssemblyAutoMaps(assemblies.ToArray())
    .BuildAutoMapper();
```

### 使用缓存

```csharp
services
    .AddDotCommon()
    .AddDotCommonCaching();

// 使用缓存
var personCache = provider.GetRequiredService<IDistributedCache<PersonCacheItem>>();
var cacheItem = await personCache.GetAsync("key1");
await personCache.SetAsync("key2", cacheItem);
```

## 扩展包

### DotCommon.AutoMapper
AutoMapper 集成扩展包，提供自动对象映射功能。

```bash
dotnet add package DotCommon.AutoMapper
```

### DotCommon.Caching
多级缓存抽象，支持分布式缓存和混合缓存（L1 内存 + L2 分布式）。

```bash
dotnet add package DotCommon.Caching
```

### DotCommon.Crypto
加密扩展包，提供额外的加密算法支持。

```bash
dotnet add package DotCommon.Crypto
```

### DotCommon.AspNetCore.Mvc
ASP.NET Core MVC 集成扩展。

```bash
dotnet add package DotCommon.AspNetCore.Mvc
```

## 构建和测试

### 构建项目

```bash
# 克隆仓库
git clone https://github.com/cocosip/DotCommon.git
cd DotCommon

# 恢复依赖
dotnet restore DotCommon.sln

# 构建（Release 模式）
dotnet build DotCommon.sln -c Release
```

### 运行测试

```bash
dotnet test DotCommon.sln -c Release --verbosity normal
```

### 打包

```bash
dotnet pack DotCommon.sln -c Release --include-symbols -o dest/
```

## 核心架构

### 对象映射架构

对象映射系统有**三个优先级层次**（按顺序检查）：

1. **类型特定映射器**: `IObjectMapper<TSource, TDestination>` - 最高优先级，用于自定义映射逻辑
2. **通用映射器**: `IObjectMapper` - 处理集合并委托给自动映射器
3. **自动映射提供者**: `IAutoObjectMappingProvider` - 可插拔（通过 `DotCommon.AutoMapper` 包集成 AutoMapper）

### 高性能反射

**ExpressionMapper** 使用 Expression Trees 编译为 IL，实现快速的对象与字典转换。使用并发字典缓存已编译的转换器。在处理对象到字典的场景时，这是首选的高性能方法。

### 依赖注入设置

服务注册遵循流式模式：

```csharp
services
    .AddDotCommon()                    // 核心服务
    .AddDotCommonSchedule()            // 调度
    .AddDotCommonObjectMapper()        // 对象映射
    .AddDotCommonSystemTextJsonSerializer(); // JSON
```

## 许可证

本项目采用 MIT 许可证 - 详见 [LICENSE](LICENSE) 文件

## 贡献

欢迎提交 Issue 和 Pull Request！