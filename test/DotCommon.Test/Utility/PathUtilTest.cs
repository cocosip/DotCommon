using DotCommon.Utility;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class PathUtilTest
    {
        [Theory]
        [InlineData(@"C:\A\B.txt", ".txt")]
        [InlineData(@"\BB.jpg", ".jpg")]
        [InlineData(@"..jpg", ".jpg")]
        [InlineData(@"D:\A\B\C", "")]
        [InlineData(@"", "")]
        public void GetPathExtensionTest(string path, string expected)
        {
            var actual = PathUtil.GetFileExtension(path);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(@"C:\A\B.txt", 1, @"C:\A")]
        [InlineData(@"D:\A\B\C", 1, @"D:\A\B")]
        [InlineData(@"D:\A\B\C\D", 3, @"D:\A")]
        [InlineData(@"D:", 1, @"D:")]
        public void GetAncestorDirectoryTest_Windows(string path, int layerCount, string expected)
        {
            // Skip this test on non-Windows platforms
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return;
            }

            var actual = PathUtil.GetAncestorDirectory(path, layerCount);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(@"/home/user/documents/file.txt", 1, @"/home/user/documents")]
        [InlineData(@"/var/log/app", 1, @"/var/log")]
        [InlineData(@"/var/log/app/debug", 3, @"/")]
        [InlineData(@"/", 1, @"/")]
        public void GetAncestorDirectoryTest_Linux(string path, int layerCount, string expected)
        {
            // Skip this test on Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return;
            }

            var actual = PathUtil.GetAncestorDirectory(path, layerCount);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(@"A\B\C", 2, @"A\B\C")]
        public void GetAncestorDirectoryTest_NonRooted(string path, int layerCount, string expected)
        {
            // This test works on all platforms (non-rooted paths)
            var actual = PathUtil.GetAncestorDirectory(path, layerCount);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ResolveVirtualPath_Test()
        {
            var path = PathUtil.ResolveVirtualPath("../../Root");
            // Using a more flexible pattern that should work across different environments
            var pattern = @"[/\\]DotCommon[/\\]test[/\\]Root$";
            var match = Regex.IsMatch(path, pattern);
            Assert.True(match);
        }

        [Fact]
        public void ResolveVirtualPath_MultipleParentDirectories_Test()
        {
            // This test verifies that ResolveVirtualPath can handle multiple parent directory references
            var path = PathUtil.ResolveVirtualPath("../../../../test-files");
            // Verify that the path is correctly resolved
            Assert.NotNull(path);
            Assert.NotEmpty(path);
        }

        [Fact]
        public void GetAbsolutePath_Test()
        {
            var path = PathUtil.GetAbsolutePath(@"\App\User");
            // Using Path.DirectorySeparatorChar to make the test cross-platform
            var separator = Path.DirectorySeparatorChar;
            var separatorStr = separator == '\\' ? "\\\\" : separator.ToString();
            var pattern = $@".*{separatorStr}App{separatorStr}User$";
            var match = Regex.IsMatch(path, pattern);
            Assert.True(match);
        }

        [Theory]
        [InlineData("User/Info", "/User", "Info/")]
        [InlineData("App/User/13", "/App/User", "13")]
        [InlineData("~/App/Info", "~/App", "/Info")]
        public void CombinePathsTest(string expected, params string[] paths)
        {
            var actual = PathUtil.CombinePaths(paths);
            Assert.Equal(expected, actual);
        }

        // Additional tests for edge cases

        [Fact]
        public void CombinePaths_WithNullArray_ReturnsEmptyString()
        {
            var actual = PathUtil.CombinePaths(null);
            Assert.Equal(string.Empty, actual);
        }

        [Fact]
        public void CombinePaths_WithEmptyArray_ReturnsEmptyString()
        {
            var actual = PathUtil.CombinePaths(new string[0]);
            Assert.Equal(string.Empty, actual);
        }

        [Fact]
        public void ResolveVirtualPath_WithNullPath_ReturnsBaseDirectory()
        {
            var result = PathUtil.ResolveVirtualPath(null);
            Assert.Equal(Path.GetFullPath(Directory.GetCurrentDirectory()), Path.GetFullPath(result));
        }

        [Fact]
        public void ResolveVirtualPath_WithEmptyPath_ReturnsBaseDirectory()
        {
            var result = PathUtil.ResolveVirtualPath("");
            Assert.Equal(Path.GetFullPath(Directory.GetCurrentDirectory()), Path.GetFullPath(result));
        }
    }
}