using System;

namespace DotCommon
{
    public class DotCommonException : Exception
    {
        public DotCommonException()
        {

        }

        public DotCommonException(string? message)
            : base(message)
        {

        }

        public DotCommonException(string? message, Exception? innerException)
            : base(message, innerException)
        {

        }
    }
}
