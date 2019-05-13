using System;

namespace DotCommon.Utility
{
    public class ValueRange<T> where T : IComparable<T>
    {
        public T MinValue { get; set; }
        public T MaxValue { get; set; }

        public ValueRange(T min, T max)
        {
            MinValue = min;
            MaxValue = max;
        }

        public bool Contains(T value)
        {
            return MinValue.CompareTo(value) <= 0 && MaxValue.CompareTo(value) >= 0;
        }

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


    public class DateRange : ValueRange<DateTime>
    {
        public DateRange() : base(DateTime.MinValue, DateTime.MaxValue)
        {

        }
        public DateRange(DateTime min, DateTime max)
           : base(min, max)
        {
        }

        public override string ToString()
        {
            return ToString("yyyyMMddHHmmss");
        }

        public string ToString(string format)
        {
            var value = (MinValue == DateTime.MinValue ? String.Empty : MinValue.ToString(format)) + "-"
                        + (MaxValue == DateTime.MaxValue ? String.Empty : MaxValue.ToString(format));
            if (value == "-") return String.Empty;
            return value;
        }
    }
}
