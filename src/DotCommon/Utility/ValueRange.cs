using System;

namespace DotCommon.Utility
{
    /// <summary>数据区间值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValueRange<T> where T : IComparable<T>
    {
        /// <summary>最小值
        /// </summary>
        public T MinValue { get; set; }

        /// <summary>最大值
        /// </summary>
        public T MaxValue { get; set; }

        /// <summary>Ctor
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public ValueRange(T min, T max)
        {
            MinValue = min;
            MaxValue = max;
        }

        /// <summary>判断是否存在该值
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            return MinValue.CompareTo(value) <= 0 && MaxValue.CompareTo(value) >= 0;
        }

        /// <summary>Join
        /// </summary>
        /// <param name="value">值</param>
        public void Join(T value)
        {
            if (MinValue.CompareTo(value) > 0)
            {
                MinValue = value;
            }
            if (MaxValue.CompareTo(value) < 0)
            {
                MaxValue = value;
            }
        }

    }

    /// <summary>时间数据区间
    /// </summary>
    public class DateRange : ValueRange<DateTime>
    {
        /// <summary>Ctor
        /// </summary>
        public DateRange() : base(DateTime.MinValue, DateTime.MaxValue)
        {

        }

        /// <summary>Ctor
        /// </summary>
        /// <param name="min">最小时间</param>
        /// <param name="max">最大时间</param>
        public DateRange(DateTime min, DateTime max)
           : base(min, max)
        {
        }

        /// <summary>格式化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString("yyyyMMddHHmmss");
        }

        /// <summary>格式化
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string ToString(string format)
        {
            var value = (MinValue == DateTime.MinValue ? String.Empty : MinValue.ToString(format)) + "-"
                        + (MaxValue == DateTime.MaxValue ? String.Empty : MaxValue.ToString(format));
            if (value == "-") return String.Empty;
            return value;
        }
    }
}
