#if !NETSTANDARD2_0
using System.Runtime.Remoting.Messaging;

namespace DotCommon.Runtime.Remoting
{
    public class CallContextAmbientDataContext : IAmbientDataContext
    {
        public void SetData(string key, object value)
        {
            if (value == null)
            {
                CallContext.FreeNamedDataSlot(key);
                return;
            }

            CallContext.LogicalSetData(key, value);
        }

        public object GetData(string key)
        {
            return CallContext.LogicalGetData(key);
        }
    }
}
#endif