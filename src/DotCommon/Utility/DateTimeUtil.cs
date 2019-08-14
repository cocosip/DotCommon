using System;
using System.Collections.Generic;
using System.Linq;

namespace DotCommon.Utility
{
    /// <summary>时间帮助类
    /// </summary>
    public static class DateTimeUtil
    {

        /// <summary> 将时间转换成int32类型
        /// </summary>
        public static int ToInt32(DateTime datetime)
        {
            //默认情况下以1970.01.01为开始时间计算
            var timeSpan = datetime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt32(timeSpan.TotalSeconds);
        }

        /// <summary> 将时间转换成long类型,以毫秒为单位
        /// </summary>
        public static long ToInt64(DateTime datetime)
        {
            //默认情况下以1970.01.01为开始时间计算
            var timeSpan = datetime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64(timeSpan.TotalMilliseconds);
        }

        /// <summary>将string类型的时间转换成int32
        /// </summary>
        public static int ToInt32(string datetime, int defaultValue = 0)
        {
            if (!RegexUtil.IsDataTime(datetime))
            {
                return defaultValue;
            }
            var end = Convert.ToDateTime(datetime);
            return ToInt32(end);
        }

        /// <summary> 将Int32类型的整数转换成时间
        /// </summary>
        public static DateTime ToDateTime(int seconds)
        {

            var begtime = Convert.ToInt64(seconds) * 10000000; //100毫微秒为单位
            var dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var tricks1970 = dt1970.Ticks; //1970年1月1日刻度
            var timeTricks = tricks1970 + begtime; //日志日期刻度
            var dt = new DateTime(timeTricks, DateTimeKind.Utc); //转化为DateTime
            return dt;
        }

        /// <summary>将long类型的整数时间(以毫秒为单位)转换成时间
        /// </summary>
        public static DateTime ToDateTime(long millSeconds)
        {
            var begtime = millSeconds * 10000; //100毫微秒为单位
            var dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var tricks1970 = dt1970.Ticks; //1970年1月1日刻度
            var timeTricks = tricks1970 + begtime; //日志日期刻度
            var dt = new DateTime(timeTricks, DateTimeKind.Utc); //转化为DateTime
            //DateTime enddt = dt.Date;//获取到日期整数
            return dt;
        }

        /// <summary>获取String类型的时间拼接,拼接到天
        /// </summary>
        public static string GetPadDay(DateTime time)
        {
            var month = time.Month.ToString().PadLeft(2, '0');
            var day = time.Day.ToString().PadLeft(2, '0');
            var pad = $"{time.Year}{month}{day}";
            return pad;
        }

        /// <summary>获取string类型拼接的时间 拼接到秒
        /// </summary>
        public static string GetPadSecond(DateTime time)
        {
            var month = time.Month.ToString().PadLeft(2, '0');
            var day = time.Day.ToString().PadLeft(2, '0');
            var hour = time.Hour.ToString().PadLeft(2, '0');
            var minute = time.Minute.ToString().PadLeft(2, '0');
            var second = time.Second.ToString().PadLeft(2, '0');
            var pad = $"{time.Year}{month}{day}{hour}{minute}{second}";
            return pad;
        }

        /// <summary> 获取string类型拼接的时间 拼接到秒,但是不包括最早的2位
        /// </summary>
        public static string GetPadSecondWithoutPrefix(DateTime time)
        {
            var month = time.Month.ToString().PadLeft(2, '0');
            var day = time.Day.ToString().PadLeft(2, '0');
            var hour = time.Hour.ToString().PadLeft(2, '0');
            var minute = time.Minute.ToString().PadLeft(2, '0');
            var second = time.Second.ToString().PadLeft(2, '0');
            var pad = $"{time.Year.ToString().Substring(2)}{month}{day}{hour}{minute}{second}";
            return pad;
        }

        /// <summary>获取到毫秒的拼接
        /// </summary>
        public static string GetPadMillSecond(DateTime time)
        {
            var month = time.Month.ToString().PadLeft(2, '0');
            var day = time.Day.ToString().PadLeft(2, '0');
            var hour = time.Hour.ToString().PadLeft(2, '0');
            var minute = time.Minute.ToString().PadLeft(2, '0');
            var second = time.Second.ToString().PadLeft(2, '0');
            var minSecond = time.Millisecond.ToString().PadLeft(3, '0');
            var pad = $"{time.Year}{month}{day}{hour}{minute}{second}{minSecond}";
            return pad;
        }

        /// <summary> 获取string类型拼接的时间 拼接到秒,但是不包括最早的2位,精确到毫秒
        /// </summary>
        public static string GetPadMillSecondWithoutPrefix(DateTime time)
        {
            var month = time.Month.ToString().PadLeft(2, '0');
            var day = time.Day.ToString().PadLeft(2, '0');
            var hour = time.Hour.ToString().PadLeft(2, '0');
            var minute = time.Minute.ToString().PadLeft(2, '0');
            var second = time.Second.ToString().PadLeft(2, '0');
            var minSecond = time.Millisecond.ToString().PadLeft(3, '0');
            var pad = $"{time.Year.ToString().Substring(2)}{month}{day}{hour}{minute}{second}{minSecond}";
            return pad;
        }

        /// <summary>获取两个时间之间经历的星期几
        /// </summary>
        public static string GetWeekCross(DateTime begin, DateTime end)
        {
            if (begin.Date > end)
            {
                return "";
            }
            var weekArray = new[] { 0, 1, 2, 3, 4, 5, 6 };

            var totalDays = (end.Date - begin.Date).TotalDays + 1;
            if (totalDays >= 7)
            {
                return string.Join(",", weekArray);
            }

            var target = new List<int>();
            var indexDate = begin.Date;
            for (var i = 0; i < totalDays; i++)
            {
                target.Add((int)indexDate.DayOfWeek);
                indexDate = indexDate.AddDays(1);
            }
            return string.Join(",", target);
        }

        /// <summary>获取某个时间的中文星期
        /// </summary>
        public static string GetChineseWeekOfDay(DateTime time)
        {
            var dayOfWeek = (int)time.DayOfWeek;
            return GetWeekDays().FirstOrDefault(x => x.Key == dayOfWeek).Value;
        }

        /// <summary>获取星期中的所有天数
        /// </summary>
        public static Dictionary<int, string> GetWeekDays()
        {
            var weekDict = new Dictionary<int, string>
            {
                {0, "星期日"},
                {1, "星期一"},
                {2, "星期二"},
                {3, "星期三"},
                {4, "星期四"},
                {5, "星期五"},
                {6, "星期六"}
            };
            return weekDict;
        }

        /// <summary>获取某时间点当月的第一天
        /// </summary>
        public static DateTime GetFirstDayOfMonth(DateTime time)
        {
            return new DateTime(time.Year, time.Month, 1);
        }

        /// <summary>获取某个时间点当月的最后一天
        /// </summary>
        public static DateTime GetLastDayOfMonth(DateTime time)
        {
            return new DateTime(time.Year, time.Month, 1).AddMonths(1).AddDays(-1);
        }

        /// <summary>获取某个时间点当年的第一天
        /// </summary>
        public static DateTime GetFirstDayOfYear(DateTime time)
        {
            return new DateTime(time.Year, 1, 1);
        }

        /// <summary>获取某个时间点当年的最后一天
        /// </summary>
        public static DateTime GetLastDayOfYear(DateTime time)
        {
            return new DateTime(time.Year + 1, 1, 1).AddDays(-1);
        }

        /// <summary>判断某个时间是否为当月的第一天
        /// </summary>
        public static bool IsFirstDayOfMonth(DateTime time)
        {
            var firstDay = GetFirstDayOfMonth(time);
            return firstDay.Date == time.Date;
        }

        /// <summary>判断某个时间是否为当月的最后一天
        /// </summary>
        public static bool IsLastDayOfMonth(DateTime time)
        {
            var lastDay = GetLastDayOfMonth(time);
            return lastDay.Date == time.Date;
        }

        /// <summary>判断某个时间是否为当年的第一天
        /// </summary>
        public static bool IsFirstDayOfYear(DateTime time)
        {
            var firstDay = GetFirstDayOfYear(time);
            return firstDay.Date == time.Date;
        }

        /// <summary>判断某个时间是否为当年的最后一天
        /// </summary>
        public static bool IsLastDayOfYear(DateTime time)
        {
            var lastDay = GetLastDayOfYear(time);
            return lastDay.Date == time.Date;
        }

        /// <summary>将时间的某个日期进行修改
        /// </summary>
        public static DateTime ReplaceDay(string day, DateTime datetime)
        {
            var fullTime = $"{datetime:yyyy-MM}-{day}";
            var date = Convert.ToDateTime(fullTime);
            return date;
        }

        /// <summary>将时间的日期后面的时间进行替换
        /// </summary>
        public static DateTime ReplaceTime(string time, DateTime datetime)
        {
            var fullTime = $"{datetime:yyyy-MM-dd} {time}";
            var date = Convert.ToDateTime(fullTime);
            return date;
        }

    }


}
