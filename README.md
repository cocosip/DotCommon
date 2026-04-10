# DotCommon

[![CI/CD Pipeline](https://github.com/cocosip/DotCommon/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/cocosip/DotCommon/actions/workflows/ci-cd.yml)
[![GitHub license](https://img.shields.io/github/license/cocosip/DotCommon)](LICENSE)

DotCommon 是一个面向 .NET 的通用基础类库，围绕序列化、对象映射、缓存、调度、时间、线程和常用工具能力提供统一抽象与默认实现。项目整体采用可插拔 Provider 模式，便于通过依赖注入替换具体实现。

## 特性概览

- 提供统一的基础设施抽象，包括 `IJsonSerializer`、`IObjectMapper`、`IObjectSerializer`、`IScheduleService` 等
- 基于 `System.Text.Json` 提供默认 JSON 实现，并内置多种转换器
- 提供三级对象映射链路，支持类型专用映射器、通用映射器和 AutoMapper 扩展
- 提供分布式缓存与混合缓存抽象，支持内存缓存和 Redis 接入
- 提供分布式锁扩展，支持本地锁以及基于 `Medallion.Threading` 的外部提供者
- 提供高性能反射工具，适合对象与字典之间的转换场景
- 提供时钟、时区、线程上下文、加密和常用工具类能力

## 包列表

| 包名 | 说明 |
| --- | --- |
| `DotCommon` | 核心库，包含序列化、对象映射、调度、时间、线程、反射与通用工具 |
| `DotCommon.AutoMapper` | AutoMapper 集成，用于接入自动对象映射 |
| `DotCommon.Caching` | 泛型分布式缓存与混合缓存抽象 |
| `DotCommon.Caching.StackExchangeRedis` | 基于 StackExchange.Redis 的缓存实现接入 |
| `DotCommon.Crypto` | 加密扩展，包含 BouncyCastle 相关能力 |
| `DotCommon.DistributedLocking` | 分布式锁抽象与注册扩展 |
| `DotCommon.AspNetCore.Mvc` | ASP.NET Core MVC 相关扩展 |

## 目标框架

- `DotCommon`: `netstandard2.0`, `netstandard2.1`
- `DotCommon.Caching`: `netstandard2.0`, `netstandard2.1`
- `DotCommon.Caching.StackExchangeRedis`: `netstandard2.0`, `netstandard2.1`
- `DotCommon.Crypto`: `netstandard2.0`, `netstandard2.1`
- `DotCommon.DistributedLocking`: `netstandard2.0`, `netstandard2.1`
- `DotCommon.AutoMapper`: `$(NetCoreFx)`
- `DotCommon.AspNetCore.Mvc`: `$(NetCoreFx)`

## 安装

安装核心包：

```bash
dotnet add package DotCommon
```

按需安装扩展包：

```bash
dotnet add package DotCommon.AutoMapper
dotnet add package DotCommon.Caching
dotnet add package DotCommon.Caching.StackExchangeRedis
dotnet add package DotCommon.Crypto
dotnet add package DotCommon.DistributedLocking
dotnet add package DotCommon.AspNetCore.Mvc
```

## 快速开始

### 基础注册

`AddDotCommon()` 会统一注册调度、对象序列化、对象映射、线程、时间和默认 JSON 服务。

```csharp
services.AddDotCommon();
```

如果只需要部分能力，也可以分别注册：

```csharp
services
    .AddDotCommonSchedule()
    .AddDotCommonSerialization()
    .AddDotCommonObjectMapper()
    .AddDotCommonThreading()
    .AddDotCommonTiming()
    .AddDotCommonSystemTextJson();
```

### 对象映射

核心对象映射默认使用 `DefaultObjectMapper`，映射优先级如下：

1. `IObjectMapper<TSource, TDestination>` 类型专用映射器
2. `IObjectMapper` 通用映射器
3. `IAutoObjectMappingProvider` 自动映射提供者

启用 AutoMapper 扩展后，默认自动映射提供者会被替换为 AutoMapper 实现。

```csharp
services
    .AddDotCommon()
    .AddDotCommonAutoMapper();
```

### 缓存

`DotCommon.Caching` 提供：

- `IDistributedCache<TCacheItem>`
- `IDistributedCache<TCacheItem, TCacheKey>`
- `IHybridCache<TCacheItem>`

默认注册包括内存缓存、分布式内存缓存、缓存键标准化和 JSON 序列化器。

```csharp
services.AddDotCommonCaching();
```

如果需要 Redis：

```csharp
services.AddDotCommonCachingWithRedis(options =>
{
    options.Configuration = "localhost:6379";
});
```

### 分布式锁

`DotCommon.DistributedLocking` 支持两类接入方式：

- 单实例场景下的本地锁
- 基于 `IDistributedLockProvider` 的外部锁提供者

```csharp
services.AddDotCommonDistributedLocking();
```

或传入外部分布式锁提供者：

```csharp
services.AddDotCommonDistributedLocking(lockProvider);
```

## 核心设计

### Provider 模式

项目中大量能力通过接口抽象和默认实现解耦，便于替换具体实现。

- `IJsonSerializer` -> `DotCommonSystemTextJsonSerializer`
- `IObjectSerializer` -> `DefaultObjectSerializer`
- `IObjectMapper` -> `DefaultObjectMapper`
- `IAutoObjectMappingProvider` -> `NotImplementedAutoObjectMappingProvider`

### JSON 序列化

默认 JSON 实现位于 `DotCommon.Json.SystemTextJson` 命名空间，内置：

- 日期时间转换器
- 字符串到枚举转换器
- 字符串到布尔值转换器
- `object` 类型推断转换器

同时集成了基于 `TypeInfoResolver` 的扩展点，支持日期时间规范化处理。

### 高性能反射

`ExpressionMapper` 使用表达式树构建和缓存转换逻辑，适合对性能敏感的对象转换场景。

### 调度与时间

- `IScheduleService` 提供后台任务调度能力
- `IClock`、`ICurrentTimezoneProvider`、`ITimezoneProvider` 提供时间与时区抽象
- `ICancellationTokenProvider`、Ambient Scope 相关类型提供线程上下文能力

## 项目结构

```text
src/
  DotCommon/
  DotCommon.AutoMapper/
  DotCommon.AspNetCore.Mvc/
  DotCommon.Caching/
  DotCommon.Caching.StackExchangeRedis/
  DotCommon.Crypto/
  DotCommon.DistributedLocking/

test/
  DotCommon.Test/
  DotCommon.AspNetCore.Mvc.Test/
```

## 构建与测试

构建整个解决方案：

```bash
dotnet build DotCommon.sln
```

Release 构建：

```bash
dotnet build DotCommon.sln -c Release
```

运行全部测试：

```bash
dotnet test DotCommon.sln
```

也可以使用仓库内脚本：

```bash
./build/build-all-release.ps1
./build/test-all.ps1
./build/pack-all.ps1
```

## 依赖管理

项目通过 `Directory.Packages.props` 集中管理依赖版本，不在单个 `.csproj` 中重复指定版本。

主要依赖包括：

- `Microsoft.Extensions.*`
- `System.Text.Json`
- `AutoMapper`
- `TimeZoneConverter`
- `BouncyCastle.Cryptography`
- `DistributedLock.Core`
- `xUnit`

## 许可证

本项目采用 MIT 许可证，详见 `LICENSE`。
