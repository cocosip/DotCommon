# Qwen 项目：DotCommon (Qwen Project: DotCommon)

本指南为 `DotCommon` 项目的开发和代码生成提供了上下文和规则。

# C# 编码规范

## 一、项目结构标准
```
src/
├── DotCommon/                     # 核心类库
│   ├── DotCommon/                 # 核心命名空间
│   │   ├── Collections/           # 集合相关工具
│   │   ├── DependencyInjection/   # 依赖注入扩展
│   │   ├── Encrypt/               # 加密相关工具
│   │   ├── IO/                    # IO 操作工具
│   │   ├── Json/                  # JSON 处理工具
│   │   ├── ObjectMapping/         # 对象映射工具
│   │   ├── Reflecting/            # 反射相关工具
│   │   ├── Reflection/            # 反射相关工具
│   │   ├── Scheduling/            # 调度相关工具
│   │   ├── Serialization/         # 序列化相关工具
│   │   ├── Threading/             # 多线程工具
│   │   ├── Timing/                # 时间相关工具
│   │   ├── Utility/               # 通用工具类
│   │   └── *.cs                   # 核心类文件
│   └── *.csproj                   # 项目文件
├── DotCommon.AspNetCore.Mvc/      # ASP.NET Core MVC 扩展
├── DotCommon.AutoMapper/          # AutoMapper 扩展
├── DotCommon.Caching/             # 缓存扩展
├── DotCommon.Crypto/              # 加密扩展
└── DotCommon.Serialization/       # 序列化扩展
```

## 二、命名规范
1. **命名空间**:
   - 与文件夹结构对齐
   - 使用 `UpperCamelCase` 命名法
   - 示例: `DotCommon.Utility`, `DotCommon.Serialization.Json`

2. **类名**:
   - 使用 `UpperCamelCase` 命名法
   - 工具类以 `Util` 或 `Helper` 结尾
   - 异常类以 `Exception` 结尾
   - 示例: `StringUtil`, `JsonHelper`, `DotCommonException`

3. **接口名**:
   - 以 `I` 开头，后跟 `UpperCamelCase` 命名法
   - 示例: `IObjectMapper`, `ICacheManager`

4. **方法名**:
   - 使用 `UpperCamelCase` 命名法
   - 异步方法以 `Async` 结尾
   - 示例: `GetString`, `SaveAsync`

5. **变量名**:
   - 使用 `lowerCamelCase` 命名法
   - 私有字段以下划线 `_` 开头
   - 示例: `userName`, `_logger`

6. **常量名**:
   - 使用 `UpperCamelCase` 命名法
   - 静态只读字段使用 `UpperCamelCase` 命名法
   - 示例: `MaxBufferSize`, `DefaultTimeout`

## 三、代码结构规范
### 1. 文件组织
- 代码注释必须使用英文
- 一个文件一个类（特殊情况除外）
- 文件名应与类名匹配
- 使用 `#region` 对相关代码进行分组

### 2. 类组织
```
// 类文件结构示例
namespace DotCommon.Utility
{
    /// <summary>
    /// 字符串工具类
    /// </summary>
    public static class StringUtil
    {
        #region 字段和属性
        
        // 静态只读字段
        private static readonly Regex UrlFilterRegex = new Regex(...);
        
        #endregion
        
        #region 公共方法
        
        /// <summary>
        /// 获取字符串的显示宽度
        /// </summary>
        public static int GetEastAsianWidthCount(string source)
        {
            // 实现
        }
        
        #endregion
        
        #region 私有方法
        
        // 私有辅助方法
        
        #endregion
    }
}
```

### 3. 方法组织
- 方法应简洁且职责单一
- 参数验证应在方法开始时进行
- 使用提前返回来减少嵌套层级

## 四、文档规范
1. **XML 文档注释**:
   - 所有公共类、方法和属性必须有 XML 文档注释
   - 注释必须使用英文
   - 使用标准 XML 文档格式

2. **代码内注释**:
   - 仅在必要时添加注释
   - 注释应解释"为什么"而不是"是什么"
   - 使用 `// TODO:` 标记未完成的功能
   - 使用 `// FIXME:` 标记需要修复的问题

## 五、错误处理规范
1. **参数验证**:
   - 使用 `Check` 类进行参数验证
   - 在方法开始时执行所有参数验证

2. **异常处理**:
   - 自定义异常应继承自适当的基类
   - 提供所有标准异常构造函数
   - 异常消息应清楚地描述问题

3. **资源管理**:
   - 实现 `IDisposable` 接口来管理资源
   - 使用 `using` 语句确保正确释放资源

## 六、代码风格规范
1. **格式化**:
   - 使用 4 个空格缩进（不使用制表符）
   - 开括号另起一行（Allman 风格）
   - 单行语句也应使用大括号

2. **行长度**:
   - 每行不超过 120 个字符
   - 过长的行应适当换行

3. **空行和空格**:
   - 方法之间用空行分隔
   - 运算符周围添加空格
   - 行尾不添加额外空格

## 七、测试规范
1. **测试覆盖率**:
   - 新代码应有相应的单元测试
   - 测试覆盖率应达到较高水平

2. **测试命名**:
   - 测试方法名格式: `方法名_状态_预期行为`
   - 示例: `TrimEnd_WithValidSuffix_ShouldRemoveSuffix`

3. **测试组织**:
   - 测试类应与被测试的类对应
   - 使用 Arrange-Act-Assert 模式

## 八、依赖管理规范
1. **包版本管理**:
   - 在 `Directory.Packages.props` 中集中管理包版本
   - 避免版本冲突

2. **依赖注入**:
   - 提供扩展方法来注册服务
   - 遵循 ASP.NET Core 的依赖注入模式
  
3. **可空引用类型 (Nullable Reference Types)**:
    - 项目不强制要求启用可空引用类型功能
    - 在生成代码时，需要检查目标项目是否启用了可空引用类型（检查项目文件中的 `<Nullable>enable</Nullable>` 设置）
    - 如果目标项目启用了可空引用类型，则生成的代码应遵循可空引用类型规则：
        - 明确标注可空引用类型，使用 `?` 后缀（例如 `string?`）表示可能为 null 的引用类型
        - 在适当的地方使用 `!` 操作符（null-forgiving operator）来抑制编译器警告，但应谨慎使用并确保运行时安全
    - 如果目标项目未启用可空引用类型，则生成的代码应保持与项目现有设置一致，避免引入不必要的可空注解

## 九、构建和部署规范
1. **构建命令**:
   ```bash
   dotnet build DotCommon.sln
   ```

2. **测试命令**:
   ```bash
   dotnet test DotCommon.sln
   ```

3. **打包命令**:
   ```bash
   ./build/pack-all.ps1
   ```

## 十、常见任务指南
1. **添加新功能**:
   - 在适当的 `src/DotCommon.*` 项目中创建新类
   - 确保符合现有模式和约定
   - 在 `test/DotCommon.Test` 项目中添加相应的单元测试

2. **重构**:
   - 分析现有代码以识别改进点
   - 应用重构技术
   - 确保所有测试通过

3. **依赖管理**:
   - 在 `Directory.Packages.props` 中集中管理 NuGet 包版本
