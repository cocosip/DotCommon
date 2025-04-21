using System;
using System.Threading;

namespace DotCommon.Threading
{
    public abstract class CancellationTokenProviderBase : ICancellationTokenProvider
    {
        public const string CancellationTokenOverrideContextKey = "DotCommon.Threading.CancellationToken.Override";

        public abstract CancellationToken Token { get; }

        protected IAmbientScopeProvider<CancellationTokenOverride> CancellationTokenOverrideScopeProvider { get; }

        protected CancellationTokenOverride? OverrideValue => CancellationTokenOverrideScopeProvider.GetValue(CancellationTokenOverrideContextKey);

        protected CancellationTokenProviderBase(IAmbientScopeProvider<CancellationTokenOverride> cancellationTokenOverrideScopeProvider)
        {
            CancellationTokenOverrideScopeProvider = cancellationTokenOverrideScopeProvider;
        }

        public IDisposable Use(CancellationToken cancellationToken)
        {
            return CancellationTokenOverrideScopeProvider.BeginScope(CancellationTokenOverrideContextKey, new CancellationTokenOverride(cancellationToken));
        }
    }
}
