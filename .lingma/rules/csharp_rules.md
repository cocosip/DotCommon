trigger: always_on

# C# Coding Rules

## I. Project Structure Standards
```
src/
├── DotCommon/                     # Core class library
│   ├── DotCommon/                 # Core namespace
│   │   ├── Collections/           # Collection-related utilities
│   │   ├── DependencyInjection/   # Dependency injection extensions
│   │   ├── Encrypt/               # Encryption-related utilities
│   │   ├── IO/                    # IO operation utilities
│   │   ├── Json/                  # JSON processing utilities
│   │   ├── ObjectMapping/         # Object mapping utilities
│   │   ├── Reflecting/            # Reflection-related utilities
│   │   ├── Reflection/            # Reflection-related utilities
│   │   ├── Scheduling/            # Scheduling-related utilities
│   │   ├── Serialization/         # Serialization-related utilities
│   │   ├── Threading/             # Multi-threading utilities
│   │   ├── Timing/                # Time-related utilities
│   │   ├── Utility/               # General utility classes
│   │   └── *.cs                   # Core class files
│   └── *.csproj                   # Project files
├── DotCommon.AspNetCore.Mvc/      # ASP.NET Core MVC extensions
├── DotCommon.AutoMapper/          # AutoMapper extensions
├── DotCommon.Caching/             # Caching extensions
├── DotCommon.Crypto/              # Encryption extensions
└── DotCommon.Serialization/       # Serialization extensions
```

## II. Naming Standards
1. **Namespaces**:
   - Align with folder structure
   - Use `UpperCamelCase`
   - Example: `DotCommon.Utility`, `DotCommon.Serialization.Json`

2. **Class Names**:
   - Use `UpperCamelCase`
   - Utility classes end with `Util` or `Helper`
   - Exception classes end with `Exception`
   - Example: `StringUtil`, `JsonHelper`, `DotCommonException`

3. **Interface Names**:
   - Start with `I`, followed by `UpperCamelCase`
   - Example: `IObjectMapper`, `ICacheManager`

4. **Method Names**:
   - Use `UpperCamelCase`
   - Async methods end with `Async`
   - Example: `GetString`, `SaveAsync`

5. **Variable Names**:
   - Use `lowerCamelCase`
   - Private fields start with `_`
   - Example: `userName`, `_logger`

6. **Constant Names**:
   - Use `UpperCamelCase`
   - Static readonly fields use `UpperCamelCase`
   - Example: `MaxBufferSize`, `DefaultTimeout`

## III. Code Structure Standards
### 1. File Organization
- Code comments must be in English
- One class per file (except for special cases)
- File names should match class names
- Use `#region` to group related code

### 2. Class Organization
```
// Class file structure example
namespace DotCommon.Utility
{
    /// <summary>
    /// String utility class
    /// </summary>
    public static class StringUtil
    {
        #region Fields and Properties
        
        // Static readonly fields
        private static readonly Regex UrlFilterRegex = new Regex(...);
        
        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Get the display width of a string
        /// </summary>
        public static int GetEastAsianWidthCount(string source)
        {
            // Implementation
        }
        
        #endregion
        
        #region Private Methods
        
        // Private helper methods
        
        #endregion
    }
}
```

### 3. Method Organization
- Methods should be concise with single responsibility
- Parameter validation should be performed at the beginning of methods
- Use early returns to reduce nesting levels

## IV. Documentation Standards
1. **XML Documentation Comments**:
   - All public classes, methods, and properties must have XML documentation comments
   - Comments must be in English
   - Use standard XML documentation format

2. **In-code Comments**:
   - Add comments only when necessary
   - Comments should explain "why" rather than "what"
   - Use `// TODO:` to mark incomplete features
   - Use `// FIXME:` to mark issues that need fixing

## V. Error Handling Standards
1. **Parameter Validation**:
   - Use the `Check` class for parameter validation
   - Perform all parameter validation at the beginning of methods

2. **Exception Handling**:
   - Custom exceptions should inherit from appropriate base classes
   - Provide all standard exception constructors
   - Exception messages should clearly describe the problem

3. **Resource Management**:
   - Implement the `IDisposable` interface to manage resources
   - Use `using` statements to ensure proper resource disposal

## VI. Code Style Standards
1. **Formatting**:
   - Use 4-space indentation (no tabs)
   - Opening braces on new lines (Allman style)
   - Always use braces for single-line statements

2. **Line Length**:
   - No more than 120 characters per line
   - Overly long lines should be appropriately wrapped

3. **Blank Lines and Spaces**:
   - Separate methods with blank lines
   - Add spaces around operators
   - Don't add extra spaces at the end of lines

## VII. Testing Standards
1. **Test Coverage**:
   - New code should have corresponding unit tests
   - Test coverage should reach a high level

2. **Test Naming**:
   - Test method name format: `MethodName_State_ExpectedBehavior`
   - Example: `TrimEnd_WithValidSuffix_ShouldRemoveSuffix`

3. **Test Organization**:
   - Test classes should correspond to the classes being tested
   - Use the Arrange-Act-Assert pattern

## VIII. Dependency Management Standards
1. **Package Version Management**:
   - Centrally manage package versions in `Directory.Packages.props`
   - Avoid version conflicts

2. **Dependency Injection**:
   - Provide extension methods to register services
   - Follow ASP.NET Core's dependency injection pattern

## IX. Build and Deployment Standards
1. **Build Commands**:
   ```bash
   dotnet build DotCommon.sln
   ```

2. **Test Commands**:
   ```bash
   dotnet test DotCommon.sln
   ```

3. **Packaging Commands**:
   ```bash
   ./build/pack-all.ps1
   ```

## X. Common Task Guidelines
1. **Adding New Features**:
   - Create new classes in the appropriate `src/DotCommon.*` project
   - Ensure compliance with existing patterns and conventions
   - Add corresponding unit tests in the `test/DotCommon.Test` project

2. **Refactoring**:
   - Analyze existing code to identify improvement points
   - Apply refactoring techniques
   - Ensure all tests pass

3. **Dependency Management**:
   - Centrally manage NuGet package versions in `Directory.Packages.props`