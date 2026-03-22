using System;
using DotCommon.IO;
using DotCommon.Utility;
using System.IO;
using Xunit;

namespace DotCommon.Test.IO
{
    public class DirectoryHelperTest
    {
        [Fact]
        public void CreateIfNotExists_Test()
        {
            var path = PathUtil.GetAbsolutePath();
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
        public void CreateIfNotExists_WithNullDirectory_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryHelper.CreateIfNotExists(null));
        }

        [Fact]
        public void CreateIfNotExists_WithEmptyDirectory_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => DirectoryHelper.CreateIfNotExists(string.Empty));
        }

        [Fact]
        public void DeleteIfExist_Test()
        {
            var path = PathUtil.GetAbsolutePath();
            var deletePath = Path.Combine(path, "to_delete\\");

            DirectoryHelper.CreateIfNotExists(deletePath);
            Assert.True(Directory.Exists(deletePath));

            DirectoryHelper.DeleteIfExist(deletePath);
            Assert.False(Directory.Exists(deletePath));

            DirectoryHelper.DeleteIfExist(deletePath);
        }

        [Fact]
        public void DeleteIfExist_WithNullDirectory_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryHelper.DeleteIfExist(null));
        }

        [Fact]
        public void DeleteIfExist_WithEmptyDirectory_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => DirectoryHelper.DeleteIfExist(string.Empty));
        }

        [Fact]
        public void DeleteIfExist_WithRecursive_ShouldDeleteDirectoryWithContents()
        {
            var path = PathUtil.GetAbsolutePath();
            var deletePath = Path.Combine(path, "recursive_delete\\");
            DirectoryHelper.CreateIfNotExists(deletePath);
            File.WriteAllText(Path.Combine(deletePath, "file.txt"), "content");
            DirectoryHelper.CreateIfNotExists(Path.Combine(deletePath, "subdir"));

            DirectoryHelper.DeleteIfExist(deletePath, true);
            Assert.False(Directory.Exists(deletePath));
        }

        [Fact]
        public void DirectoryCopy_Test()
        {

            var path = PathUtil.GetAbsolutePath();
            var sourcePath1 = Path.Combine(path, "source1\\");
            var sourcePath1_1 = Path.Combine(sourcePath1, "source1_1\\");
            DirectoryHelper.CreateIfNotExists(sourcePath1);
            DirectoryHelper.CreateIfNotExists(sourcePath1_1);
            var file1 = Path.Combine(sourcePath1, "1.txt");
            File.WriteAllText(file1, "sample");

            var sourcePath2 = Path.Combine(path, "source2\\");
            var sourcePath2_1 = Path.Combine(sourcePath2, "source1_1\\");
            var file2 = Path.Combine(sourcePath2, "1.txt");
            DirectoryHelper.DirectoryCopy(sourcePath1, sourcePath2);

            Assert.True(Directory.Exists(sourcePath2));
            Assert.True(Directory.Exists(sourcePath2_1));
            Assert.True(Directory.Exists(sourcePath2));
            Assert.True(File.Exists(file2));

            Directory.Delete(sourcePath1, true);
            Directory.Delete(sourcePath2, true);

        }

        [Fact]
        public void DirectoryCopy_WithNullSourceDir_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryHelper.DirectoryCopy(null, "target"));
        }

        [Fact]
        public void DirectoryCopy_WithNullTargetDir_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => DirectoryHelper.DirectoryCopy("source", null));
        }

        [Fact]
        public void DirectoryCopy_WithEmptySourceDir_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => DirectoryHelper.DirectoryCopy(string.Empty, "target"));
        }

        [Fact]
        public void DirectoryCopy_WithEmptyTargetDir_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => DirectoryHelper.DirectoryCopy("source", string.Empty));
        }

        [Fact]
        public void DirectoryCopy_WithNonExistingSource_ShouldThrowDirectoryNotFoundException()
        {
            var path = PathUtil.GetAbsolutePath();
            var sourceDir = Path.Combine(path, "non_existing_source_" + Guid.NewGuid());
            var targetDir = Path.Combine(path, "target_" + Guid.NewGuid());

            Assert.Throws<DirectoryNotFoundException>(() => DirectoryHelper.DirectoryCopy(sourceDir, targetDir));
        }

        [Fact]
        public void DirectoryCopy_WithOverwrite_ShouldOverwriteExistingFile()
        {
            var path = PathUtil.GetAbsolutePath();
            var sourceDir = Path.Combine(path, "overwrite_source_" + Guid.NewGuid());
            var targetDir = Path.Combine(path, "overwrite_target_" + Guid.NewGuid());

            DirectoryHelper.CreateIfNotExists(sourceDir);
            File.WriteAllText(Path.Combine(sourceDir, "file.txt"), "new content");

            DirectoryHelper.CreateIfNotExists(targetDir);
            File.WriteAllText(Path.Combine(targetDir, "file.txt"), "old content");

            DirectoryHelper.DirectoryCopy(sourceDir, targetDir);

            Assert.Equal("new content", File.ReadAllText(Path.Combine(targetDir, "file.txt")));

            Directory.Delete(sourceDir, true);
            Directory.Delete(targetDir, true);
        }
    }
}
