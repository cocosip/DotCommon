# DotCommon.Caching

[![NuGet](https://img.shields.io/nuget/v/DotCommon.Caching.svg)](https://www.nuget.org/packages/DotCommon.Caching) ![NuGet](https://img.shields.io/nuget/dt/DotCommon.Caching.svg)

DotCommon.Caching 是 DotCommon 的缓存模块，提供了分布式缓存和混合缓存（L1 + L2）的抽象与实现。基于 Microsoft.Extensions.Caching 基础设施构建，支持多种缓存后端。

## 特性

- **分布式缓存抽象** - 泛型分布式缓存接口 `IDistributedCache<T>`
- **混合缓存支持** - L1（内存）+ L2（分布式）两级缓存架构
- **多键批量操作** - 支持 GetMany/SetMany 等批量操作
- **可插拔序列化** - 支持自定义缓存序列化器
- **缓存键规范化** - 自动添加缓存名称和前缀
- **错误处理策略** - 可配置隐藏或抛出缓存异常
- **工作单元支持** - 支持与工作单元集成

## 安装

```bash
dotnet add package DotCommon.Caching
```

## 快速开始

### 1. 注册服务

```csharp
using Microsoft.Extensions.DependencyInjection;
using DotCommon.Caching;

var services = new ServiceCollection();

services
    .AddDotCommon()              // 注册 DotCommon 核心服务
    .AddDotCommonCaching();      // 注册缓存服务

var serviceProvider = services.BuildServiceProvider();
```

### 2. 定义缓存项类型

```csharp
using DotCommon.Caching;

// 使用 CacheNameAttribute 指定缓存名称（可选）
[CacheName("User")]
public class UserCacheItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
```

### 3. 使用分布式缓存

```csharp
using DotCommon.Caching;

public class UserService
{
    private readonly IDistributedCache<UserCacheItem> _userCache;

    public UserService(IDistributedCache<UserCacheItem> userCache)
    {
        _userCache = userCache;
    }

    public async Task<UserCacheItem?> GetUserAsync(int userId)
    {
        var cacheKey = $"User:{userId}";

        // GetOrAddAsync: 如果缓存不存在，则调用 factory 获取数据并缓存
        return await _userCache.GetOrAddAsync(
            cacheKey,
            async () => await LoadUserFromDatabaseAsync(userId),
            () => new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            }
        );
    }

    public async Task UpdateUserAsync(int userId, UserCacheItem user)
    {
        await UpdateUserInDatabaseAsync(user);

        // 更新缓存
        await _userCache.SetAsync(
            $"User:{userId}",
            user,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            }
        );
    }

    public async Task RemoveUserAsync(int userId)
    {
        await DeleteUserFromDatabaseAsync(userId);

        // 删除缓存
        await _userCache.RemoveAsync($"User:{userId}");
    }

    private async Task<UserCacheItem> LoadUserFromDatabaseAsync(int userId)
    {
        // 模拟数据库查询
        return new UserCacheItem { Id = userId, Name = "Test User", Email = "test@example.com" };
    }

    private Task UpdateUserInDatabaseAsync(UserCacheItem user) => Task.CompletedTask;
    private Task DeleteUserFromDatabaseAsync(int userId) => Task.CompletedTask;
}
```

## 分布式缓存

### IDistributedCache<TCacheItem> 接口

分布式缓存接口提供完整的缓存操作：

```csharp
public interface IDistributedCache<TCacheItem> where TCacheItem : class
{
    // 获取缓存
    TCacheItem? Get(string key, bool? hideErrors = null, bool considerUow = false);
    Task<TCacheItem?> GetAsync(string key, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default);

    // 获取或添加缓存
    TCacheItem? GetOrAdd(string key, Func<TCacheItem> factory, Func<DistributedCacheEntryOptions>? optionsFactory = null, ...);
    Task<TCacheItem?> GetOrAddAsync(string key, Func<Task<TCacheItem>> factory, Func<DistributedCacheEntryOptions>? optionsFactory = null, ...);

    // 设置缓存
    void Set(string key, TCacheItem value, DistributedCacheEntryOptions? options = null, ...);
    Task SetAsync(string key, TCacheItem value, DistributedCacheEntryOptions? options = null, ...);

    // 删除缓存
    void Remove(string key, bool? hideErrors = null, bool considerUow = false);
    Task RemoveAsync(string key, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default);

    // 批量操作
    KeyValuePair<string, TCacheItem?>[] GetMany(IEnumerable<string> keys, ...);
    Task<KeyValuePair<string, TCacheItem?>[]> GetManyAsync(IEnumerable<string> keys, ...);
    void SetMany(IEnumerable<KeyValuePair<string, TCacheItem>> items, ...);
    Task SetManyAsync(IEnumerable<KeyValuePair<string, TCacheItem>> items, ...);
    void RemoveMany(IEnumerable<string> keys, ...);
    Task RemoveManyAsync(IEnumerable<string> keys, ...);

    // 刷新缓存（重置滑动过期时间）
    void Refresh(string key, bool? hideErrors = null);
    Task RefreshAsync(string key, bool? hideErrors = null, CancellationToken token = default);
}
```

### 自定义缓存键类型

默认情况下，缓存键使用 `string` 类型。你可以使用自定义键类型：

```csharp
public class UserCacheKey
{
    public int UserId { get; set; }
    public string Region { get; set; }

    public override string ToString() => $"{Region}:{UserId}";
}

// 使用自定义键类型
public class UserService
{
    private readonly IDistributedCache<UserCacheItem, UserCacheKey> _userCache;

    public async Task<UserCacheItem?> GetUserAsync(int userId, string region)
    {
        return await _userCache.GetOrAddAsync(
            new UserCacheKey { UserId = userId, Region = region },
            async () => await LoadUserAsync(userId, region)
        );
    }
}
```

### 缓存过期配置

```csharp
// 绝对过期时间
await _cache.SetAsync(key, value, new DistributedCacheEntryOptions
{
    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
});

// 滑动过期时间（在指定时间内访问会重置过期时间）
await _cache.SetAsync(key, value, new DistributedCacheEntryOptions
{
    SlidingExpiration = TimeSpan.FromMinutes(30)
});

// 绝对过期时间（指定具体时间点）
await _cache.SetAsync(key, value, new DistributedCacheEntryOptions
{
    AbsoluteExpiration = DateTimeOffset.Now.AddDays(1)
});
```

### 全局缓存配置

```csharp
services.Configure<DotCommonDistributedCacheOptions>(options =>
{
    // 是否隐藏缓存异常（默认 true）
    options.HideErrors = true;

    // 缓存键前缀（用于多租户或多应用隔离）
    options.KeyPrefix = "MyApp:";

    // 全局默认缓存过期配置
    options.GlobalCacheEntryOptions = new DistributedCacheEntryOptions
    {
        SlidingExpiration = TimeSpan.FromMinutes(20)
    };

    // 为特定缓存类型配置过期时间
    options.ConfigureCache<UserCacheItem>(new DistributedCacheEntryOptions
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
    });
});
```

### 使用 Redis 作为分布式缓存后端

```csharp
using Microsoft.Extensions.Caching.StackExchangeRedis;

services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "MyApp:";
});

services
    .AddDotCommon()
    .AddDotCommonCaching();
```

### 使用 SQL Server 作为分布式缓存后端

```csharp
using Microsoft.Extensions.Caching.SqlServer;

services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString = Configuration.GetConnectionString("Default");
    options.SchemaName = "dbo";
    options.TableName = "CacheEntries";
});

services
    .AddDotCommon()
    .AddDotCommonCaching();
```

## 混合缓存（L1 + L2）

混合缓存是一种两级缓存架构，结合了内存缓存（L1）和分布式缓存（L2）的优点：

- **L1（内存缓存）**：极快的读取速度，但无法跨进程共享
- **L2（分布式缓存）**：可跨进程共享，但网络延迟较高

### 工作原理

```
┌─────────────────────────────────────────────────────────────────┐
│                        应用程序                                   │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                     IHybridCache<T>                              │
│                                                                  │
│  GetOrCreateAsync(key, factory):                                │
│    1. 检查 L1 缓存（内存）                                        │
│       ├── 命中 → 直接返回                                        │
│       └── 未命中 → 继续步骤 2                                    │
│    2. 检查 L2 缓存（分布式）                                      │
│       ├── 命中 → 回填 L1 → 返回                                  │
│       └── 未命中 → 继续步骤 3                                    │
│    3. 调用 factory 获取数据                                       │
│    4. 写入 L2 → 写入 L1 → 返回                                   │
└─────────────────────────────────────────────────────────────────┘
         │                                      │
         ▼                                      ▼
┌──────────────────────┐            ┌──────────────────────┐
│   L1: 内存缓存        │            │   L2: 分布式缓存      │
│   (IMemoryCache)     │            │   (IDistributedCache) │
│                      │            │                      │
│   • 极速读取          │            │   • 跨进程共享        │
│   • 进程内有效        │            │   • 持久化存储        │
│   • 容量有限          │            │   • 容量大           │
└──────────────────────┘            └──────────────────────┘
```

### 使用混合缓存

```csharp
using DotCommon.Caching.Hybrid;

public class ProductService
{
    private readonly IHybridCache<ProductCacheItem> _productCache;

    public ProductService(IHybridCache<ProductCacheItem> productCache)
    {
        _productCache = productCache;
    }

    public async Task<ProductCacheItem?> GetProductAsync(int productId)
    {
        // 混合缓存会自动处理 L1 和 L2
        return await _productCache.GetOrCreateAsync(
            $"Product:{productId}",
            async () => await LoadProductFromDatabaseAsync(productId),
            () => new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromHours(1),           // L1 过期时间
                LocalCacheExpiration = TimeSpan.FromMinutes(30) // L2 过期时间
            }
        );
    }

    public async Task UpdateProductAsync(int productId, ProductCacheItem product)
    {
        await UpdateProductInDatabaseAsync(product);

        // 更新缓存（会同时更新 L1 和 L2）
        await _productCache.SetAsync(
            $"Product:{productId}",
            product,
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromHours(1)
            }
        );
    }

    public async Task RemoveProductAsync(int productId)
    {
        await DeleteProductFromDatabaseAsync(productId);

        // 删除缓存（会同时删除 L1 和 L2）
        await _productCache.RemoveAsync($"Product:{productId}");
    }

    private Task<ProductCacheItem> LoadProductFromDatabaseAsync(int productId)
    {
        return Task.FromResult(new ProductCacheItem { Id = productId, Name = "Product" });
    }

    private Task UpdateProductInDatabaseAsync(ProductCacheItem product) => Task.CompletedTask;
    private Task DeleteProductFromDatabaseAsync(int productId) => Task.CompletedTask;
}

[CacheName("Product")]
public class ProductCacheItem
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

### 混合缓存配置

```csharp
services.Configure<DotCommonHybridCacheOptions>(options =>
{
    // 是否隐藏缓存异常（默认 true）
    options.HideErrors = true;

    // 缓存键前缀
    options.KeyPrefix = "MyApp:";

    // 全局默认配置
    options.GlobalHybridCacheEntryOptions = new HybridCacheEntryOptions
    {
        Expiration = TimeSpan.FromMinutes(20),
        LocalCacheExpiration = TimeSpan.FromMinutes(10)
    };

    // 为特定类型配置
    options.ConfigureCache<ProductCacheItem>(new HybridCacheEntryOptions
    {
        Expiration = TimeSpan.FromHours(1),
        LocalCacheExpiration = TimeSpan.FromMinutes(30)
    });
});
```

### IHybridCache<TCacheItem> 接口

```csharp
public interface IHybridCache<TCacheItem> where TCacheItem : class
{
    // 获取或创建缓存
    Task<TCacheItem?> GetOrCreateAsync(
        string key,
        Func<Task<TCacheItem>> factory,
        Func<HybridCacheEntryOptions>? optionsFactory = null,
        bool? hideErrors = null,
        bool considerUow = false,
        CancellationToken token = default);

    // 设置缓存
    Task SetAsync(
        string key,
        TCacheItem value,
        HybridCacheEntryOptions? options = null,
        bool? hideErrors = null,
        bool considerUow = false,
        CancellationToken token = default);

    // 删除缓存
    Task RemoveAsync(string key, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default);
    Task RemoveManyAsync(IEnumerable<string> keys, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default);
}
```

## 缓存键规范化

所有缓存键都会自动规范化，添加缓存名称和前缀：

```
原始键: "123"
规范化后: "c:User,k:MyApp:123"
```

### 自定义键规范化

```csharp
public class CustomCacheKeyNormalizer : IDistributedCacheKeyNormalizer
{
    public string NormalizeKey(DistributedCacheKeyNormalizeArgs args)
    {
        // 自定义键格式
        return $"myapp:{args.CacheName}:{args.Key}";
    }
}

// 注册自定义规范化器
services.AddTransient<IDistributedCacheKeyNormalizer, CustomCacheKeyNormalizer>();
```

## 序列化

### 默认序列化器

默认使用 JSON 序列化（基于 System.Text.Json）：

```csharp
// 分布式缓存使用 IJsonSerializer
services.AddTransient<IDistributedCacheSerializer, Utf8JsonDistributedCacheSerializer>();

// 混合缓存使用 IHybridCacheSerializerFactory
services.AddHybridCache()
    .AddSerializerFactory<DotCommonHybridCacheJsonSerializerFactory>();
```

### 自定义序列化器

```csharp
using DotCommon.Caching;

public class MessagePackDistributedCacheSerializer : IDistributedCacheSerializer
{
    public byte[] Serialize<T>(T obj)
    {
        // 使用 MessagePack 序列化
        return MessagePackSerializer.Serialize(obj);
    }

    public T Deserialize<T>(byte[] bytes)
    {
        // 使用 MessagePack 反序列化
        return MessagePackSerializer.Deserialize<T>(bytes);
    }
}

// 注册自定义序列化器
services.AddTransient<IDistributedCacheSerializer, MessagePackDistributedCacheSerializer>();
```

## 高级用法

### 批量操作

当缓存后端支持 `ICacheSupportsMultipleItems` 时，批量操作会更高效：

```csharp
public class BatchCacheService
{
    private readonly IDistributedCache<UserCacheItem> _cache;

    public async Task<Dictionary<int, UserCacheItem>> GetUsersAsync(IEnumerable<int> userIds)
    {
        var keys = userIds.Select(id => $"User:{id}").ToList();
        var results = await _cache.GetManyAsync(keys);

        return results
            .Where(x => x.Value != null)
            .ToDictionary(
                x => int.Parse(x.Key.Replace("User:", "")),
                x => x.Value!
            );
    }

    public async Task SetUsersAsync(IEnumerable<UserCacheItem> users)
    {
        var items = users.Select(u => new KeyValuePair<string, UserCacheItem>(
            $"User:{u.Id}",
            u
        ));

        await _cache.SetManyAsync(items, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        });
    }

    public async Task RemoveUsersAsync(IEnumerable<int> userIds)
    {
        var keys = userIds.Select(id => $"User:{id}");
        await _cache.RemoveManyAsync(keys);
    }
}
```

### 缓存名称属性

使用 `[CacheName]` 属性指定缓存名称，用于键规范化：

```csharp
[CacheName("Product")]
public class ProductCacheItem
{
    public int Id { get; set; }
    public string Name { get; set; }
}

// 缓存键示例: "c:Product,k:123"
```

如果不指定，默认使用类型的完整名称（去掉 "CacheItem" 后缀）：

```csharp
public class UserCacheItem { }
// 默认缓存名: "User" 或 类型的 FullName
```

### 错误处理

```csharp
// 默认情况下，缓存异常会被隐藏（hideErrors: true）
// 可以在单次调用中覆盖
var user = await _cache.GetAsync("key", hideErrors: false);

// 或者全局配置
services.Configure<DotCommonDistributedCacheOptions>(options =>
{
    options.HideErrors = false; // 抛出异常而不是隐藏
});
```

### 与工作单元集成

```csharp
public async Task<UserCacheItem> UpdateUserAsync(int userId, UserCacheItem user)
{
    using (var uow = _unitOfWorkManager.Begin())
    {
        await _userRepository.UpdateAsync(user);

        // considerUow: true 时，缓存在工作单元提交后才真正更新
        await _cache.SetAsync($"User:{userId}", user, considerUow: true);

        await uow.CompleteAsync();
    }

    return user;
}
```

## 架构设计

### 核心组件

| 组件 | 说明 |
|------|------|
| `IDistributedCache<T>` | 分布式缓存接口 |
| `DistributedCache<T>` | 分布式缓存实现 |
| `IHybridCache<T>` | 混合缓存接口 |
| `DotCommonHybridCache<T>` | 混合缓存实现 |
| `IDistributedCacheSerializer` | 缓存序列化器接口 |
| `Utf8JsonDistributedCacheSerializer` | JSON 序列化实现 |
| `IDistributedCacheKeyNormalizer` | 缓存键规范化接口 |
| `DistributedCacheKeyNormalizer` | 键规范化实现 |
| `ICacheSupportsMultipleItems` | 多项操作支持接口 |

### 类图

```
┌─────────────────────────────────────────────────────────────────┐
│                    IDistributedCache<TCacheItem>                │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                    IDistributedCache<TCacheItem, TCacheKey>     │
│  - Get/GetAsync                                                 │
│  - GetOrAdd/GetOrAddAsync                                       │
│  - Set/SetAsync                                                 │
│  - Remove/RemoveAsync                                           │
│  - GetMany/SetMany/RemoveMany                                   │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                    DistributedCache<TCacheItem, TCacheKey>      │
│                                                                 │
│  依赖:                                                          │
│  - IDistributedCache (Microsoft)                                │
│  - IDistributedCacheSerializer                                  │
│  - IDistributedCacheKeyNormalizer                               │
└─────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────┐
│                    IHybridCache<TCacheItem>                     │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                    IHybridCache<TCacheItem, TCacheKey>          │
│  - GetOrCreateAsync                                             │
│  - SetAsync                                                     │
│  - RemoveAsync/RemoveManyAsync                                  │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                    DotCommonHybridCache<TCacheItem, TCacheKey>  │
│                                                                 │
│  依赖:                                                          │
│  - HybridCache (Microsoft)                                      │
│  - IDistributedCacheKeyNormalizer                               │
│  - IHybridCacheSerializer<T>                                    │
└─────────────────────────────────────────────────────────────────┘
```

## 完整示例

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using DotCommon.Caching;
using DotCommon.Caching.Hybrid;

// 1. 定义缓存项
[CacheName("User")]
public class UserCacheItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

// 2. 配置服务
var services = new ServiceCollection();

// 配置 Redis 作为分布式缓存后端
services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "MyApp:";
});

// 注册 DotCommon 缓存服务
services
    .AddDotCommon()
    .AddDotCommonCaching();

// 配置缓存选项
services.Configure<DotCommonDistributedCacheOptions>(options =>
{
    options.KeyPrefix = "MyApp:";
    options.GlobalCacheEntryOptions = new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
    {
        SlidingExpiration = TimeSpan.FromMinutes(30)
    };
});

var serviceProvider = services.BuildServiceProvider();

// 3. 使用缓存
var userCache = serviceProvider.GetRequiredService<IDistributedCache<UserCacheItem>>();
var hybridCache = serviceProvider.GetRequiredService<IHybridCache<UserCacheItem>>();

// 分布式缓存示例
var user = await userCache.GetOrAddAsync(
    "User:1",
    async () => await Task.FromResult(new UserCacheItem { Id = 1, Name = "张三", Email = "zhangsan@example.com" }),
    () => new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
    }
);

Console.WriteLine($"分布式缓存: {user.Name}");

// 混合缓存示例
var hybridUser = await hybridCache.GetOrCreateAsync(
    "User:2",
    async () => await Task.FromResult(new UserCacheItem { Id = 2, Name = "李四", Email = "lisi@example.com" }),
    () => new Microsoft.Extensions.Caching.Hybrid.HybridCacheEntryOptions
    {
        Expiration = TimeSpan.FromHours(1),
        LocalCacheExpiration = TimeSpan.FromMinutes(30)
    }
);

Console.WriteLine($"混合缓存: {hybridUser.Name}");

// 清理缓存
await userCache.RemoveAsync("User:1");
await hybridCache.RemoveAsync("User:2");
```

## 最佳实践

### 1. 选择合适的缓存类型

| 场景 | 推荐缓存类型 |
|------|-------------|
| 配置数据、用户会话 | 分布式缓存 |
| 热点数据、频繁读取 | 混合缓存 |
| 临时计算结果 | 内存缓存 |
| 跨服务共享数据 | 分布式缓存 |

### 2. 合理设置过期时间

```csharp
// 热点数据 - 较长过期时间
options.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2);

// 用户会话 - 滑动过期
options.SlidingExpiration = TimeSpan.FromMinutes(30);

// 实时性要求高 - 较短过期时间
options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
```

### 3. 缓存穿透防护

```csharp
// 对于空值也进行缓存，避免频繁查询数据库
var user = await _cache.GetOrAddAsync(
    $"User:{userId}",
    async () =>
    {
        var dbUser = await _userRepository.FindAsync(userId);
        return dbUser != null
            ? new UserCacheItem { Id = dbUser.Id, Name = dbUser.Name }
            : null; // 空值也会被缓存
    },
    () => new DistributedCacheEntryOptions
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
    }
);
```

### 4. 缓存更新策略

```csharp
// 写入时更新缓存
public async Task UpdateUserAsync(User user)
{
    await _userRepository.UpdateAsync(user);

    // 立即更新缓存
    await _cache.SetAsync(
        $"User:{user.Id}",
        _mapper.Map<UserCacheItem>(user)
    );
}

// 或使用删除策略（下次读取时重新加载）
public async Task UpdateUserAsync(User user)
{
    await _userRepository.UpdateAsync(user);

    // 删除缓存，下次读取时重新加载
    await _cache.RemoveAsync($"User:{user.Id}");
}
```

### 5. 批量操作优化

```csharp
// 推荐：批量操作
var keys = userIds.Select(id => $"User:{id}");
var users = await _cache.GetManyAsync(keys);

// 不推荐：循环单次操作
foreach (var userId in userIds)
{
    var user = await _cache.GetAsync($"User:{userId}");
}
```

## 依赖项

- Microsoft.Extensions.Caching.Memory
- Microsoft.Extensions.Caching.Hybrid
- DotCommon

## 许可证

本项目采用 MIT 许可证

## 相关链接

- [DotCommon 主仓库](https://github.com/cocosip/DotCommon)
- [NuGet 包](https://www.nuget.org/packages/DotCommon.Caching)
- [Microsoft.Extensions.Caching 文档](https://learn.microsoft.com/en-us/dotnet/core/extensions/caching)