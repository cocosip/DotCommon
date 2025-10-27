# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

DotCommon is a C# utility library providing core abstractions and utilities for .NET applications. It uses a pluggable provider pattern to support swappable implementations for serialization, object mapping, caching, and scheduling.

## Build and Test Commands

### Build
```bash
# Build entire solution
dotnet build DotCommon.sln

# Build in Release mode
dotnet build DotCommon.sln -c:Release

# Or use PowerShell script
./build/build-all-release.ps1
```

### Test
```bash
# Run all tests
dotnet test DotCommon.sln

# Run tests for specific project
dotnet test test/DotCommon.Test/DotCommon.Test.csproj

# Or use PowerShell script
./build/test-all.ps1
```

### Package
```bash
# Create NuGet packages (outputs to dest/)
./build/pack-all.ps1
```

## Architecture

### Core Abstraction Pattern

DotCommon uses a **provider pattern** throughout - most features are defined as interfaces with swappable implementations registered via dependency injection:

- **IJsonSerializer** → Default: `DotCommonSystemTextJsonSerializer` (System.Text.Json)
- **IObjectSerializer** → Default: `DefaultObjectSerializer` (JSON-based)
- **IObjectMapper** → Default: `DefaultObjectMapper` (with pluggable auto-mapper)
- **IAutoObjectMappingProvider** → Default: `NotImplementedAutoObjectMappingProvider` (requires explicit registration of AutoMapper or custom implementation)

### Object Mapping Architecture

The object mapping system has **three levels of specificity** (checked in order):

1. **Type-specific mappers**: `IObjectMapper<TSource, TDestination>` - Highest priority, for custom mapping logic
2. **Generic mapper**: `IObjectMapper` - Handles collections and delegates to auto-mapper
3. **Auto-mapping provider**: `IAutoObjectMappingProvider` - Pluggable (AutoMapper via `DotCommon.AutoMapper` package)

**Interface-based mapping**:
- Source objects can implement `IMapTo<TDestination>` to control mapping
- Destination types can have constructors accepting source type (for `IMapFrom<TSource>` pattern)

**Implementation**: The `DefaultObjectMapper` (src/DotCommon/DotCommon/ObjectMapping/DefaultObjectMapper.cs) implements this fallback chain.

### High-Performance Reflection

**ExpressionMapper** (src/DotCommon/DotCommon/Reflecting/ExpressionMapper.cs) compiles Expression Trees to IL for fast object ↔ dictionary conversion. Uses concurrent dictionary caching for compiled converters. When working with object-to-dictionary scenarios, this is the preferred high-performance approach.

Other reflection utilities:
- `EmitMapper` - IL Emit-based mapping
- `PropertyInfoUtil` - Cached property metadata
- `TypeUtil`, `ReflectionUtil` - Type analysis

### Dependency Injection Setup

Service registration follows a fluent pattern via `ServiceCollectionExtensions`:

```csharp
services
    .AddDotCommon()                    // Core services
    .AddDotCommonSchedule()            // Scheduling
    .AddDotCommonObjectMapper()        // Object mapping
    .AddDotCommonSystemTextJsonSerializer(); // JSON
```

Extension packages add their own registration methods:
```csharp
services.AddDotCommonAutoMapper()      // AutoMapper integration
    .AddAssemblyAutoMaps(assemblies)   // Scan for mapping attributes
    .BuildAutoMapper();

services.AddGenericsMemoryCache();     // Caching
```

**Key DI utilities**:
- `IObjectAccessor<T>` - Lazy dependency resolution pattern
- `ServiceCollectionCommonExtensions.IsAdded<T>()` - Check service registration
- `GetSingletonInstance<T>()` - Retrieve singleton from collection

### JSON Serialization

Built on System.Text.Json with custom converters in `src/DotCommon/DotCommon/Json/SystemTextJson/`:

- **DotCommonDateTimeConverter** - DateTime normalization (respects `ICurrentTimezoneProvider`)
- **DotCommonStringToEnumConverter** - String → Enum conversion
- **DotCommonStringToBooleanConverter** - Flexible boolean parsing
- **ObjectToInferredTypesConverter** - Type inference for `object` types

**Date/Time handling**: The library normalizes DateTime values to UTC during JSON serialization/deserialization unless disabled with `[DisableDateTimeNormalization]` attribute.

### Caching Infrastructure

Multi-level caching abstraction in `src/DotCommon.Caching/`:

- **IDistributedCache<T>** - Generic distributed cache
- **IHybridCache<T>** - L1 (memory) + L2 (distributed) hybrid
- **DotCommonHybridCache** - Implementation with JSON serialization
- **IDistributedCacheSerializer** - Pluggable serialization (default: UTF8 JSON)
- **DistributedCacheKeyNormalizer** - Key transformation with prefix support

### Scheduling

Background task scheduling via `IScheduleService`:

- **ScheduleService** - Default TPL-based implementation
- **LimitedConcurrencyLevelTaskScheduler** - Limits parallel execution
- **ICancellationTokenProvider** - Token injection for graceful shutdown

### Timing Abstractions

Multi-timezone support:
- **IClock** - Current time provider (respects timezone)
- **ICurrentTimezoneProvider** - User's timezone context
- **ITimezoneProvider** - Timezone conversion (uses TimeZoneConverter library)

## Project Structure

```
src/
├── DotCommon/                        # Core library (netstandard2.0, netstandard2.1)
│   ├── Collections/                  # Collection utilities
│   ├── DependencyInjection/          # DI extensions
│   ├── Encrypt/                      # Encryption utilities (MD5, RSA, AES, DES, SM2/SM3/SM4)
│   ├── IO/                           # IO operations
│   ├── Json/SystemTextJson/          # JSON serialization with custom converters
│   ├── ObjectMapping/                # Object mapping abstractions
│   ├── Reflecting/                   # High-performance reflection (ExpressionMapper, EmitMapper)
│   ├── Scheduling/                   # Background task scheduling
│   ├── Serialization/                # Binary serialization abstractions
│   ├── Threading/                    # Threading utilities
│   ├── Timing/                       # Clock and timezone abstractions
│   └── Utility/                      # 30+ utility classes (conversion, encryption, text, etc.)
├── DotCommon.AutoMapper/             # AutoMapper integration (net9.0)
├── DotCommon.Caching/                # Caching abstractions (netstandard2.0, netstandard2.1)
├── DotCommon.Crypto/                 # Cryptography extensions (netstandard2.0, netstandard2.1)
└── DotCommon.AspNetCore.Mvc/         # ASP.NET Core integration (net9.0)
```

## Coding Standards (from .trae/rules/project_rules.md)

### Naming Conventions
- **Classes**: UpperCamelCase, utilities end with `Util` or `Helper`
- **Interfaces**: Prefix with `I`, e.g., `IObjectMapper`
- **Methods**: UpperCamelCase, async methods end with `Async`
- **Private fields**: Prefix with `_`, e.g., `_logger`
- **Constants**: UpperCamelCase

### Code Organization
- **Comments**: Must be in English
- **Regions**: Use `#region` to group related code (fields, public methods, private methods)
- **File structure**: One class per file (exceptions allowed for small related types)

### Error Handling
- Use `Check` class for parameter validation at method start
- Custom exceptions inherit from appropriate base classes
- Implement `IDisposable` for resource management

### Code Style
- **Indentation**: 4 spaces (no tabs)
- **Braces**: Allman style (opening brace on new line)
- **Line length**: Max 120 characters
- Always use braces for single-line statements

### Testing
- Test method naming: `MethodName_State_ExpectedBehavior`
- Use Arrange-Act-Assert pattern
- Test files in `test/DotCommon.Test/` mirror `src/DotCommon/` structure

### Nullable Reference Types
- Not required by default in this project
- When generating code, check if target project has `<Nullable>enable</Nullable>`
- If enabled, use `?` for nullable references and `!` null-forgiving operator judiciously
- If disabled, omit nullable annotations

## Dependency Management

Package versions are centrally managed in `Directory.Packages.props`. Do not specify versions in individual `.csproj` files.

Key dependencies:
- **Microsoft.Extensions.*** (9.x) - DI, Options, Configuration, Caching
- **System.Text.Json** - JSON serialization
- **AutoMapper** (15.1.0) - Object mapping (via DotCommon.AutoMapper)
- **TimeZoneConverter** (7.2.0) - Timezone handling
- **BouncyCastle.Cryptography** (2.6.2) - SM2/SM3/SM4 encryption
- **xUnit** (2.9.3) - Testing framework

## Common Development Patterns

### Adding a New Utility Class

1. Create class in appropriate `src/DotCommon/DotCommon/Utility/` namespace
2. Use static class pattern for stateless utilities
3. Add XML documentation (in English) for all public members
4. Add corresponding test file in `test/DotCommon.Test/Utility/`

### Creating a New Abstraction

1. Define interface in `src/DotCommon/DotCommon/<Feature>/` (e.g., `IMyService`)
2. Create default implementation (e.g., `DefaultMyService`)
3. Add service registration extension method in `ServiceCollectionExtensions.cs`:
   ```csharp
   public static IServiceCollection AddMyService(this IServiceCollection services)
   {
       return services.AddSingleton<IMyService, DefaultMyService>();
   }
   ```
4. Add integration tests

### Implementing a Custom Mapper

Register type-specific mapper with highest priority:
```csharp
services.AddSingleton<IObjectMapper<SourceType, DestType>, CustomMapper>();
```

Or use interface-based approach on source/destination classes:
```csharp
public class Destination : IMapFrom<Source>
{
    public Destination(Source source) { /* map */ }
}
```

## Key File Locations

- **DI Setup**: `src/DotCommon/Microsoft/Extensions/DependencyInjection/ServiceCollectionExtensions.cs`
- **Object Mapping Core**: `src/DotCommon/DotCommon/ObjectMapping/DefaultObjectMapper.cs`
- **Expression Mapping**: `src/DotCommon/DotCommon/Reflecting/ExpressionMapper.cs` (355 lines, performance-critical)
- **JSON Converters**: `src/DotCommon/DotCommon/Json/SystemTextJson/`
- **AutoMapper Integration**: `src/DotCommon.AutoMapper/DotCommon/AutoMapper/AutoMapperAutoObjectMappingProvider.cs`
- **Caching**: `src/DotCommon.Caching/DotCommon/Caching/`
