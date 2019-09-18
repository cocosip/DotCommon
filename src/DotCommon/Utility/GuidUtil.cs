using System;

namespace DotCommon.Utility
{
    /// <summary>Guid工具类
    /// </summary>
    public static class GuidUtil
    {
        private static readonly long EpochMilliseconds = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks / 10000L;

        /// <summary>根据SqlServer数据库主键排序规则生成Guid,能当主键进行排序
        /// </summary>
        public static Guid NewSequentialGuid()
        {
            // This code was not reviewed to guarantee uniqueness under most conditions, nor completely optimize for avoiding
            // page splits in SQL Server when doing inserts from multiple hosts, so do not re-use in production systems.
            var guidBytes = Guid.NewGuid().ToByteArray();

            // get the milliseconds since Jan 1 1970
            byte[] sequential = BitConverter.GetBytes(DateTime.Now.Ticks / 10000L - EpochMilliseconds);

            // discard the 2 most significant bytes, as we only care about the milliseconds increasing, but the highest ones 
            // should be 0 for several thousand years to come (non-issue).
            if (BitConverter.IsLittleEndian)
            {
                guidBytes[10] = sequential[5];
                guidBytes[11] = sequential[4];
                guidBytes[12] = sequential[3];
                guidBytes[13] = sequential[2];
                guidBytes[14] = sequential[1];
                guidBytes[15] = sequential[0];
            }
            else
            {
                Buffer.BlockCopy(sequential, 2, guidBytes, 10, 6);
            }

            return new Guid(guidBytes);
        }


        /// <summary>获取新的Guid,Format example: D , N
        /// </summary>
        /// <param name="format">Guid格式,D或者N格式</param>
        /// <returns></returns>
        public static string NewGuidString(string format = "D")
        {
            return Guid.NewGuid().ToString(format);
        }


    }
}
