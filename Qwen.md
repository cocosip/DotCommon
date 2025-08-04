# 项目概述

DotCommon 是一个基于 .NET Core 和 .NET Standard 构建的通用类库。它在加密、序列化和依赖注入等多个领域提供常见的工具和扩展。

# 技术栈

- **框架**: .NET Core, .NET Standard
- **语言**: C#

# 项目结构

- `src/`: 包含所有库项目的源代码 (例如 `DotCommon`, `DotCommon.AspNetCore.Mvc`, `DotCommon.Caching`)。
- `test/`: 包含库项目的单元测试和集成测试 (例如 `DotCommon.Test`)。
- `samples/`: 包含演示库使用方法的示例应用程序 (例如 `DotCommon.ConsoleTest`)。
- `build/`: 包含用于构建、测试和打包解决方案的 PowerShell 脚本。
- `docs/`: 包含项目文档。
- `.github/workflows/`: 包含用于 CI/CD (构建、测试、发布) 的 GitHub Actions 工作流。
- `common.props`, `Directory.Build.props`, `Directory.Packages.props`: 用于通用配置和集中包管理的 MSBuild 属性文件。

# 构建和测试命令

## 构建

构建整个解决方案：
```bash
dotnet build DotCommon.sln
```
或者使用 PowerShell 脚本进行发布构建：
```bash
./build/build-all-release.ps1
```

## 测试

运行所有测试：
```bash
dotnet test DotCommon.sln
```
或者使用 PowerShell 脚本：
```bash
./build/test-all.ps1
```

## 打包

将所有项目打包成 NuGet 包：
```bash
./build/pack-all.ps1
```

# 编码标准和风格

请遵循 `src/` 目录中现有的 C# 编码标准和风格。这包括命名约定、格式化和架构模式。项目通过 `Directory.Packages.props` 使用集中式包管理。
- **代码注释**: 所有代码注释必须使用英文，除非特定方法或类明确指定使用中文注释。

# 常见任务

- **添加新功能**: 在适当的 `src/DotCommon.*` 项目中创建新类或模块，确保它们遵循现有的模式和约定。在 `test/DotCommon.Test` 项目中添加相应的单元测试。
- **重构**: 分析现有代码，识别改进领域，并应用重构技术，同时确保所有测试通过。
- **依赖管理**: 在 `Directory.Packages.props` 中集中管理 NuGet 包版本。