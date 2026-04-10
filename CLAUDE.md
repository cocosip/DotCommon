# CLAUDE.md

This file provides guidance to Claude Code when working with this repository.

## Project Overview

DotCommon is a C# utility library for .NET applications. It provides common abstractions and default implementations for serialization, object mapping, caching, scheduling, timing, threading, and related infrastructure concerns. The codebase follows a pluggable provider pattern so implementations can be swapped through dependency injection.

## Build And Test

- Build solution: `dotnet build DotCommon.sln`
- Build release: `dotnet build DotCommon.sln -c:Release`
- Run all tests: `dotnet test DotCommon.sln`
- Run core tests: `dotnet test test/DotCommon.Test/DotCommon.Test.csproj`
- Build script: `./build/build-all-release.ps1`
- Test script: `./build/test-all.ps1`
- Pack script: `./build/pack-all.ps1`

## Architecture

### Core Abstractions

The project uses interface-first abstractions with DI-registered default implementations.

- `IJsonSerializer`: default JSON serializer based on `System.Text.Json`
- `IObjectSerializer`: default object serializer
- `IObjectMapper`: default object mapper with pluggable auto-mapping provider
- `IAutoObjectMappingProvider`: extension point for AutoMapper or custom mapping backends

### Object Mapping

Object mapping follows a three-level resolution order:

1. Type-specific mappers via `IObjectMapper<TSource, TDestination>`
2. Generic mapping via `IObjectMapper`
3. Auto-mapping via `IAutoObjectMappingProvider`

Interface-based mapping patterns are also used in the codebase. The main implementation is `DefaultObjectMapper` under `src/DotCommon/DotCommon/ObjectMapping/`.

### Reflection And Conversion

`ExpressionMapper` is a performance-sensitive component for object and dictionary conversion. It compiles and caches conversion logic and is the preferred path for high-throughput reflection-based mapping scenarios.

Related utilities include `EmitMapper`, `PropertyInfoUtil`, `TypeUtil`, and `ReflectionUtil`.

### JSON Serialization

JSON support is built on `System.Text.Json` with custom converters in `src/DotCommon/DotCommon/Json/SystemTextJson/`.

Important behaviors:

- Date/time normalization support
- String-to-enum conversion
- String-to-boolean conversion
- Inferred type handling for `object`

### Caching

Caching abstractions live in `src/DotCommon.Caching/`.

- `IDistributedCache<T>` for generic distributed cache access
- `IHybridCache<T>` for layered memory plus distributed caching
- `DotCommonHybridCache` as the main hybrid cache implementation
- Serializer and key normalizer abstractions for cache storage behavior

There is also a Redis integration package under `src/DotCommon.Caching.StackExchangeRedis/`.

### Scheduling

Scheduling is centered on `IScheduleService` and the default `ScheduleService`. There is also a limited-concurrency task scheduler and token-provider abstractions for coordinated cancellation.

### Timing

Timing support includes:

- `IClock`
- `ICurrentTimezoneProvider`
- `ITimezoneProvider`

Timezone conversion relies on `TimeZoneConverter`.

## Project Structure

- `src/DotCommon/`: core library
- `src/DotCommon.AutoMapper/`: AutoMapper integration
- `src/DotCommon.Caching/`: caching abstractions and implementations
- `src/DotCommon.Caching.StackExchangeRedis/`: Redis cache integration
- `src/DotCommon.Crypto/`: crypto extensions
- `src/DotCommon.DistributedLocking/`: distributed lock integration
- `src/DotCommon.AspNetCore.Mvc/`: ASP.NET Core MVC integration
- `test/DotCommon.Test/`: main test project
- `test/DotCommon.AspNetCore.Mvc.Test/`: MVC integration tests

## Coding Standards

### Naming

- Classes use UpperCamelCase
- Interfaces start with `I`
- Methods use UpperCamelCase
- Async methods end with `Async`
- Private fields use `_` prefix

### Organization

- Comments must be in English
- Use `#region` to group fields and methods where appropriate
- Prefer one class per file unless small related types belong together

### Error Handling

- Use the `Check` helper for parameter validation
- Keep exception hierarchies aligned with the relevant base types
- Implement `IDisposable` where resource ownership requires it

### Style

- 4 spaces indentation
- Allman braces
- Maximum line length: 120
- Always use braces, even for single-line statements

### Tests

- Test names follow `MethodName_State_ExpectedBehavior`
- Use Arrange-Act-Assert structure
- Mirror source structure under `test/DotCommon.Test/` where possible

### Nullable Reference Types

- Nullable is not universally assumed across the repository
- Check the target project before introducing nullable annotations
- Use nullable syntax only where the target project enables it

## Dependency Management

Package versions are centrally managed in `Directory.Packages.props`. Do not hardcode versions in individual project files.

Important dependencies include `Microsoft.Extensions.*`, `System.Text.Json`, `AutoMapper`, `TimeZoneConverter`, `BouncyCastle.Cryptography`, `DistributedLock.Core`, and `xUnit`.

## Key File Locations

- DI registrations: `src/DotCommon/Microsoft/Extensions/DependencyInjection/ServiceCollectionExtensions.cs`
- Object mapping core: `src/DotCommon/DotCommon/ObjectMapping/DefaultObjectMapper.cs`
- Expression mapping: `src/DotCommon/DotCommon/Reflecting/ExpressionMapper.cs`
- JSON converters: `src/DotCommon/DotCommon/Json/SystemTextJson/`
- AutoMapper integration: `src/DotCommon.AutoMapper/DotCommon/AutoMapper/`
- Caching components: `src/DotCommon.Caching/DotCommon/Caching/`
