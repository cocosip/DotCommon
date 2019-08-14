using DotCommon.Utility;
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
            var actual = PathUtil.GetPathExtension(path);
            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(@"C:\A\B.txt", 1, @"C:\A")]
        [InlineData(@"D:\A\B\C", 1, @"D:\A\B")]
        [InlineData(@"D:\A\B\C\D", 3, @"D:\A")]
        [InlineData(@"D:", 1, @"D:")]
        [InlineData(@"A\B\C", 2, @"A\B\C")]
        public void BackDirectoryTest(string path, int layerCount, string expected)
        {
            var actual = PathUtil.BackDirectory(path, layerCount);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MapPath_Test()
        {
            var path = PathUtil.MapPath("../../Root");
            var match1 = Regex.IsMatch(path, @"^.*DotCommon\\test\\Root$");
            Assert.True(match1);
        }

        [Fact]
        public void LocateServerPath_Test()
        {
            var path = PathUtil.LocateServerPath(@"\App\User");
            var match1 = Regex.IsMatch(path, @"^.*\\App\\User$");
            Assert.True(match1);
        }


        [Theory]
        [InlineData("User/Info","/User","Info/")]
        [InlineData("App/User/13", "/App/User", "13")]
        [InlineData("~/App/Info", "~/App", "/Info")]
        public void CombineTest(string expected, params string[] paths)
        {
            var actual = PathUtil.CombineRelative(paths);
            Assert.Equal(expected, actual);
        }
    }
}
