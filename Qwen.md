# Project Overview

DotCommon is a generic class library built on .NET Core and .NET Standard. It provides common utilities and extensions in various areas such as cryptography, serialization, and dependency injection.

# Tech Stack

- **Framework**: .NET Core, .NET Standard
- **Language**: C#

# Project Structure

- `src/`: Contains source code for all library projects (e.g., `DotCommon`, `DotCommon.AspNetCore.Mvc`, `DotCommon.Caching`).
- `test/`: Contains unit tests and integration tests for library projects (e.g., `DotCommon.Test`).
- `samples/`: Contains sample applications demonstrating library usage (e.g., `DotCommon.ConsoleTest`).
- `build/`: Contains PowerShell scripts for building, testing, and packaging the solution.
- `docs/`: Contains project documentation.
- `.github/workflows/`: Contains GitHub Actions workflows for CI/CD (build, test, release).
- `common.props`, `Directory.Build.props`, `Directory.Packages.props`: MSBuild property files for common configuration and centralized package management.

# Build and Test Commands

## Build

Build the entire solution:
```bash
dotnet build DotCommon.sln
```
Or use the PowerShell script for a release build:
```bash
./build/build-all-release.ps1
```

## Test

Run all tests:
```bash
dotnet test DotCommon.sln
```
Or use the PowerShell script:
```bash
./build/test-all.ps1
```

## Packaging

Package all projects into NuGet packages:
```bash
./build/pack-all.ps1
```

# Coding Standards and Style

Please follow the existing C# coding standards and style in the `src/` directory. This includes naming conventions, formatting, and architectural patterns. The project uses centralized package management through `Directory.Packages.props`.
- **Code Comments**: All code comments must be in English unless a specific method or class is explicitly designated to use Chinese comments.

# Common Tasks

- **Adding New Features**: Create new classes or modules in the appropriate `src/DotCommon.*` project, ensuring they follow existing patterns and conventions. Add corresponding unit tests in the `test/DotCommon.Test` project.
- **Refactoring**: Analyze existing code, identify areas for improvement, and apply refactoring techniques while ensuring all tests pass.
- **Dependency Management**: Centrally manage NuGet package versions in `Directory.Packages.props`.