using DotCommon.IO;
using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace DotCommon.Test.IO
{
    public class DirectoryHelperTest
    {
        [Fact]
        public void CreateIfNotExists_Test()
        {
            var path = PathUtil.LocateServerPath();
            var createPath1 = Path.Combine(path, "test01\\");

            DirectoryHelper.CreateIfNotExists(createPath1);
            Assert.True(Directory.Exists(createPath1));
            DirectoryHelper.CreateIfNotExists(createPath1);

            Directory.Delete(createPath1);

            var filePath1 = Path.Combine(path, "1.text");
            File.WriteAllText(filePath1, "haha");
            Assert.True(File.Exists(filePath1));
            FileHelper.DeleteIfExists(filePath1);
            Assert.False(File.Exists(filePath1));

        }

        [Fact]
        public void DirectoryCopy_Test()
        {

            var path = PathUtil.LocateServerPath();
            var sourcePath1 = Path.Combine(path, "source1\\");
            var sourcePath1_1 = Path.Combine(sourcePath1, "source1_1\\");
            DirectoryHelper.CreateIfNotExists(sourcePath1);
            DirectoryHelper.CreateIfNotExists(sourcePath1_1);
            var file1 = Path.Combine(sourcePath1, "1.txt");
            File.WriteAllText(file1, "sample");

            var sourcePath2 = Path.Combine(path, "source2\\");
            var sourcePath2_1 = Path.Combine(sourcePath2, "source1_1\\");
            var file2= Path.Combine(sourcePath2, "1.txt");
            DirectoryHelper.DirectoryCopy(sourcePath1, sourcePath2);

            Assert.True(Directory.Exists(sourcePath2));
            Assert.True(Directory.Exists(sourcePath2_1));
            Assert.True(Directory.Exists(sourcePath2));
            Assert.True(File.Exists(file2));

            Directory.Delete(sourcePath1,true);
            Directory.Delete(sourcePath2,true);

        }
    }
}
