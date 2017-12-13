using DotCommon.Utility;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class PathUtilTest
    {
        [Theory]
        [InlineData(@"C:\A\B.txt", ".txt")]
        [InlineData(@"\BB.jpg", ".jpg")]
        public void GetPathExtensionTest(string path, string expected)
        {
            var actual = PathUtil.GetPathExtension(path);
            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(@"C:\A\B.txt", 1, @"C:\A")]
        [InlineData(@"D:\A\B\C", 1, @"D:\A\B")]
        [InlineData(@"D:\A\B\C\D", 3, @"D:\A")]
        public void BackDirectoryTest(string path, int layerCount, string expected)
        {
            var actual = PathUtil.BackDirectory(path, layerCount);
            Assert.Equal(expected, actual);
        }

        //[Theory]
        //[InlineData(@"C:\A\B\C", "C:", "A", "B", "C")]
        //[InlineData(@"C:\A\Path1\Path2\x.txt", @"C:\A", "Path1", @"\Path2\", @"x.txt")]
        //[InlineData(@"C:\A\X\Y\x.txt", @"C:\A\", @"X\", @"\Y\", @"x.txt")]
        //public void CombineTest(string expected, params string[] paths)
        //{
        //    var actual = PathUtil.Combine(paths);
        //    Assert.Equal(expected, actual);
        //}
    }
}
