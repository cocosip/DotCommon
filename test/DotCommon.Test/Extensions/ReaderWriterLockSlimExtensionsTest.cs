using System;
using System.Threading;
using Moq;
using Xunit;

namespace DotCommon.Test.Extensions
{
    public class ReaderWriterLockSlimExtensionsTest
    {
        private readonly Mock<Action> _mockAction;
        private readonly Mock<Func<int>> _mockFunc;
        public ReaderWriterLockSlimExtensionsTest()
        {
            _mockAction = new Mock<Action>();
            _mockFunc = new Mock<Func<int>>();
        }

        [Fact]
        public void AtomRead_Test()
        {
            ReaderWriterLockSlim readerWriterLockSlim = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                readerWriterLockSlim.AtomRead(() => { });
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                readerWriterLockSlim.AtomRead<int>(() => 1);
            });


            readerWriterLockSlim = new ReaderWriterLockSlim();
            Assert.Throws<ArgumentNullException>(() =>
            {
                readerWriterLockSlim.AtomRead(null);
            });

            Func<int> func1 = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                readerWriterLockSlim.AtomRead(func1);
            });

            readerWriterLockSlim.AtomRead(_mockAction.Object);
            _mockAction.Verify(x => x.Invoke(), Times.Once);

            readerWriterLockSlim.AtomRead(_mockFunc.Object);
            _mockFunc.Verify(x => x.Invoke(), Times.Once);

        }

        [Fact]
        public void AtomWrite_Test()
        {
            ReaderWriterLockSlim readerWriterLockSlim = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                readerWriterLockSlim.AtomWrite(() => { });
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                readerWriterLockSlim.AtomWrite<int>(() => 1);
            });


            readerWriterLockSlim = new ReaderWriterLockSlim();
            Assert.Throws<ArgumentNullException>(() =>
            {
                readerWriterLockSlim.AtomWrite(null);
            });

            Func<int> func1 = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                readerWriterLockSlim.AtomWrite(func1);
            });

            readerWriterLockSlim.AtomWrite(_mockAction.Object);
            _mockAction.Verify(x => x.Invoke(), Times.Once);

            readerWriterLockSlim.AtomWrite(_mockFunc.Object);
            _mockFunc.Verify(x => x.Invoke(), Times.Once);
        }

    }
}
