using System.Threading;

namespace DotCommon.Timing
{
    public class CurrentTimezoneProvider : ICurrentTimezoneProvider
    {
        public string? TimeZone
        {
            get => _currentScope.Value;
            set => _currentScope.Value = value;
        }

        private readonly AsyncLocal<string?> _currentScope;

        public CurrentTimezoneProvider()
        {
            _currentScope = new AsyncLocal<string?>();
        }
    }
}
