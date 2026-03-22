using System;
using System.Threading;
using DotCommon.Threading;
using Moq;
using Xunit;

namespace DotCommon.Test.Threading
{
    public class CancellationTokenOverrideTest
    {
        [Fact]
        public void Constructor_ShouldSetCancellationToken()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var overrideObj = new CancellationTokenOverride(token);

            Assert.Equal(token, overrideObj.CancellationToken);
        }

        [Fact]
        public void CancellationToken_ShouldBeCorrectToken()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var overrideObj = new CancellationTokenOverride(token);

            Assert.Equal(token, overrideObj.CancellationToken);
            Assert.False(overrideObj.CancellationToken.IsCancellationRequested);

            cts.Cancel();
            Assert.True(overrideObj.CancellationToken.IsCancellationRequested);
        }
    }

    public class CancellationTokenProviderExtensionsTest
    {
        [Fact]
        public void FallbackToProvider_WithValidToken_ShouldReturnPreferredValue()
        {
            var cts = new CancellationTokenSource();
            var mockProvider = new Mock<ICancellationTokenProvider>();
            mockProvider.Setup(x => x.Token).Returns(CancellationToken.None);

            var result = mockProvider.Object.FallbackToProvider(cts.Token);

            Assert.Equal(cts.Token, result);
        }

        [Fact]
        public void FallbackToProvider_WithDefaultToken_ShouldReturnProviderToken()
        {
            var cts = new CancellationTokenSource();
            var mockProvider = new Mock<ICancellationTokenProvider>();
            mockProvider.Setup(x => x.Token).Returns(cts.Token);

            var result = mockProvider.Object.FallbackToProvider(default);

            Assert.Equal(cts.Token, result);
        }

        [Fact]
        public void FallbackToProvider_WithNoneToken_ShouldReturnProviderToken()
        {
            var cts = new CancellationTokenSource();
            var mockProvider = new Mock<ICancellationTokenProvider>();
            mockProvider.Setup(x => x.Token).Returns(cts.Token);

            var result = mockProvider.Object.FallbackToProvider(CancellationToken.None);

            Assert.Equal(cts.Token, result);
        }
    }

public class NullCancellationTokenProviderTest
    {
        [Fact]
        public void Token_ShouldReturnNone()
        {
            var provider = NullCancellationTokenProvider.Instance;
            Assert.Equal(CancellationToken.None, provider.Token);
        }
    }

    public class CancellationTokenProviderBaseTest
    {
        [Fact]
        public void Use_ShouldBeginScope()
        {
            var scopeProvider = new TestAmbientScopeProvider<CancellationTokenOverride>();
            var testProvider = new TestCancellationTokenProvider(scopeProvider);
            var cts = new CancellationTokenSource();

            using (testProvider.Use(cts.Token))
            {
                Assert.True(scopeProvider.HasScope);
                Assert.Equal(cts.Token, scopeProvider.CurrentValue?.CancellationToken);
            }

            Assert.False(scopeProvider.HasScope);
        }

        [Fact]
        public void OverrideValue_ShouldReturnNullWhenNoScope()
        {
            var scopeProvider = new TestAmbientScopeProvider<CancellationTokenOverride>();
            var testProvider = new TestCancellationTokenProvider(scopeProvider);

            Assert.Null(testProvider.GetOverrideValue());
        }

        [Fact]
        public void OverrideValue_ShouldReturnValueWhenScopeExists()
        {
            var scopeProvider = new TestAmbientScopeProvider<CancellationTokenOverride>();
            var testProvider = new TestCancellationTokenProvider(scopeProvider);
            var cts = new CancellationTokenSource();

            using (testProvider.Use(cts.Token))
            {
                Assert.NotNull(testProvider.GetOverrideValue());
                Assert.Equal(cts.Token, testProvider.GetOverrideValue()?.CancellationToken);
            }
        }

        private class TestCancellationTokenProvider : CancellationTokenProviderBase
        {
            public TestCancellationTokenProvider(IAmbientScopeProvider<CancellationTokenOverride> scopeProvider)
                : base(scopeProvider)
            {
            }

            public override CancellationToken Token => OverrideValue != null ? OverrideValue.CancellationToken : CancellationToken.None;

            public CancellationTokenOverride GetOverrideValue()
            {
                return OverrideValue;
            }
        }
    }

    public class TestAmbientScopeProvider<T> : IAmbientScopeProvider<T>
        where T : class
    {
        private T _currentValue;
        private bool _hasScope;

        public bool HasScope => _hasScope;
        public T CurrentValue => _currentValue;

        public T GetValue(string contextKey)
        {
            return _currentValue;
        }

        public IDisposable BeginScope(string contextKey, T value)
        {
            _currentValue = value;
            _hasScope = true;
            return new TestDisposable(() =>
            {
                _currentValue = null;
                _hasScope = false;
            });
        }

        private class TestDisposable : IDisposable
        {
            private readonly Action _disposeAction;

            public TestDisposable(Action disposeAction)
            {
                _disposeAction = disposeAction;
            }

            public void Dispose()
            {
                _disposeAction();
            }
        }
    }
}