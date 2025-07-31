using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

#nullable enable

namespace DotCommon.Test.Utility
{
    public class PathUtilTest
    {
        [Theory]
        [InlineData("", null)]
        [InlineData("myfolder", null)]
        [InlineData("/absolute/path", "/absolute/path")]
        public void GetAbsolutePath_Test(string path, string? expectedRoot)
        {
            string result = PathUtil.GetAbsolutePath(path);
            if (expectedRoot == null)
            {
                Assert.StartsWith(AppContext.BaseDirectory, result);
                Assert.EndsWith(path, result);
            }
            else
            {
                Assert.Equal(expectedRoot, result);
            }
        }

        [Theory]
        [MemberData(nameof(CombinePathsTestData))]
        public void CombinePaths_Test(string[] paths, string expected)
        {
            string result = PathUtil.CombinePaths(paths);
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> CombinePathsTestData()
        {
            yield return new object[] { new string[] { "folder1", "folder2", "file.txt" }, Path.Combine("folder1", "folder2", "file.txt") };
            yield return new object[] { new string[] { Path.DirectorySeparatorChar + "root", "sub", "file.txt" }, Path.Combine(Path.DirectorySeparatorChar + "root", "sub", "file.txt") };
            yield return new object[] { new string[] { "" }, "" };
            yield return new object[] { new string[] { }, "" };
        }

        [Theory]
        [MemberData(nameof(GetAncestorDirectoryTestData))]
        public void GetAncestorDirectory_ValidCases_Test(string pathRaw, int layerCount, string expectedRaw)
        {
            string path = pathRaw.Replace('/', Path.DirectorySeparatorChar);
            string expected = expectedRaw.Replace('/', Path.DirectorySeparatorChar);
            string result = PathUtil.GetAncestorDirectory(path, layerCount);
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> GetAncestorDirectoryTestData()
        {
            // Unix-like paths
            yield return new object[] { "/a/b/c", 1, "/a/b" };
            yield return new object[] { "/a/b/c", 2, "/a" };
            yield return new object[] { "/a/b/c", 3, "/" };
            yield return new object[] { "/a/b/c", 0, "/a/b/c" };
            yield return new object[] { "/a/b/c", 10, "/" }; // Navigating beyond root

            // Windows paths
            yield return new object[] { "C:\\a\\b\\c", 1, "C:\\a\\b" };
            yield return new object[] { "C:\\a\\b\\c", 3, "C:\\" };
        }

        [Fact]
        public void GetAncestorDirectory_InvalidPath_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => PathUtil.GetAncestorDirectory("relative/path", 1));
        }

        [Fact]
        public void GetAncestorDirectory_NegativeLayerCount_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => PathUtil.GetAncestorDirectory("/absolute/path", -1));
        }

        [Theory]
        [InlineData("~/file.txt", null)]
        [InlineData("~/folder/file.txt", null)]
        [InlineData("relative/path.txt", null)]
        [InlineData("", null)]
        public void ResolveVirtualPath_Test(string virtualPath, string? expectedAbsolutePrefix)
        {
            string result = PathUtil.ResolveVirtualPath(virtualPath);
            if (expectedAbsolutePrefix == null)
            {
                Assert.StartsWith(AppContext.BaseDirectory, result);
            }
            else
            {
                Assert.StartsWith(expectedAbsolutePrefix, result);
            }
            Assert.True(Path.IsPathRooted(result));
        }

        [Theory]
        [InlineData("file.txt", ".txt")]
        [InlineData("archive.tar.gz", ".gz")]
        [InlineData("folder/file.jpg", ".jpg")]
        [InlineData("noextension", "")]
        [InlineData(".bashrc", ".bashrc")]
        [InlineData(null, null)]
        [InlineData("", "")]
        public void GetFileExtension_Test(string? path, string? expectedExtension)
        {
            string? result = PathUtil.GetFileExtension(path);
            Assert.Equal(expectedExtension, result);
        }
    }
}