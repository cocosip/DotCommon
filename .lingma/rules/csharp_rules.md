---
trigger: always_on
---

---
# 使用英文进行代码的注释
---

## 一、项目结构规范
```
src/
├── DotCommon/                     # 核心类库
│   ├── DotCommon/                 # 核心命名空间
│   │   ├── Collections/           # 集合相关工具
│   │   ├── DependencyInjection/   # 依赖注入扩展
│   │   ├── Encrypt/               # 加密相关工具
│   │   ├── IO/                    # IO操作工具
│   │   ├── Json/                  # JSON处理工具
│   │   ├── ObjectMapping/         # 对象映射工具
│   │   ├── Reflecting/            # 反射相关工具
│   │   ├── Reflection/            # 反射相关工具
│   │   ├── Scheduling/            # 调度相关工具
│   │   ├── Serialization/         # 序列化相关工具
│   │   ├── Threading/             # 多线程相关工具
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

---

## 二、命名规范
1. **命名空间**：
   - 与文件夹结构保持一致
   - 使用 `UpperCamelCase`
   - 示例：`DotCommon.Utility`, `DotCommon.Serialization.Json`

2. **类名**：
   - 使用 `UpperCamelCase`
   - 工具类以 `Util` 或 `Helper` 结尾
   - 异常类以 `Exception` 结尾
   - 示例：`StringUtil`, `JsonHelper`, `DotCommonException`

3. **接口名**：
   - 以 `I` 开头，后跟 `UpperCamelCase`
   - 示例：`IObjectMapper`, `ICacheManager`

4. **方法名**：
   - 使用 `UpperCamelCase`
   - 异步方法以 `Async` 结尾
   - 示例：`GetString`, `SaveAsync`

5. **变量名**：
   - 使用 `lowerCamelCase`
   - 私有字段以 `_` 开头
   - 示例：`userName`, `_logger`

6. **常量名**：
   - 使用 `UpperCamelCase`
   - 静态只读字段使用 `UpperCamelCase`
   - 示例：`MaxBufferSize`, `DefaultTimeout`

---

## 三、代码结构规范
### 1. 文件组织
- 一个类一个文件（特殊情况除外）
- 文件名与类名保持一致
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
- 方法应保持简洁，单一职责
- 参数验证应在方法开始处进行
- 使用早期返回减少嵌套层级

---

## 四、文档规范
1. **XML 文档注释**：
   - 所有公共类、方法、属性都必须有 XML 文档注释
   - 注释必须使用英文
   - 使用标准的 XML 文档格式

2. **代码内注释**：
   - 仅在必要时添加注释
   - 注释应解释"为什么"而不是"是什么"
   - 使用 `// TODO:` 标记待完成的功能
   - 使用 `// FIXME:` 标记需要修复的问题

---

## 五、错误处理规范
1. **参数验证**：
   - 使用 `Check` 类进行参数验证
   - 在方法开始处进行所有参数验证

2. **异常处理**：
   - 自定义异常应继承自合适的基类
   - 提供所有标准异常构造函数
   - 异常信息应清晰描述问题

3. **资源管理**：
   - 实现 `IDisposable` 接口管理资源
   - 使用 `using` 语句确保资源正确释放

---

## 六、代码风格规范
1. **格式化**：
   - 使用 4 个空格缩进（不使用 Tab）
   - 左大括号另起一行（Allman 风格）
   - 单行语句也使用大括号

2. **行长度**：
   - 每行不超过 120 个字符
   - 过长的行应合理换行

3. **空行和空格**：
   - 方法之间使用空行分隔
   - 运算符前后添加空格
   - 不在行尾添加多余空格

---

## 七、测试规范
1. **测试覆盖率**：
   - 新增代码应有相应的单元测试
   - 测试覆盖率应达到较高水平

2. **测试命名**：
   - 测试方法名格式：`MethodName_State_ExpectedBehavior`
   - 示例：`TrimEnd_WithValidSuffix_ShouldRemoveSuffix`

3. **测试组织**：
   - 测试类与被测试类保持对应关系
   - 使用 Arrange-Act-Assert 模式

---

## 八、依赖管理规范
1. **包版本管理**：
   - 在 `Directory.Packages.props` 中集中管理包版本
   - 避免版本冲突

2. **依赖注入**：
   - 提供扩展方法注册服务
   - 遵循 ASP.NET Core 的依赖注入模式

---

## 九、构建和部署规范
1. **构建命令**：
   ```bash
   dotnet build DotCommon.sln
   ```

2. **测试命令**：
   ```bash
   dotnet test DotCommon.sln
   ```

3. **打包命令**：
   ```bash
   ./build/pack-all.ps1
   ```

---

## 十、常见任务指南
1. **添加新功能**：
   - 在适当的 `src/DotCommon.*` 项目中创建新类
   - 确保遵循现有模式和约定
   - 在 `test/DotCommon.Test` 项目中添加相应单元测试

2. **重构**：
   - 分析现有代码找出改进点
   - 应用重构技术
   - 确保所有测试通过

3. **依赖管理**：
   - 在 `Directory.Packages.props` 中集中管理 NuGet 包版本