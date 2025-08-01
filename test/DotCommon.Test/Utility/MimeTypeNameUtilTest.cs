using Xunit;
using DotCommon.Utility;

namespace DotCommon.Test.Utility
{
    public class MimeTypeNameUtilTest
    {
        [Theory]
        [InlineData("txt", "text/plain")]
        [InlineData(".JPG", "image/jpeg")]
        [InlineData("json", "application/json")]
        [InlineData("unknown_extension", "application/octet-stream")]
        [InlineData(null, "application/octet-stream")]
        [InlineData("", "application/octet-stream")]
        public void GetMimeName_ShouldReturnCorrectMimeType(string extension, string expectedMimeType)
        {
            var mimeType = MimeTypeNameUtil.GetMimeName(extension);
            Assert.Equal(expectedMimeType, mimeType);
        }

        [Theory]
        [InlineData("C:\\temp\\document.pdf", "application/pdf")]
        [InlineData("archive.zip", "application/zip")]
        [InlineData("photo.JPEG", "image/jpeg")]
        [InlineData("file_without_extension", "application/octet-stream")]
        public void GetMimeNameFromFile_ShouldReturnCorrectMimeType(string filePath, string expectedMimeType)
        {
            var mimeType = MimeTypeNameUtil.GetMimeNameFromFile(filePath);
            Assert.Equal(expectedMimeType, mimeType);
        }

        [Theory]
        [InlineData("image/png", ".png")]
        [InlineData("application/json", ".json")]
        [InlineData("TEXT/HTML", ".htm")]
        [InlineData("application/x-unknown-type", null)]
        [InlineData(null, null)]
        [InlineData("", null)]
        public void GetExtension_ShouldReturnCorrectExtension(string mimeType, string expectedExtension)
        {
            var extension = MimeTypeNameUtil.GetExtension(mimeType);
            Assert.Equal(expectedExtension, extension);
        }
    }
}