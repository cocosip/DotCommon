using System;

namespace DotCommon.Utility
{
    /**
     * Twitter_Snowflake
     * SnowFlake的结构如下(每部分用-分开):
     * 0 - 0000000000 0000000000 0000000000 0000000000 0 - 00000 - 00000 - 000000000000
     * 1位标识，由于long基本类型在Java中是带符号的，最高位是符号位，正数是0，负数是1，所以id一般是正数，最高位是0
     * 41位时间截(毫秒级)，注意，41位时间截不是存储当前时间的时间截，而是存储时间截的差值（当前时间截 - 开始时间截)
     * 得到的值），这里的的开始时间截，一般是我们的id生成器开始使用的时间，由我们程序来指定的（如下下面程序IdWorker类的startTime属性）。41位的时间截，可以使用69年，年T = (1L << 41) / (1000L * 60 * 60 * 24 * 365) = 69
     * 10位的数据机器位，可以部署在1024个节点，包括5位datacenterId和5位workerId<br>
     * 12位序列，毫秒内的计数，12位的计数顺序号支持每个节点每毫秒(同一机器，同一时间截)产生4096个ID序号
     * 加起来刚好64位，为一个Long型
     * SnowFlake的优点是，整体上按照时间自增排序，并且整个分布式系统内不会产生ID碰撞(由数据中心ID和机器ID作区分)，并且效率较高，经测试，SnowFlake每秒能够产生26万ID左右。
     */
    public class SnowflakeDistributeId
    {

        //开始时间截(2015-01-01)
        private static long twepoch = 1420041600000L;

        //机器id所占的位数
        private static int workerIdBits = 5;

        //数据标识id所占的位数
        private static int datacenterIdBits = 5;

        //支持的最大机器id，结果是31 (这个移位算法可以很快的计算出几位二进制数所能表示的最大十进制数)
        private static long maxWorkerId = -1L ^ (-1L << workerIdBits);

        //支持的最大数据标识id，结果是31
        private static long maxDatacenterId = -1L ^ (-1L << datacenterIdBits);

        //序列在id中占的位数
        private static int sequenceBits = 12;

        //机器ID向左移12位
        private static int workerIdShift = sequenceBits;

        //数据标识id向左移17位(12+5)
        private static int datacenterIdShift = sequenceBits + workerIdBits;

        //时间截向左移22位(5+5+12)
        private static int timestampLeftShift = sequenceBits + workerIdBits + datacenterIdBits;

        // 生成序列的掩码，这里为4095 (0b111111111111=0xfff=4095)
        private static long sequenceMask = -1L ^ (-1L << sequenceBits);

        // 工作机器ID(0~31)
        private long workerId;

        //数据中心ID(0~31)
        private long datacenterId;

        //毫秒内序列(0~4095)
        private long sequence = 0L;

        //上次生成ID的时间截
        private long lastTimestamp = -1L;

        private readonly object syncObject = new object();


        /// <summary>Ctor
        /// </summary>
        public SnowflakeDistributeId() : this(0L, 0L)
        {

        }

        /// <summary>Ctor
        /// </summary>
        /// <param name="workerId">工作ID (0~31)</param>
        public SnowflakeDistributeId(long workerId) : this(workerId, 0L)
        {

        }

        /// <summary>Ctor
        /// </summary>
        /// <param name="workerId">工作ID (0~31)</param>
        /// <param name="datacenterId">数据中心ID (0~31)</param>
        public SnowflakeDistributeId(long workerId, long datacenterId)
        {
            if (workerId > maxWorkerId || workerId < 0)
            {
                throw new ArgumentException(string.Format("worker Id can't be greater than %d or less than 0", maxWorkerId));
            }
            if (datacenterId > maxDatacenterId || datacenterId < 0)
            {
                throw new ArgumentException(string.Format("datacenter Id can't be greater than %d or less than 0", maxDatacenterId));
            }
            this.workerId = workerId;
            this.datacenterId = datacenterId;
        }

        /// <summary>获得下一个ID (该方法是线程安全的)
        /// </summary>
        public long NextId()
        {
            lock (syncObject)
            {
                long timestamp = TimeGen();

                //如果当前时间小于上一次ID生成的时间戳，说明系统时钟回退过这个时候应当抛出异常
                if (timestamp < lastTimestamp)
                {
                    throw new InvalidTimeZoneException(
                            string.Format("Clock moved backwards.  Refusing to generate id for %d milliseconds", lastTimestamp - timestamp));
                }

                //如果是同一时间生成的，则进行毫秒内序列
                if (lastTimestamp == timestamp)
                {
                    sequence = (sequence + 1) & sequenceMask;
                    //毫秒内序列溢出
                    if (sequence == 0)
                    {
                        //阻塞到下一个毫秒,获得新的时间戳
                        timestamp = TilNextMillis(lastTimestamp);
                    }
                }
                //时间戳改变，毫秒内序列重置
                else
                {
                    sequence = 0L;
                }

                //上次生成ID的时间截
                lastTimestamp = timestamp;

                //移位并通过或运算拼到一起组成64位的ID
                return ((timestamp - twepoch) << timestampLeftShift) //
                        | (datacenterId << datacenterIdShift) //
                        | (workerId << workerIdShift) //
                        | sequence;
            }
        }




        /// <summary>阻塞到下一个毫秒，直到获得新的时间戳
        /// </summary>
        private long TilNextMillis(long lastTimestamp)
        {
            long timestamp = TimeGen();
            while (timestamp <= lastTimestamp)
            {
                timestamp = TimeGen();
            }
            return timestamp;
        }

        /// <summary>返回以毫秒为单位的当前时间
        /// </summary>
        protected long TimeGen()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        /// <summary>获得下一个ID,使用默认的0L,OL作为workerId与datacenterId
        /// </summary>
        public static long GenerateNextId()
        {
            return new SnowflakeDistributeId().NextId();
        }

    }
}
