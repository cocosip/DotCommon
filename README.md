# DotCommon

[![996.icu](https://img.shields.io/badge/link-996.icu-red.svg)](https://996.icu) [![GitHub](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/cocosip/DotCommon/blob/master/LICENSE) ![GitHub last commit](https://img.shields.io/github/last-commit/cocosip/DotCommon.svg) ![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/cocosip/DotCommon.svg)

[![CI/CD Pipeline](https://github.com/cocosip/DotCommon/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/cocosip/DotCommon/actions/workflows/ci-cd.yml)

| Package  | Version | Downloads|
| -------- | ------- | -------- |
| `DotCommon` | [![NuGet](https://img.shields.io/nuget/v/DotCommon.svg)](https://www.nuget.org/packages/DotCommon) |![NuGet](https://img.shields.io/nuget/dt/DotCommon.svg)|
| `DotCommon.AutoMapper` | [![NuGet](https://img.shields.io/nuget/v/DotCommon.AutoMapper.svg)](https://www.nuget.org/packages/DotCommon.AutoMapper) |![NuGet](https://img.shields.io/nuget/dt/DotCommon.AutoMapper.svg)|
| `DotCommon.Caching` | [![NuGet](https://img.shields.io/nuget/v/DotCommon.Caching.svg)](https://www.nuget.org/packages/DotCommon.Caching) |![NuGet](https://img.shields.io/nuget/dt/DotCommon.Caching.svg)|
| `DotCommon.Crypto` | [![NuGet](https://img.shields.io/nuget/v/DotCommon.Crypto.svg)](https://www.nuget.org/packages/DotCommon.Crypto) |![NuGet](https://img.shields.io/nuget/dt/DotCommon.Crypto.svg)|
| `DotCommon.DistributedLocking` | [![NuGet](https://img.shields.io/nuget/v/DotCommon.DistributedLocking.svg)](https://www.nuget.org/packages/DotCommon.DistributedLocking) |![NuGet](https://img.shields.io/nuget/dt/DotCommon.DistributedLocking.svg)|
| `DotCommon.AspNetCore.Mvc` | [![NuGet](https://img.shields.io/nuget/v/DotCommon.AspNetCore.Mvc.svg)](https://www.nuget.org/packages/DotCommon.AspNetCore.Mvc) |![NuGet](https://img.shields.io/nuget/dt/DotCommon.AspNetCore.Mvc.svg)|

## 项目简介

DotCommon 是一个 C# 工具类库，为 .NET 应用提供核心抽象和实用工具。它采用**可插拔的提供者模式**，支持序列化、对象映射、缓存和调度的可替换实现。

### 主要特性

- **高性能反射** - 使用 Expression Trees 编译为 IL，实现快速的对象与字典转换
- **可插拔架构** - 基于接口的提供者模式，支持依赖注入
- **JSON 序列化** - 基于 System.Text.Json，支持自定义转换器和日期时间规范化
- **对象映射** - 三级映射系统（类型特定映射器 → 通用映射器 → 自动映射提供者）
- **多级缓存** - 支持分布式缓存和混合缓存（L1 内存 + L2 分布式）
- **分布式锁** - 支持进程内锁和多种分布式后端（Redis、SQL Server、PostgreSQL、MySQL 等）
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

var personCache = provider.GetRequiredService<IDistributedCache<PersonCacheItem>>();
var cacheItem = await personCache.GetAsync("key1");
await personCache.SetAsync("key2", cacheItem);
```

### 使用分布式锁

```csharp
// 进程内锁（单实例应用）
services.AddDotCommonDistributedLocking(options =>
{
    options.KeyPrefix = "MyApp:";
});

// Redis 分布式锁
var redis = await ConnectionMultiplexer.ConnectAsync("localhost:6379");
var lockProvider = new RedisDistributedSynchronizationProvider(redis.GetDatabase());
services.AddDotCommonDistributedLocking(lockProvider);

// 使用
await using (var handle = await _lock.TryAcquireAsync("MyLock", TimeSpan.FromSeconds(5)))
{
    if (handle != null)
    {
        // 执行业务逻辑
    }
}
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

加密扩展包，提供国密算法（SM2/SM3/SM4）和 RSA 加密服务。

```bash
dotnet add package DotCommon.Crypto
```

**功能特性：**
- **SM2** - 国密非对称加密算法
- **SM3** - 国密哈希算法
- **SM4** - 国密对称加密算法
- **RSA** - RSA 加密服务扩展

### DotCommon.DistributedLocking

分布式锁扩展包，基于 Medallion.Threading，支持多种后端（Redis、SQL Server、PostgreSQL、MySQL 等）。

```bash
dotnet add package DotCommon.DistributedLocking
```

### DotCommon.AspNetCore.Mvc

ASP.NET Core MVC 集成扩展，提供 CORS 配置、请求格式化器、HTTP 扩展等功能。

```bash
dotnet add package DotCommon.AspNetCore.Mvc
```

## 项目结构

```
src/
├── DotCommon/                           # 核心库 (netstandard2.0, netstandard2.1)
│   ├── Collections/                     # 集合工具
│   ├── DependencyInjection/             # 依赖注入扩展
│   ├── Encrypt/                         # 加密工具 (MD5, RSA, AES, DES, TripleDES)
│   ├── IO/                              # IO 操作
│   ├── Json/SystemTextJson/             # JSON 序列化及自定义转换器
│   ├── ObjectMapping/                   # 对象映射抽象
│   ├── Reflecting/                      # 高性能反射 (ExpressionMapper, EmitMapper)
│   ├── Scheduling/                     # 后台任务调度
│   ├── Serialization/                  # 序列化抽象
│   ├── Threading/                      # 线程工具
│   ├── Timing/                          # 时钟和时区抽象
│   └── Utility/                         # 工具类 (转换、加密、文本等)
├── DotCommon.AutoMapper/                # AutoMapper 集成 (net10.0)
├── DotCommon.Caching/                   # 缓存抽象 (netstandard2.0, netstandard2.1)
├── DotCommon.Crypto/                    # 国密算法扩展 (netstandard2.0, netstandard2.1)
│   ├── SM2/                             # SM2 非对称加密
│   ├── SM3/                             # SM3 哈希算法
│   └── SM4/                             # SM4 对称加密
├── DotCommon.DistributedLocking/        # 分布式锁 (netstandard2.0, netstandard2.1)
└── DotCommon.AspNetCore.Mvc/            # ASP.NET Core MVC 集成 (net10.0)
```

## 核心架构

### 对象映射架构

对象映射系统有**三个优先级层次**（按顺序检查）：

1. **类型特定映射器**: `IObjectMapper<TSource, TDestination>` - 最高优先级，用于自定义映射逻辑
2. **通用映射器**: `IObjectMapper` - 处理集合并委托给自动映射器
3. **自动映射提供者**: `IAutoObjectMappingProvider` - 可插拔（通过 `DotCommon.AutoMapper` 包集成 AutoMapper）

### 高性能反射

**ExpressionMapper** 使用 Expression Trees 编译为 IL，实现快速的对象与字典转换。使用并发字典缓存已编译的转换器。在处理对象到字典的场景时，这是首选的高性能方法。

### 缓存架构

- **IDistributedCache\<T\>** - 泛型分布式缓存
- **IHybridCache\<T\>** - L1 (内存) + L2 (分布式) 混合缓存
- **DotCommonHybridCache** - 基于 JSON 序列化的实现
- **IDistributedCacheSerializer** - 可插拔序列化（默认: UTF8 JSON）
- **DistributedCacheKeyNormalizer** - 支持前缀的键转换

### 依赖注入设置

服务注册遵循流式模式：

```csharp
services
    .AddDotCommon()                    // 核心服务
    .AddDotCommonSchedule()            // 调度
    .AddDotCommonObjectMapper()        // 对象映射
    .AddDotCommonSystemTextJsonSerializer(); // JSON
```

扩展包注册方法：
```csharp
services.AddDotCommonAutoMapper()      // AutoMapper 集成
    .AddAssemblyAutoMaps(assemblies)   // 扫描映射特性
    .BuildAutoMapper();

services.AddDotCommonCaching();         // 缓存
services.AddDotCommonDistributedLocking(); // 分布式锁
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

## 依赖项

主要依赖项（版本通过 Directory.Packages.props 集中管理）：

| 包 | 版本 | 用途 |
|---|---|---|
| Microsoft.Extensions.* | 10.0.5+ | DI、Options、Configuration、Caching |
| System.Text.Json | 10.0.5+ | JSON 序列化 |
| AutoMapper | 16.1.1 | 对象映射 (DotCommon.AutoMapper) |
| TimeZoneConverter | 7.2.0 | 时区处理 |
| BouncyCastle.Cryptography | 2.6.2 | SM2/SM3/SM4 加密 |
| DistributedLock.Core | 1.0.9 | 分布式锁 |
| xUnit | 2.9.3 | 测试框架 |

## 许可证

本项目采用 MIT 许可证 - 详见 [LICENSE](LICENSE) 文件

## 贡献

欢迎提交 Issue 和 Pull Request！