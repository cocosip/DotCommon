using DotCommon.Extensions;
using System;
using System.Collections.Concurrent;

namespace DotCommon.Threading
{
    /// <summary>
    /// AmbientDataContextAmbientScopeProvider
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AmbientDataContextAmbientScopeProvider<T> : IAmbientScopeProvider<T>
    {

        private static readonly ConcurrentDictionary<string, ScopeItem> ScopeDictionary = new ConcurrentDictionary<string, ScopeItem>();

        private readonly IAmbientDataContext _dataContext;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="dataContext"></param>
        public AmbientDataContextAmbientScopeProvider(IAmbientDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <summary>
        /// 根据上下文Key取值
        /// </summary>
        /// <param name="contextKey">Key</param>
        /// <returns></returns>
        public T GetValue(string contextKey)
        {
            var item = GetCurrentItem(contextKey);
            if (item == null)
            {
                return default;
            }

            return item.Value;
        }

        /// <summary>
        /// 开始上下文生命周期
        /// </summary>
        /// <param name="contextKey">上下文Key</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public IDisposable BeginScope(string contextKey, T value)
        {
            var item = new ScopeItem(value, GetCurrentItem(contextKey));

            if (!ScopeDictionary.TryAdd(item.Id, item))
            {
                throw new Exception("Can not add item! ScopeDictionary.TryAdd returns false!");
            }

            _dataContext.SetData(contextKey, item.Id);

            return new DisposeAction(() =>
            {
                ScopeDictionary.TryRemove(item.Id, out item);

                if (item.Outer == null)
                {
                    _dataContext.SetData(contextKey, null);
                    return;
                }

                _dataContext.SetData(contextKey, item.Outer.Id);
            });
        }

        private ScopeItem GetCurrentItem(string contextKey)
        {
            return _dataContext.GetData(contextKey) is string objKey ? ScopeDictionary.GetOrDefault(objKey) : null;
        }

        private class ScopeItem
        {
            public string Id { get; }

            public ScopeItem Outer { get; }

            public T Value { get; }

            public ScopeItem(T value, ScopeItem outer = null)
            {
                Id = Guid.NewGuid().ToString();

                Value = value;
                Outer = outer;
            }
        }
    }
}
