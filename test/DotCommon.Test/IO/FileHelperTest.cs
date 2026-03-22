using System;
using System.IO;
using DotCommon.IO;
using Xunit;

namespace DotCommon.Test.IO
{
    public class FileHelperTest
    {
        private readonly string _testDirectory;

        public FileHelperTest()
        {
            _testDirectory = Path.Combine(Path.GetTempPath(), "DotCommon_FileHelper_Test");
            if (!Directory.Exists(_testDirectory))
            {
                Directory.CreateDirectory(_testDirectory);
            }
        }

        [Fact]
        public void DeleteIfExists_WithNullFileName_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => FileHelper.DeleteIfExists(null));
        }

        [Fact]
        public void DeleteIfExists_WithEmptyFileName_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => FileHelper.DeleteIfExists(string.Empty));
        }

        [Fact]
        public void DeleteIfExists_WithExistingFile_ShouldDeleteFile()
        {
            var filePath = Path.Combine(_testDirectory, "test_delete.txt");
            File.WriteAllText(filePath, "test content");

            Assert.True(File.Exists(filePath));

            FileHelper.DeleteIfExists(filePath);

            Assert.False(File.Exists(filePath));
        }

        [Fact]
        public void DeleteIfExists_WithNonExistingFile_ShouldNotThrow()
        {
            var filePath = Path.Combine(_testDirectory, "non_existing_file.txt");

            Assert.False(File.Exists(filePath));

            var exception = Record.Exception(() => FileHelper.DeleteIfExists(filePath));
            Assert.Null(exception);
        }

        [Fact]
        public void GetFileSize_WithNullFileName_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => FileHelper.GetFileSize(null));
        }

        [Fact]
        public void GetFileSize_WithEmptyFileName_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => FileHelper.GetFileSize(string.Empty));
        }

        [Fact]
        public void GetFileSize_WithExistingFile_ShouldReturnCorrectSize()
        {
            var filePath = Path.Combine(_testDirectory, "test_size.txt");
            var content = "test content for size";
            File.WriteAllText(filePath, content);

            var size = FileHelper.GetFileSize(filePath);

            Assert.Equal(content.Length, size);

            File.Delete(filePath);
        }

        [Fact]
        public void GetFileSize_WithNonExistingFile_ShouldThrowFileNotFoundException()
        {
            var filePath = Path.Combine(_testDirectory, "non_existing_size.txt");

            Assert.Throws<FileNotFoundException>(() => FileHelper.GetFileSize(filePath));
        }

        [Fact]
        public void GetFileSize_WithEmptyFile_ShouldReturnZero()
        {
            var filePath = Path.Combine(_testDirectory, "empty_file.txt");
            File.WriteAllText(filePath, string.Empty);

            var size = FileHelper.GetFileSize(filePath);

            Assert.Equal(0, size);

            File.Delete(filePath);
        }
    }
}