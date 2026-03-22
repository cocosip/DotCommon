using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Tests
{
    public class StreamExtensionsTest
    {
        [Fact]
        public void GetAllBytes_FromMemoryStream_ShouldReturnBytes()
        {
            var expected = Encoding.UTF8.GetBytes("Hello World");
            using var stream = new MemoryStream(expected);
            var result = stream.GetAllBytes();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetAllBytes_FromRegularStream_ShouldReturnBytes()
        {
            var expected = Encoding.UTF8.GetBytes("Hello World");
            using var stream = new NonSeekableStream(expected);
            var result = stream.GetAllBytes();
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetAllBytesAsync_FromMemoryStream_ShouldReturnBytes()
        {
            var expected = Encoding.UTF8.GetBytes("Hello World");
            using var stream = new MemoryStream(expected);
            var result = await stream.GetAllBytesAsync();
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetAllBytesAsync_FromRegularStream_ShouldReturnBytes()
        {
            var expected = Encoding.UTF8.GetBytes("Hello World");
            using var stream = new NonSeekableStream(expected);
            var result = await stream.GetAllBytesAsync();
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetAllBytesAsync_WithCancellationToken_ShouldReturnBytes()
        {
            var expected = Encoding.UTF8.GetBytes("Hello World");
            using var stream = new MemoryStream(expected);
            var result = await stream.GetAllBytesAsync(CancellationToken.None);
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task CopyToAsync_ShouldCopyStream()
        {
            var data = Encoding.UTF8.GetBytes("Hello World");
            using var source = new MemoryStream(data);
            using var destination = new MemoryStream();
            await source.CopyToAsync(destination, CancellationToken.None);

            var result = destination.ToArray();
            Assert.Equal(data, result);
        }

        [Fact]
        public async Task CreateMemoryStreamAsync_ShouldCreateMemoryStream()
        {
            var data = Encoding.UTF8.GetBytes("Hello World");
            using var source = new MemoryStream(data);
            source.Position = 5;

            using var result = await source.CreateMemoryStreamAsync();

            Assert.Equal(0, result.Position);
            Assert.Equal(data, result.ToArray());
            Assert.Equal(0, source.Position);
        }

        [Fact]
        public void CreateMemoryStream_ShouldCreateMemoryStream()
        {
            var data = Encoding.UTF8.GetBytes("Hello World");
            using var source = new MemoryStream(data);
            source.Position = 5;

            using var result = source.CreateMemoryStream();

            Assert.Equal(0, result.Position);
            Assert.Equal(data, result.ToArray());
            Assert.Equal(0, source.Position);
        }

        [Fact]
        public void CreateMemoryStream_NonSeekableStream_ShouldWork()
        {
            var data = Encoding.UTF8.GetBytes("Hello World");
            using var source = new NonSeekableStream(data);

            using var result = source.CreateMemoryStream();

            Assert.Equal(data, result.ToArray());
        }

        private class NonSeekableStream : Stream
        {
            private readonly MemoryStream _inner;

            public NonSeekableStream(byte[] data)
            {
                _inner = new MemoryStream(data);
            }

            public override bool CanRead => _inner.CanRead;
            public override bool CanSeek => false;
            public override bool CanWrite => false;
            public override long Length => _inner.Length;
            public override long Position
            {
                get => _inner.Position;
                set => _inner.Position = value;
            }

            public override void Flush() => _inner.Flush();
            public override int Read(byte[] buffer, int offset, int count) => _inner.Read(buffer, offset, count);
            public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
            public override void SetLength(long value) => throw new NotSupportedException();
            public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        }
    }
}