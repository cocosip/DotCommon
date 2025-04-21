using System;

namespace DotCommon.Timing
{
    public class DotCommonClockOptions
    {
        /// <summary>
        /// Default: <see cref="DateTimeKind.Unspecified"/>
        /// </summary>
        public DateTimeKind Kind { get; set; }

        public DotCommonClockOptions()
        {
            Kind = DateTimeKind.Unspecified;
        }
    }
}
