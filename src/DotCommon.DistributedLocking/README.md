# DotCommon.DistributedLocking

分布式锁库，支持进程内锁和多种分布式后端（Redis、SQL Server、PostgreSQL、MySQL 等）。

## 功能特性

- 简洁的 API 设计
- 支持进程内锁（单实例应用）
- 支持多种分布式后端（基于 Medallion.Threading）
- 异步优先
- 支持超时和取消令牌
- 完整的依赖注入支持

## 快速开始

### 1. 进程内锁（单实例应用/开发测试）

```csharp
// Startup.cs 或 Program.cs
services.AddDotCommonDistributedLocking(options =>
{
    options.KeyPrefix = "MyApp:";
});

// 使用
public class OrderService
{
    private readonly IDistributedLock _lock;

    public OrderService(IDistributedLock distributedLock)
    {
        _lock = distributedLock;
    }

    public async Task ProcessOrderAsync(string orderId)
    {
        await using (var handle = await _lock.TryAcquireAsync(
            $"Order:{orderId}",
            TimeSpan.FromSeconds(5)))
        {
            if (handle != null)
            {
                // 锁获取成功，执行业务逻辑
                await DoWorkAsync();
            }
            else
            {
                // 锁获取失败（超时）
                throw new Exception("Could not acquire lock");
            }
        } // 自动释放锁
    }
}
```

### 2. Redis 分布式锁

**安装 NuGet 包：**
```bash
dotnet add package DistributedLock.Redis
dotnet add package StackExchange.Redis
```

**配置：**
```csharp
using Medallion.Threading.Redis;
using StackExchange.Redis;

// Startup.cs 或 Program.cs
var redis = await ConnectionMultiplexer.ConnectAsync("localhost:6379");
var lockProvider = new RedisDistributedSynchronizationProvider(redis.GetDatabase());

services.AddDotCommonDistributedLocking(lockProvider, options =>
{
    options.KeyPrefix = "MyApp:";
});
```

### 3. SQL Server 分布式锁

**安装 NuGet 包：**
```bash
dotnet add package DistributedLock.SqlServer
```

**配置：**
```csharp
using Medallion.Threading.SqlServer;

// Startup.cs 或 Program.cs
var connectionString = configuration.GetConnectionString("Default");
var lockProvider = new SqlDistributedSynchronizationProvider(connectionString);

services.AddDotCommonDistributedLocking(lockProvider, options =>
{
    options.KeyPrefix = "MyApp:";
});
```

### 4. PostgreSQL 分布式锁

**安装 NuGet 包：**
```bash
dotnet add package DistributedLock.Postgres
```

**配置：**
```csharp
using Medallion.Threading.Postgres;

var connectionString = configuration.GetConnectionString("Default");
var lockProvider = new PostgresDistributedSynchronizationProvider(connectionString);

services.AddDotCommonDistributedLocking(lockProvider, options =>
{
    options.KeyPrefix = "MyApp:";
});
```

### 5. MySQL 分布式锁

**安装 NuGet 包：**
```bash
dotnet add package DistributedLock.MySql
```

**配置：**
```csharp
using Medallion.Threading.MySql;

var connectionString = configuration.GetConnectionString("Default");
var lockProvider = new MySqlDistributedSynchronizationProvider(connectionString);

services.AddDotCommonDistributedLocking(lockProvider, options =>
{
    options.KeyPrefix = "MyApp:";
});
```

## 高级用法

### 使用工厂函数

```csharp
services.AddDotCommonDistributedLocking(
    sp =>
    {
        var config = sp.GetRequiredService<IConfiguration>();
        var connectionString = config["Redis:ConnectionString"];
        var redis = ConnectionMultiplexer.Connect(connectionString);
        return new RedisDistributedSynchronizationProvider(redis.GetDatabase());
    },
    options =>
    {
        options.KeyPrefix = "MyApp:";
    }
);
```

### 不使用超时（无限等待）

```csharp
// timeout 为 null 时，会无限等待直到获取锁
await using (var handle = await _lock.TryAcquireAsync("MyLock", timeout: null))
{
    if (handle != null)
    {
        // 执行业务逻辑
    }
}
```

### 支持取消令牌

```csharp
var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

await using (var handle = await _lock.TryAcquireAsync(
    "MyLock",
    TimeSpan.FromSeconds(5),
    cts.Token))
{
    if (handle != null)
    {
        // 执行业务逻辑
    }
}
```

## 配置选项

### DistributedLockOptions

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| KeyPrefix | string | "DistributedLock:" | 所有锁名称的前缀，用于命名空间隔离 |

## 支持的后端

| 后端 | NuGet 包 | 提供者类 |
|------|----------|---------|
| 进程内 | - | LocalDistributedLock |
| Redis | DistributedLock.Redis | RedisDistributedSynchronizationProvider |
| SQL Server | DistributedLock.SqlServer | SqlDistributedSynchronizationProvider |
| PostgreSQL | DistributedLock.Postgres | PostgresDistributedSynchronizationProvider |
| MySQL | DistributedLock.MySql | MySqlDistributedSynchronizationProvider |
| ZooKeeper | DistributedLock.ZooKeeper | ZooKeeperDistributedSynchronizationProvider |
| Oracle | DistributedLock.Oracle | OracleDistributedSynchronizationProvider |
| Azure | DistributedLock.Azure | AzureDistributedLock |

更多信息请参考 [Medallion.Threading](https://github.com/madelson/DistributedLock) 文档。

## 许可证

MIT License
