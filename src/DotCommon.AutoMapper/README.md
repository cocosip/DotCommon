# DotCommon.AutoMapper

[![NuGet](https://img.shields.io/nuget/v/DotCommon.AutoMapper.svg)](https://www.nuget.org/packages/DotCommon.AutoMapper) ![NuGet](https://img.shields.io/nuget/dt/DotCommon.AutoMapper.svg)

DotCommon.AutoMapper 是 DotCommon 的 AutoMapper 集成扩展包，提供基于 AutoMapper 的自动对象映射功能。它将 AutoMapper 无缝集成到 DotCommon 的对象映射系统中，使得对象映射更加简单和高效。

## 特性

- **AutoMapper 集成** - 将 AutoMapper 集成为 DotCommon 的默认对象映射提供者
- **自动扫描映射配置** - 支持从程序集自动扫描并注册 AutoMapper Profile
- **配置验证** - 支持在启动时验证映射配置的正确性
- **依赖注入支持** - 完全基于 Microsoft.Extensions.DependencyInjection
- **扩展方法** - 提供便捷的对象映射扩展方法
- **接口驱动映射** - 支持 `IMapTo<T>` 和 `IMapFrom<T>` 接口

## 安装

```bash
dotnet add package DotCommon.AutoMapper
```

## 快速开始

### 1. 注册服务

```csharp
using Microsoft.Extensions.DependencyInjection;
using DotCommon.AutoMapper;

var services = new ServiceCollection();

services
    .AddDotCommon()
    .AddDotCommonAutoMapper()
    .Configure<DotCommonAutoMapperOptions>(options =>
    {
        // 从当前程序集扫描并添加所有 AutoMapper Profile
        options.AddMaps<Program>();
    });

var serviceProvider = services.BuildServiceProvider();
```

### 2. 定义映射配置

使用 AutoMapper 的 `Profile` 定义映射规则：

```csharp
using AutoMapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // 简单映射（属性名相同时自动映射）
        CreateMap<UserDto, User>();
        CreateMap<User, UserDto>();

        // 自定义映射规则
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.CustomerName,
                       opt => opt.MapFrom(src => src.Customer.Name))
            .ForMember(dest => dest.TotalAmount,
                       opt => opt.MapFrom(src => src.Items.Sum(i => i.Price * i.Quantity)));
    }
}
```

### 3. 使用对象映射

```csharp
using DotCommon.ObjectMapping;

// 获取对象映射器
var objectMapper = serviceProvider.GetRequiredService<IObjectMapper>();

// 映射单个对象
var user = new User { Id = 1, Name = "张三", Email = "zhangsan@example.com" };
var userDto = objectMapper.Map<User, UserDto>(user);

// 映射列表
var users = GetUsers(); // List<User>
var userDtos = objectMapper.Map<List<User>, List<UserDto>>(users);
```

## 核心功能

### 1. 自动扫描程序集

使用 `AddMaps<T>()` 方法自动扫描指定类型所在程序集中的所有 `Profile`：

```csharp
services.Configure<DotCommonAutoMapperOptions>(options =>
{
    // 扫描包含 Startup 类的程序集
    options.AddMaps<Startup>();

    // 也可以扫描多个程序集
    options.AddMaps<Program>();
    options.AddMaps<ApplicationService>();
});
```

### 2. 添加单个 Profile

如果只需要添加特定的 Profile：

```csharp
services.Configure<DotCommonAutoMapperOptions>(options =>
{
    options.AddProfile<UserProfile>();
    options.AddProfile<OrderProfile>();
});
```

### 3. 配置验证

启用映射配置验证可以在应用启动时检测配置错误：

```csharp
services.Configure<DotCommonAutoMapperOptions>(options =>
{
    // 添加映射并启用验证
    options.AddMaps<Program>(validate: true);

    // 或者为单个 Profile 启用验证
    options.AddProfile<UserProfile>(validate: true);

    // 手动设置验证
    options.ValidateProfile<OrderProfile>(validate: true);
});
```

### 4. 自定义映射配置

使用配置器（Configurator）添加自定义配置：

```csharp
services.Configure<DotCommonAutoMapperOptions>(options =>
{
    options.Configurators.Add(context =>
    {
        var config = context.MapperConfiguration;

        // 添加全局配置
        config.CreateMap<DateTime, string>()
            .ConvertUsing(dt => dt.ToString("yyyy-MM-dd HH:mm:ss"));

        // 禁用空集合映射
        config.AllowNullCollections = true;

        // 添加值转换器
        config.CreateMap<decimal, string>()
            .ConvertUsing(d => d.ToString("N2"));
    });
});
```

## 高级用法

### 1. 使用 AutoMapper 的属性

AutoMapper 支持使用 `[AutoMap]` 属性简化配置：

```csharp
using AutoMapper;

// 自动创建 UserDto -> User 和 User -> UserDto 的映射
[AutoMap(typeof(User))]
public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
```

### 2. 双向映射

创建双向映射（两个方向都可以映射）：

```csharp
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ReverseMap 创建反向映射
        CreateMap<Source, Destination>()
            .ForMember(d => d.FullName, opt => opt.MapFrom(s => $"{s.FirstName} {s.LastName}"))
            .ReverseMap()
            .ForPath(s => s.FirstName, opt => opt.MapFrom(d => d.FullName.Split(' ')[0]))
            .ForPath(s => s.LastName, opt => opt.MapFrom(d => d.FullName.Split(' ')[1]));
    }
}
```

### 3. 条件映射

根据条件决定是否映射某些属性：

```csharp
CreateMap<User, UserDto>()
    .ForMember(dest => dest.Email, opt =>
        opt.Condition(src => !string.IsNullOrEmpty(src.Email)))
    .ForMember(dest => dest.PhoneNumber, opt =>
        opt.PreCondition(src => src.IsActive));
```

### 4. 嵌套对象映射

自动映射嵌套对象：

```csharp
public class Order
{
    public int Id { get; set; }
    public Customer Customer { get; set; }
    public List<OrderItem> Items { get; set; }
}

public class OrderDto
{
    public int Id { get; set; }
    public CustomerDto Customer { get; set; }
    public List<OrderItemDto> Items { get; set; }
}

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Order, OrderDto>();
        CreateMap<Customer, CustomerDto>();
        CreateMap<OrderItem, OrderItemDto>();
    }
}
```

### 5. 值解析器

使用值解析器处理复杂的映射逻辑：

```csharp
public class FullNameResolver : IValueResolver<User, UserDto, string>
{
    public string Resolve(User source, UserDto destination, string destMember, ResolutionContext context)
    {
        return $"{source.FirstName} {source.LastName}".Trim();
    }
}

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom<FullNameResolver>());
    }
}
```

### 6. 映射前后处理

在映射前后执行自定义逻辑：

```csharp
CreateMap<User, UserDto>()
    .BeforeMap((src, dest) =>
    {
        // 映射前执行
        Console.WriteLine($"Mapping user {src.Id}");
    })
    .AfterMap((src, dest) =>
    {
        // 映射后执行
        dest.MappedAt = DateTime.UtcNow;
    });
```

### 7. 投影查询（Queryable Extensions）

使用 `ProjectTo` 将 IQueryable 直接投影为 DTO：

```csharp
using AutoMapper.QueryableExtensions;

// 获取 IMapper 实例
var mapper = objectMapper.GetMapper();
var config = mapper.ConfigurationProvider;

// 直接在数据库查询中投影
var userDtos = dbContext.Users
    .Where(u => u.IsActive)
    .ProjectTo<UserDto>(config)
    .ToList();
```

## 接口驱动映射

DotCommon 支持通过实现接口来定义映射关系。

### IMapTo<TDestination>

源对象实现此接口，定义如何映射到目标对象：

```csharp
public class User : IMapTo<UserDto>
{
    public int Id { get; set; }
    public string Name { get; set; }

    public UserDto MapTo()
    {
        return new UserDto
        {
            Id = this.Id,
            Name = this.Name
        };
    }

    public void MapTo(UserDto destination)
    {
        destination.Id = this.Id;
        destination.Name = this.Name;
    }
}
```

### IMapFrom<TSource>

目标对象实现此接口，定义如何从源对象映射：

```csharp
public class UserDto : IMapFrom<User>
{
    public int Id { get; set; }
    public string Name { get; set; }

    public void MapFrom(User source)
    {
        this.Id = source.Id;
        this.Name = source.Name;
    }
}
```

## 访问原生 AutoMapper

如果需要使用 AutoMapper 的原生功能：

```csharp
using DotCommon.ObjectMapping;
using AutoMapper;

// 方法 1: 通过 IObjectMapper 获取
var mapper = objectMapper.GetMapper();

// 方法 2: 直接注入 IMapper
public class MyService
{
    private readonly IMapper _mapper;

    public MyService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public UserDto GetUser(int id)
    {
        var user = GetUserFromDatabase(id);
        return _mapper.Map<UserDto>(user);
    }
}

// 方法 3: 使用 IMapperAccessor
public class MyService
{
    private readonly IMapperAccessor _mapperAccessor;

    public MyService(IMapperAccessor mapperAccessor)
    {
        _mapperAccessor = mapperAccessor;
    }
}
```

## 完整示例

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using DotCommon.AutoMapper;
using DotCommon.ObjectMapping;

// 1. 定义实体和 DTO
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Order> Orders { get; set; }
}

public class UserDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public int OrderCount { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
}

// 2. 定义映射配置
public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(d => d.FullName,
                       opt => opt.MapFrom(s => $"{s.FirstName} {s.LastName}"))
            .ForMember(d => d.OrderCount,
                       opt => opt.MapFrom(s => s.Orders.Count));
    }
}

// 3. 配置服务
var services = new ServiceCollection();
services
    .AddDotCommon()
    .AddDotCommonAutoMapper()
    .Configure<DotCommonAutoMapperOptions>(options =>
    {
        options.AddMaps<Program>(validate: true);
    });

var serviceProvider = services.BuildServiceProvider();

// 4. 使用映射
var objectMapper = serviceProvider.GetRequiredService<IObjectMapper>();

var user = new User
{
    Id = 1,
    FirstName = "张",
    LastName = "三",
    Email = "zhangsan@example.com",
    CreatedAt = DateTime.Now,
    Orders = new List<Order>
    {
        new Order { Id = 1, TotalAmount = 100.50m },
        new Order { Id = 2, TotalAmount = 200.75m }
    }
};

var userDto = objectMapper.Map<User, UserDto>(user);

Console.WriteLine($"ID: {userDto.Id}");
Console.WriteLine($"Full Name: {userDto.FullName}");
Console.WriteLine($"Email: {userDto.Email}");
Console.WriteLine($"Order Count: {userDto.OrderCount}");
```

## 最佳实践

### 1. 组织映射配置

将映射配置按模块或功能分组到不同的 Profile 中：

```csharp
// 用户模块
public class UserMappingProfile : Profile { }

// 订单模块
public class OrderMappingProfile : Profile { }

// 产品模块
public class ProductMappingProfile : Profile { }
```

### 2. 避免过度映射

不要为每个实体都创建 DTO，只为需要的场景创建：

```csharp
// ✅ 好的做法 - 针对特定场景
public class UserListDto { }      // 列表显示
public class UserDetailDto { }    // 详情页面
public class UserCreateDto { }    // 创建用户

// ❌ 不好的做法 - 过度设计
public class UserDto1 { }
public class UserDto2 { }
public class UserDto3 { }
```

### 3. 使用配置验证

在开发和测试环境启用映射配置验证：

```csharp
#if DEBUG
options.AddMaps<Program>(validate: true);
#else
options.AddMaps<Program>(validate: false);
#endif
```

### 4. 性能优化

对于大量数据的映射，考虑使用投影查询：

```csharp
// ❌ 不推荐 - 查询所有数据后再映射
var users = dbContext.Users.ToList();
var dtos = objectMapper.Map<List<User>, List<UserDto>>(users);

// ✅ 推荐 - 使用 ProjectTo 在数据库层面投影
var mapper = objectMapper.GetMapper();
var dtos = dbContext.Users
    .ProjectTo<UserDto>(mapper.ConfigurationProvider)
    .ToList();
```

## 常见问题

### Q: 如何映射集合？

A: AutoMapper 自动支持集合映射：

```csharp
var users = new List<User> { /* ... */ };
var dtos = objectMapper.Map<List<User>, List<UserDto>>(users);

// 也支持数组、IEnumerable、ICollection 等
var array = objectMapper.Map<User[], UserDto[]>(users.ToArray());
```

### Q: 如何处理循环引用？

A: 配置最大深度限制：

```csharp
CreateMap<User, UserDto>()
    .MaxDepth(3);
```

### Q: 如何忽略某些属性？

A: 使用 `Ignore()`：

```csharp
CreateMap<User, UserDto>()
    .ForMember(d => d.Password, opt => opt.Ignore());
```

## 依赖项

- AutoMapper (>= 15.1.0)
- DotCommon
- Microsoft.Extensions.DependencyInjection

## 许可证

本项目采用 MIT 许可证

## 相关链接

- [DotCommon 主仓库](https://github.com/cocosip/DotCommon)
- [AutoMapper 官方文档](https://docs.automapper.org/)
- [NuGet 包](https://www.nuget.org/packages/DotCommon.AutoMapper)
