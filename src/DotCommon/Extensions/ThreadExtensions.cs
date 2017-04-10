#if !NET45
using System;
using System.Reflection;
using System.Threading;

namespace DotCommon.Extensions
{
    public class ThreadExtensions
    {
        /// <summary>线程Abort扩展方法
        /// </summary>
        public static void Abort(Thread thread)
        {
            MethodInfo abort = null;
            foreach (var m in thread.GetType().GetTypeInfo().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (m.Name.Equals("AbortInternal") && m.GetParameters().Length == 0) abort = m;
            }
            if (abort == null)
            {
                throw new Exception("Failed to get Thread.Abort method");
            }
            abort.Invoke(thread, new object[0]);
        }
    }
}
#endif