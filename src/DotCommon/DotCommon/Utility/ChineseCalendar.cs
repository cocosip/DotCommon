using System;

namespace DotCommon.Utility
{
    #region ChineseCalendarException

    /// <summary>
    /// Represents an exception specific to Chinese calendar operations.
    /// </summary>
    public class ChineseCalendarException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChineseCalendarException"/> class with a specified error message.
        /// </summary>
        /// <param name="msg">The message that describes the error.</param>
        public ChineseCalendarException(string msg)
            : base(msg)
        {
        }
    }

    #endregion

    /// <summary>
    /// Represents the Chinese Lunar Calendar. Version V1.0 supports data from January 31, 1900, to December 31, 2049.
    /// This program uses data sourced from online perpetual calendars and other combined data.
    /// </summary>
    public class ChineseCalendar
    {
        #region Internal Structures

        /// <summary>
        /// Represents a solar holiday with its month, day, recess duration, and name.
        /// </summary>
        private struct SolarHolidayStruct
        {
            /// <summary>The month of the holiday.</summary>
            public readonly int Month;
            /// <summary>The day of the holiday.</summary>
            public readonly int Day;
            private readonly int _recess; // Duration of the holiday recess
            /// <summary>The name of the holiday.</summary>
            public readonly string HolidayName;

            /// <summary>
            /// Initializes a new instance of the <see cref="SolarHolidayStruct"/>.
            /// </summary>
            /// <param name="month">The month of the holiday.</param>
            /// <param name="day">The day of the holiday.</param>
            /// <param name="recess">The duration of the holiday recess.</param>
            /// <param name="holidayName">The name of the holiday.</param>
            public SolarHolidayStruct(int month, int day, int recess, string holidayName)
            {
                Month = month;
                Day = day;
                _recess = recess;
                HolidayName = holidayName;
            }
        }

        /// <summary>
        /// Represents a lunar holiday with its month, day, recess duration, and name.
        /// </summary>
        private struct LunarHolidayStruct
        {
            /// <summary>The month of the holiday.</summary>
            public readonly int Month;
            /// <summary>The day of the holiday.</summary>
            public readonly int Day;
            private readonly int _recess;
            /// <summary>The name of the holiday.</summary>
            public readonly string HolidayName;

            /// <summary>
            /// Initializes a new instance of the <see cref="LunarHolidayStruct"/>.
            /// </summary>
            /// <param name="month">The month of the holiday.</param>
            /// <param name="day">The day of the holiday.</param>
            /// <param name="recess">The duration of the holiday recess.</param>
            /// <param name="holidayName">The name of the holiday.</param>
            public LunarHolidayStruct(int month, int day, int recess, string holidayName)
            {
                Month = month;
                Day = day;
                _recess = recess;
                HolidayName = holidayName;
            }
        }

        /// <summary>
        /// Represents a holiday defined by its month, week of the month, day of the week, and name.
        /// </summary>
        private struct WeekHolidayStruct
        {
            /// <summary>The month of the holiday.</summary>
            public readonly int Month;
            /// <summary>The week number within the month (e.g., 2 for the second week).</summary>
            public readonly int WeekAtMonth;
            /// <summary>The day of the week (e.g., 1 for Monday, 7 for Sunday).</summary>
            public readonly int WeekDay;
            /// <summary>The name of the holiday.</summary>
            public readonly string HolidayName;

            /// <summary>
            /// Initializes a new instance of the <see cref="WeekHolidayStruct"/>.
            /// </summary>
            /// <param name="month">The month of the holiday.</param>
            /// <param name="weekAtMonth">The week number within the month.</param>
            /// <param name="weekDay">The day of the week.</param>
            /// <param name="name">The name of the holiday.</param>
            public WeekHolidayStruct(int month, int weekAtMonth, int weekDay, string name)
            {
                Month = month;
                WeekAtMonth = weekAtMonth;
                WeekDay = weekDay;
                HolidayName = name;
            }
        }

        #endregion

        #region Internal Variables

        private DateTime _date;
        private readonly DateTime _datetime;

        private readonly int _cYear;
        private readonly int _cMonth; // Lunar month
        private readonly int _cDay; // Day of the lunar month
        private readonly bool _isLeapMonth; // Indicates if the current month is a leap month
        private readonly bool _isLeapYear; // Indicates if the current lunar year has a leap month

        #endregion

        #region Basic Data

        #region Basic Constants

        private const int MinYear = 1900;
        private const int MaxYear = 2050;
        private static DateTime _minDay = new DateTime(1900, 1, 30);
        private static readonly DateTime MaxDay = new DateTime(2049, 12, 31);
        private const int GanZhiStartYear = 1864; // Start year for GanZhi calculation
        private static readonly DateTime GanZhiStartDay = new DateTime(1899, 12, 22); // Start day for GanZhi calculation
        private const string ChineseNum = "零一二三四五六七八九";
        private const int AnimalStartYear = 1900; // 1900 is the year of the Rat
        private static readonly DateTime ChineseConstellationReferDay = new DateTime(2007, 9, 13); // Reference value for 28 constellations, this day is Jiao

        #endregion

        #region Lunar Calendar Data

        /// <summary>
        /// Lunar calendar data sourced from the internet
        /// </summary>
        /// <remarks>
        /// Data structure: 17 bits total
        /// Bit 17: Leap month days, 0 = 29 days, 1 = 30 days
        /// Bits 16-5 (12 bits): Represent 12 months, bit 16 is the first month, 1 = 30 days, 0 = 29 days
        /// Bits 4-1 (4 bits): Which month is the leap month, 0 if no leap month in that year
        ///</remarks>
        private static readonly int[] LunarDateArray = {
            0x04BD8, 0x04AE0, 0x0A570, 0x054D5, 0x0D260, 0x0D950, 0x16554, 0x056A0, 0x09AD0, 0x055D2,
            0x04AE0, 0x0A5B6, 0x0A4D0, 0x0D250, 0x1D255, 0x0B540, 0x0D6A0, 0x0ADA2, 0x095B0, 0x14977,
            0x04970, 0x0A4B0, 0x0B4B5, 0x06A50, 0x06D40, 0x1AB54, 0x02B60, 0x09570, 0x052F2, 0x04970,
            0x06566, 0x0D4A0, 0x0EA50, 0x06E95, 0x05AD0, 0x02B60, 0x186E3, 0x092E0, 0x1C8D7, 0x0C950,
            0x0D4A0, 0x1D8A6, 0x0B550, 0x056A0, 0x1A5B4, 0x025D0, 0x092D0, 0x0D2B2, 0x0A950, 0x0B557,
            0x06CA0, 0x0B550, 0x15355, 0x04DA0, 0x0A5B0, 0x14573, 0x052B0, 0x0A9A8, 0x0E950, 0x06AA0,
            0x0AEA6, 0x0AB50, 0x04B60, 0x0AAE4, 0x0A570, 0x05260, 0x0F263, 0x0D950, 0x05B57, 0x056A0,
            0x096D0, 0x04DD5, 0x04AD0, 0x0A4D0, 0x0D4D4, 0x0D250, 0x0D558, 0x0B540, 0x0B6A0, 0x195A6,
            0x095B0, 0x049B0, 0x0A974, 0x0A4B0, 0x0B27A, 0x06A50, 0x06D40, 0x0AF46, 0x0AB60, 0x09570,
            0x04AF5, 0x04970, 0x064B0, 0x074A3, 0x0EA50, 0x06B58, 0x055C0, 0x0AB60, 0x096D5, 0x092E0,
            0x0C960, 0x0D954, 0x0D4A0, 0x0DA50, 0x07552, 0x056A0, 0x0ABB7, 0x025D0, 0x092D0, 0x0CAB5,
            0x0A950, 0x0B4A0, 0x0BAA4, 0x0AD50, 0x055D9, 0x04BA0, 0x0A5B0, 0x15176, 0x052B0, 0x0A930,
            0x07954, 0x06AA0, 0x0AD50, 0x05B52, 0x04B60, 0x0A6E6, 0x0A4E0, 0x0D260, 0x0EA65, 0x0D530,
            0x05AA0, 0x076A3, 0x096D0, 0x04BD7, 0x04AD0, 0x0A4D0, 0x1D0B6, 0x0D250, 0x0D520, 0x0DD45,
            0x0B5A0, 0x056D0, 0x055B2, 0x049B0, 0x0A577, 0x0A4B0, 0x0AA50, 0x1B255, 0x06D20, 0x0ADA0,
            0x14B63
        };

        #endregion

        #region Constellation Names

        private static readonly string[] ConstellationName =
        {
            "白羊座", "金牛座", "双子座",
            "巨蟹座", "狮子座", "处女座",
            "天秤座", "天蝎座", "射手座",
            "摩羯座", "水瓶座", "双鱼座"
        };

        #endregion


        #region Twenty-Eight Constellations

        private static readonly string[] ChineseConstellationName =
        {
            // Thu      Fri     Sat        Sun       Mon     Tue     Wed
            "角木蛟", "亢金龙", "女土蝠", "房日兔", "心月狐", "尾火虎", "箕水豹",
            "斗木獬", "牛金牛", "氐土貉", "虚日鼠", "危月燕", "室火猪", "壁水獝",
            "奎木狼", "娄金狗", "胃土彘", "昴日鸡", "毕月乌", "觜火猴", "参水猿",
            "井木犴", "鬼金羊", "柳土獐", "星日马", "张月鹿", "翼火蛇", "轸水蚓"
        };

        #endregion

        #region Solar Terms Data

        private static readonly string[] SolarTerm =
        {
            "小寒", "大寒", "立春", "雨水", "惊蛰", "春分", "清明", "谷雨", "立夏", "小满", "芒种",
            "夏至", "小暑", "大暑", "立秋", "处暑", "白露", "秋分", "寒露", "霜降", "立冬", "小雪", "大雪", "冬至"
        };

        private static readonly int[] STermInfo =
        {
            0, 21208, 42467, 63836, 85337, 107014, 128867, 150921, 173149, 195551,
            218072, 240693, 263343, 285989, 308563, 331033, 353350, 375494, 397447, 419210, 440795, 462224, 483532,
            504758
        };

        #endregion

        #region Lunar Calendar Related Data

        private static readonly string GanStr = "甲乙丙丁戊己庚辛壬癸";
        private static readonly string ZhiStr = "子丑寅卯辰巳午未申酉戌亥";
        private static readonly string AnimalStr = "鼠牛虎兔龙蛇马羊猴鸡狗猪";
        private static readonly string NStr1 = "日一二三四五六七八九";
        private static readonly string NStr2 = "初十廿卅";

        private static readonly string[] MonthString =
        {
            "出错", "正月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "腊月"
        };

        #endregion

        #region Solar Calendar Holidays

        private static readonly SolarHolidayStruct[] SolaryHolidayInfo =
        {
            new SolarHolidayStruct(1, 1, 1, "元旦"),
            new SolarHolidayStruct(2, 2, 0, "世界湿地日"),
            new SolarHolidayStruct(2, 10, 0, "国际气象节"),
            new SolarHolidayStruct(2, 14, 0, "情人节"),
            new SolarHolidayStruct(3, 1, 0, "国际海豹日"),
            new SolarHolidayStruct(3, 5, 0, "学雷锋纪念日"),
            new SolarHolidayStruct(3, 8, 0, "妇女节"),
            new SolarHolidayStruct(3, 12, 0, "植树节 孙中山逝世纪念日"),
            new SolarHolidayStruct(3, 14, 0, "国际警察日"),
            new SolarHolidayStruct(3, 15, 0, "消费者权益日"),
            new SolarHolidayStruct(3, 17, 0, "中国国医节 国际航海日"),
            new SolarHolidayStruct(3, 21, 0, "世界森林日 消除种族歧视国际日 世界儿歌日"),
            new SolarHolidayStruct(3, 22, 0, "世界水日"),
            new SolarHolidayStruct(3, 24, 0, "世界防治结核病日"),
            new SolarHolidayStruct(4, 1, 0, "愚人节"),
            new SolarHolidayStruct(4, 7, 0, "世界卫生日"),
            new SolarHolidayStruct(4, 22, 0, "世界地球日"),
            new SolarHolidayStruct(5, 1, 1, "劳动节"),
            new SolarHolidayStruct(5, 2, 1, "劳动节假日"),
            new SolarHolidayStruct(5, 3, 1, "劳动节假日"),
            new SolarHolidayStruct(5, 4, 0, "青年节"),
            new SolarHolidayStruct(5, 8, 0, "世界红十字日"),
            new SolarHolidayStruct(5, 12, 0, "国际护士节"),
            new SolarHolidayStruct(5, 31, 0, "世界无烟日"),
            new SolarHolidayStruct(6, 1, 0, "国际儿童节"),
            new SolarHolidayStruct(6, 5, 0, "世界环境保护日"),
            new SolarHolidayStruct(6, 26, 0, "国际禁毒日"),
            new SolarHolidayStruct(7, 1, 0, "建党节 香港回归纪念 世界建筑日"),
            new SolarHolidayStruct(7, 11, 0, "世界人口日"),
            new SolarHolidayStruct(8, 1, 0, "建军节"),
            new SolarHolidayStruct(8, 8, 0, "中国男子节 父亲节"),
            new SolarHolidayStruct(8, 15, 0, "抗日战争胜利纪念"),
            new SolarHolidayStruct(9, 9, 0, "  逝世纪念"),
            new SolarHolidayStruct(9, 10, 0, "教师节"),
            new SolarHolidayStruct(9, 18, 0, "九·一八事变纪念日"),
            new SolarHolidayStruct(9, 20, 0, "国际爱牙日"),
            new SolarHolidayStruct(9, 27, 0, "世界旅游日"),
            new SolarHolidayStruct(9, 28, 0, "孔子诞辰"),
            new SolarHolidayStruct(10, 1, 1, "国庆节 国际音乐日"),
            new SolarHolidayStruct(10, 2, 1, "国庆节假日"),
            new SolarHolidayStruct(10, 3, 1, "国庆节假日"),
            new SolarHolidayStruct(10, 6, 0, "老人节"),
            new SolarHolidayStruct(10, 24, 0, "联合国日"),
            new SolarHolidayStruct(11, 10, 0, "世界青年节"),
            new SolarHolidayStruct(11, 12, 0, "孙中山诞辰纪念"),
            new SolarHolidayStruct(12, 1, 0, "世界艾滋病日"),
            new SolarHolidayStruct(12, 3, 0, "世界残疾人日"),
            new SolarHolidayStruct(12, 20, 0, "澳门回归纪念"),
            new SolarHolidayStruct(12, 24, 0, "平安夜"),
            new SolarHolidayStruct(12, 25, 0, "圣诞节"),
            new SolarHolidayStruct(12, 26, 0, " 诞辰纪念")
        };

        #endregion

        #region Lunar Calendar Holidays

        private static readonly LunarHolidayStruct[] LunarHolidayInfo =
        {
            new LunarHolidayStruct(1, 1, 1, "春节"),
            new LunarHolidayStruct(1, 15, 0, "元宵节"),
            new LunarHolidayStruct(5, 5, 0, "端午节"),
            new LunarHolidayStruct(7, 7, 0, "七夕情人节"),
            new LunarHolidayStruct(7, 15, 0, "中元节 盂兰盆节"),
            new LunarHolidayStruct(8, 15, 0, "中秋节"),
            new LunarHolidayStruct(9, 9, 0, "重阳节"),
            new LunarHolidayStruct(12, 8, 0, "腊八节"),
            new LunarHolidayStruct(12, 23, 0, "北方小年(扫房)"),
            new LunarHolidayStruct(12, 24, 0, "南方小年(掸尘)"),
            // Note: Chinese New Year's Eve needs to be calculated by other methods
        };

        #endregion

        #region Holidays by Week of Month

        private static readonly WeekHolidayStruct[] WeekHolidayInfo =
        {
            new WeekHolidayStruct(5, 2, 1, "母亲节"),
            new WeekHolidayStruct(5, 3, 1, "全国助残日"),
            new WeekHolidayStruct(6, 3, 1, "父亲节"),
            new WeekHolidayStruct(9, 3, 3, "国际和平日"),
            new WeekHolidayStruct(9, 4, 1, "国际聋人节"),
            new WeekHolidayStruct(10, 1, 2, "国际住房日"),
            new WeekHolidayStruct(10, 1, 4, "国际减轻自然灾害日"),
            new WeekHolidayStruct(11, 4, 5, "感恩节")
        };

        #endregion

        #endregion

        #region Constructor

        #region ChineseCalendar - Initialize with Solar Date

        /// <summary>
        /// Initialize with a standard solar calendar date
        /// </summary>
        public ChineseCalendar(DateTime date)
        {
            int i;
            CheckDateLimit(date);
            _date = date.Date;
            _datetime = date;
            // Lunar date calculation part
            var temp = 0;
            var ts = _date - _minDay; // Calculate the difference in days between 1900 and the current day
            var offset = ts.Days;
            for (i = MinYear; i <= MaxYear; i++)
            {
                temp = GetChineseYearDays(i); // Get the number of days in the lunar year
                if (offset - temp < 1)
                {
                    break;
                }
                offset = offset - temp;
            }
            _cYear = i;

            var leap = GetChineseLeapMonth(_cYear);
            // Set whether the current year has a leap month
            _isLeapYear = leap > 0;
            _isLeapMonth = false;
            for (i = 1; i <= 12; i++)
            {
                // Leap month
                if ((leap > 0) && (i == leap + 1) && (_isLeapMonth == false))
                {
                    _isLeapMonth = true;
                    i = i - 1;
                    temp = GetChineseLeapMonthDays(_cYear); // Calculate leap month days
                }
                else
                {
                    _isLeapMonth = false;
                    temp = GetChineseMonthDays(_cYear, i); // Calculate non-leap month days
                }

                offset = offset - temp;
                if (offset <= 0) break;
            }

            offset = offset + temp;
            _cMonth = i;
            _cDay = offset;
        }

        #endregion

        #region ChineseCalendar - Initialize with Lunar Date

        /// <summary>
        /// Initialize with a lunar calendar date
        /// </summary>
        /// <param name="cy">Lunar year</param>
        /// <param name="cm">Lunar month</param>
        /// <param name="cd">Lunar day</param>
        /// <param name="leapMonthFlag">Leap month flag</param>
        public ChineseCalendar(int cy, int cm, int cd, bool leapMonthFlag)
        {
            int i, temp;

            CheckChineseDateLimit(cy, cm, cd, leapMonthFlag);

            _cYear = cy;
            _cMonth = cm;
            _cDay = cd;

            var offset = 0;

            for (i = MinYear; i < cy; i++)
            {
                temp = GetChineseYearDays(i); // Get the number of days in the lunar year
                offset = offset + temp;
            }

            var leap = GetChineseLeapMonth(cy);
            _isLeapYear = leap != 0;

            _isLeapMonth = cm == leap && leapMonthFlag;


            if ((_isLeapYear == false) || // No leap month in the current year
                (cm < leap)) // Calculated month is less than the leap month
            {
                #region ...

                for (i = 1; i < cm; i++)
                {
                    temp = GetChineseMonthDays(cy, i); // Calculate non-leap month days
                    offset = offset + temp;
                }

                // Check if date exceeds maximum days
                if (cd > GetChineseMonthDays(cy, cm))
                {
                    throw new ChineseCalendarException("不合法的农历日期");
                }
                offset = offset + cd; // Add days of the current month

                #endregion
            }
            else // It's a leap year, and the calculated month is greater than or equal to the leap month
            {
                #region ...

                for (i = 1; i < cm; i++)
                {
                    temp = GetChineseMonthDays(cy, i); // Calculate non-leap month days
                    offset = offset + temp;
                }

                if (cm > leap) // Calculated month is greater than the leap month
                {
                    temp = GetChineseLeapMonthDays(cy); // Calculate leap month days
                    offset = offset + temp; // Add leap month days

                    if (cd > GetChineseMonthDays(cy, cm))
                    {
                        throw new ChineseCalendarException("不合法的农历日期");
                    }
                    offset = offset + cd;
                }
                else // Calculated month equals the leap month
                {
                    // If calculating for a leap month, first add the days of the corresponding normal month
                    if (_isLeapMonth) // The calculated month is a leap month
                    {
                        temp = GetChineseMonthDays(cy, cm); // Calculate non-leap month days
                        offset = offset + temp;
                    }

                    if (cd > GetChineseLeapMonthDays(cy))
                    {
                        throw new ChineseCalendarException("不合法的农历日期");
                    }
                    offset = offset + cd;
                }

                #endregion
            }


            _date = _minDay.AddDays(offset);
        }

        #endregion

        #endregion

        #region Private Methods

        #region GetChineseMonthDays

        // Returns the total number of days in lunar year y, month m
        private int GetChineseMonthDays(int year, int month)
        {
            //0X0FFFF[0000 {1111 1111 1111} 1111]
            if (BitTest32((LunarDateArray[year - MinYear] & 0x0000FFFF), (16 - month)))
            {
                return 30;
            }
            return 29;
        }

        #endregion

        #region GetChineseLeapMonth

        // Returns which month is the leap month in lunar year y (1-12), returns 0 if no leap month
        private int GetChineseLeapMonth(int year)
        {
            // The last 4 bits represent the leap month of the year, 0 means no leap. The first 4 bits work with the last 4 bits
            return LunarDateArray[year - MinYear] & 0xF;

        }

        #endregion

        #region GetChineseLeapMonthDays

        // Returns the number of days in the leap month of lunar year y
        private int GetChineseLeapMonthDays(int year)
        {
            if (GetChineseLeapMonth(year) != 0)
            {
                // The first 4 bits are meaningful only in a leap year, representing whether the leap month is a big or small month
                if ((LunarDateArray[year - MinYear] & 0x10000) != 0)
                {
                    // 1 means leap month is a big month (30 days)
                    return 30;
                }
                else
                {
                    // 0 means leap month is a small month (29 days)
                    return 29;
                }
            }
            else
            {
                return 0;
            }
        }

        #endregion

        #region GetChineseYearDays

        /// <summary>
        /// Get the number of days in a lunar year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        private int GetChineseYearDays(int year)
        {
            int i, f, sumDay, info;

            sumDay = 348; // 29 days X 12 months
            i = 0x8000;
            info = LunarDateArray[year - MinYear] & 0x0FFFF;
            // 0x04BD8 & 0x0FFFF: middle 12 bits, each bit represents a month, 1 = big month, 0 = small month
            // Count how many months have 30 days
            for (int m = 0; m < 12; m++)
            {
                f = info & i; // 0x04BD8  & 0x0FFFF  & 0x8000[1000 0000 0000 0000]
                if (f != 0)
                {
                    sumDay++;
                }
                i = i >> 1;
            }
            return sumDay + GetChineseLeapMonthDays(year);
        }

        #endregion

        #region GetChineseHour

        /// <summary>
        /// Get the Chinese hour (ShiChen) for the current time
        /// </summary>
        /// <returns></returns>
        private string GetChineseHour(DateTime dt)
        {
            //string ganHour, zhiHour;

            // Calculate the earthly branch of the hour
            var hour = dt.Hour;
            var minute = dt.Minute;
            if (minute != 0)
            {
                hour += 1;
            }
            var offset = hour / 2;
            if (offset >= 12) offset = 0;
            //zhiHour = ZhiStr[offset].ToString();

            // Calculate the heavenly stem
            TimeSpan ts = _date - GanZhiStartDay;
            var i = ts.Days % 60;

            var indexGan = ((i % 10 + 1) * 2 - 1) % 10 - 1;
            var tmpGan = GanStr.Substring(indexGan) + GanStr.Substring(0, indexGan + 2);
            //ganHour = GanStr[((i % 10 + 1) * 2 - 1) % 10 - 1].ToString();

            return tmpGan[offset] + ZhiStr[offset].ToString();

        }

        #endregion

        #region CheckDateLimit

        /// <summary> Checks if the solar date is valid
        /// </summary>
        private void CheckDateLimit(DateTime date)
        {
            if ((date < _minDay) || (date > MaxDay))
            {
                throw new ChineseCalendarException("超出可转换的日期");
            }
        }

        #endregion

        #region CheckChineseDateLimit

        /// <summary> Checks if the lunar date is valid
        /// </summary>
        private void CheckChineseDateLimit(int year, int month, int day, bool leapMonth)
        {
            if ((year < MinYear || year > MaxYear) || (month < 1 || month > 12))
            {
                throw new ChineseCalendarException("非法农历日期");
            }
            if ((day < 1) || (day > 30)) // Chinese months have at most 30 days
            {
                throw new ChineseCalendarException("非法农历日期");
            }

            int leap = GetChineseLeapMonth(year); // Calculate which month should be the leap month in that year
            if (leapMonth && (month != leap))
            {
                throw new ChineseCalendarException("非法农历日期");
            }
        }

        #endregion

        #region ConvertNumToChineseNum

        /// <summary>
        /// Convert 0-9 to Chinese numeral form
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private string ConvertNumToChineseNum(char n)
        {
            if ((n < '0') || (n > '9')) return "";
            switch (n)
            {
                case '0':
                    return ChineseNum[0].ToString();
                case '1':
                    return ChineseNum[1].ToString();
                case '2':
                    return ChineseNum[2].ToString();
                case '3':
                    return ChineseNum[3].ToString();
                case '4':
                    return ChineseNum[4].ToString();
                case '5':
                    return ChineseNum[5].ToString();
                case '6':
                    return ChineseNum[6].ToString();
                case '7':
                    return ChineseNum[7].ToString();
                case '8':
                    return ChineseNum[8].ToString();
                case '9':
                    return ChineseNum[9].ToString();
                default:
                    return "";
            }
        }

        #endregion

        #region BitTest32

        /// <summary>
        /// Test if a specific bit is set
        /// </summary>
        /// <param name="num"></param>
        /// <param name="bitpostion"></param>
        /// <returns></returns>
        private bool BitTest32(int num, int bitpostion)
        {

            if ((bitpostion > 31) || (bitpostion < 0))
                throw new Exception("Error Param: bitpostion[0-31]:" + bitpostion.ToString());

            int bit = 1 << bitpostion;

            if ((num & bit) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region ConvertDayOfWeek

        /// <summary>
        /// Convert DayOfWeek to numeric representation
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        private int ConvertDayOfWeek(DayOfWeek dayOfWeek)
        {
            return ((int)dayOfWeek) + 1;
        }

        #endregion

        #region CompareWeekDayHoliday

        /// <summary>
        /// Compare if the date matches the specified week and day of week
        /// </summary>
        /// <param name="date"></param>
        /// <param name="month"></param>
        /// <param name="week"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        private bool CompareWeekDayHoliday(DateTime date, int month, int week, int day)
        {
            bool ret = false;

            if (date.Month == month) // Same month
            {
                if (ConvertDayOfWeek(date.DayOfWeek) == day) // Same day of week
                {
                    DateTime firstDay = new DateTime(date.Year, date.Month, 1); // Generate the first day of the month
                    int i = ConvertDayOfWeek(firstDay.DayOfWeek);
                    int firWeekDays = 7 - ConvertDayOfWeek(firstDay.DayOfWeek) + 1; // Calculate remaining days in the first week

                    if (i > day)
                    {
                        if ((week - 1) * 7 + day + firWeekDays == date.Day)
                        {
                            ret = true;
                        }
                    }
                    else
                    {
                        if (day + firWeekDays + (week - 2) * 7 == date.Day)
                        {
                            ret = true;
                        }
                    }
                }
            }

            return ret;
        }

        #endregion

        #endregion

        #region Properties

        #region Holidays

        #region ChineseCalendarHoliday

        /// <summary>
        /// Calculate Chinese lunar calendar holidays
        /// </summary>
        public string ChineseCalendarHoliday
        {
            get
            {
                string tempStr = "";
                if (_isLeapMonth == false) // Leap months don't have holidays
                {
                    foreach (LunarHolidayStruct lh in LunarHolidayInfo)
                    {
                        if ((lh.Month == _cMonth) && (lh.Day == _cDay))
                        {

                            tempStr = lh.HolidayName;
                            break;

                        }
                    }

                    // Special handling for Chinese New Year's Eve
                    if (_cMonth == 12)
                    {
                        int i = GetChineseMonthDays(_cYear, 12); // Calculate total days in lunar December of the year
                        if (_cDay == i) // If it's the last day
                        {
                            tempStr = "除夕";
                        }
                    }
                }
                return tempStr;
            }
        }

        #endregion

        #region WeekDayHoliday

        /// <summary>
        /// Holidays calculated by the nth week and day of week in a month
        /// </summary>
        public string WeekDayHoliday
        {
            get
            {
                string tempStr = "";
                foreach (WeekHolidayStruct wh in WeekHolidayInfo)
                {
                    if (CompareWeekDayHoliday(_date, wh.Month, wh.WeekAtMonth, wh.WeekDay))
                    {
                        tempStr = wh.HolidayName;
                        break;
                    }
                }
                return tempStr;
            }
        }

        #endregion

        #region DateHoliday

        /// <summary>
        /// Holidays calculated by solar calendar date
        /// </summary>
        public string DateHoliday
        {
            get
            {
                string tempStr = "";

                foreach (SolarHolidayStruct sh in SolaryHolidayInfo)
                {
                    if ((sh.Month == _date.Month) && (sh.Day == _date.Day))
                    {
                        tempStr = sh.HolidayName;
                        break;
                    }
                }
                return tempStr;
            }
        }

        #endregion

        #endregion

        #region Solar Calendar Date

        #region Date

        /// <summary>
        /// Get the corresponding solar calendar date
        /// </summary>
        public DateTime Date
        {
            get { return _date; }
        }

        #endregion

        #region WeekDay

        /// <summary>
        /// Get the day of week
        /// </summary>
        public DayOfWeek WeekDay
        {
            get { return _date.DayOfWeek; }
        }

        #endregion

        #region WeekDayStr

        /// <summary>
        /// Get the day of week as a Chinese string
        /// </summary>
        public string WeekDayStr
        {
            get
            {
                switch (_date.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        return "星期日";
                    case DayOfWeek.Monday:
                        return "星期一";
                    case DayOfWeek.Tuesday:
                        return "星期二";
                    case DayOfWeek.Wednesday:
                        return "星期三";
                    case DayOfWeek.Thursday:
                        return "星期四";
                    case DayOfWeek.Friday:
                        return "星期五";
                    default:
                        return "星期六";
                }
            }
        }

        #endregion

        #region DateString

        /// <summary>
        /// Solar calendar date in Chinese representation, e.g., 一九九七年七月一日
        /// </summary>
        public string DateString => "公元" + _date.ToString("yyyy-MM-dd HH:mm:ss");

        #endregion

        #region IsLeapYear

        /// <summary>
        /// Whether the current year is a solar leap year
        /// </summary>
        public bool IsLeapYear
        {
            get { return DateTime.IsLeapYear(_date.Year); }
        }

        #endregion

        #region ChineseConstellation

        /// <summary>
        /// Twenty-eight constellations calculation
        /// </summary>
        public string ChineseConstellation
        {
            get
            {
                TimeSpan ts = _date - ChineseConstellationReferDay;
                var offset = ts.Days;
                var modStarDay = offset % 28;
                return (modStarDay >= 0
                    ? ChineseConstellationName[modStarDay]
                    : ChineseConstellationName[27 + modStarDay]);
            }
        }

        #endregion

        #region ChineseHour

        /// <summary>
        /// Chinese hour (ShiChen)
        /// </summary>
        public string ChineseHour
        {
            get { return GetChineseHour(_datetime); }
        }

        #endregion

        #endregion

        #region Lunar Calendar Date

        #region IsChineseLeapMonth

        /// <summary>
        /// Whether it is a leap month
        /// </summary>
        public bool IsChineseLeapMonth
        {
            get { return _isLeapMonth; }
        }

        #endregion

        #region IsChineseLeapYear

        /// <summary>
        /// Whether the current year has a leap month
        /// </summary>
        public bool IsChineseLeapYear
        {
            get { return _isLeapYear; }
        }

        #endregion

        #region ChineseDay

        /// <summary>
        /// Lunar calendar day
        /// </summary>
        public int ChineseDay
        {
            get { return _cDay; }
        }

        #endregion

        #region ChineseDayString

        /// <summary>
        /// Lunar calendar day in Chinese representation
        /// </summary>
        public string ChineseDayString
        {
            get
            {
                switch (_cDay)
                {
                    //case 0:
                    //    return "";
                    case 10:
                        return "初十";
                    case 20:
                        return "二十";
                    case 30:
                        return "三十";
                    default:
                        return NStr2[_cDay / 10] + NStr1[_cDay % 10].ToString();

                }
            }
        }

        #endregion

        #region ChineseMonth

        /// <summary>
        /// Lunar calendar month
        /// </summary>
        public int ChineseMonth
        {
            get { return _cMonth; }
        }

        #endregion

        #region ChineseMonthString

        /// <summary>
        /// Lunar calendar month string
        /// </summary>
        public string ChineseMonthString
        {
            get { return MonthString[_cMonth]; }
        }

        #endregion

        #region ChineseYear

        /// <summary>
        /// Get the lunar calendar year
        /// </summary>
        public int ChineseYear
        {
            get { return _cYear; }
        }

        #endregion

        #region ChineseYearString

        /// <summary>
        /// Get the lunar calendar year string, e.g., 一九九七年
        /// </summary>
        public string ChineseYearString
        {
            get
            {
                string tempStr = "";
                string num = _cYear.ToString();
                for (int i = 0; i < 4; i++)
                {
                    tempStr += ConvertNumToChineseNum(num[i]);
                }
                return tempStr + "年";
            }
        }

        #endregion

        #region ChineseDateString

        /// <summary>
        /// Get the lunar calendar date representation: 农历一九九七年正月初五
        /// </summary>
        public string ChineseDateString
        {
            get
            {
                if (_isLeapMonth)
                {
                    return "农历" + ChineseYearString + "闰" + ChineseMonthString + ChineseDayString;
                }
                return "农历" + ChineseYearString + ChineseMonthString + ChineseDayString;
            }
        }

        #endregion

        #region ChineseTwentyFourDay

        /// <summary>
        /// Calculate the twenty-four solar terms using the fixed position method. Solar terms are calculated based on Earth's orbit, not the lunar calendar.
        /// </summary>
        /// <remarks>
        /// There are two methods for defining solar terms. The ancient calendar used "constant qi" (恒气), 
        /// dividing the year into 24 equal parts by time, each solar term averaging slightly more than 15 days, 
        /// also known as "flat qi" (平气). Modern lunar calendar uses "fixed qi" (定气), 
        /// based on Earth's position in its orbit, 360° per cycle, with 15° between each solar term. 
        /// Since Earth is near perihelion during winter solstice and moves faster, the sun moves 15° 
        /// on the ecliptic in less than 15 days. The opposite occurs around summer solstice, where a solar term 
        /// can last up to 16 days. Using fixed qi ensures spring and autumn equinoxes fall on the days 
        /// with equal day and night lengths.
        /// </remarks>
        public string ChineseTwentyFourDay
        {
            get
            {
                DateTime baseDateAndTime = new DateTime(1900, 1, 6, 2, 5, 0); //#1/6/1900 2:05:00 AM#
                string tempStr = "";

                var y = _date.Year;

                for (int i = 1; i <= 24; i++)
                {
                    var num = 525948.76 * (y - 1900) + STermInfo[i - 1];

                    var newDate = baseDateAndTime.AddMinutes(num);
                    if (newDate.DayOfYear == _date.DayOfYear)
                    {
                        tempStr = SolarTerm[i - 1];
                        break;
                    }
                }
                return tempStr;
            }
        }

        /// <summary>The most recent solar term before the current date
        /// </summary>
        public string ChineseTwentyFourPrevDay
        {
            get
            {
                DateTime baseDateAndTime = new DateTime(1900, 1, 6, 2, 5, 0); //#1/6/1900 2:05:00 AM#
                int y;
                string tempStr = "";

                y = _date.Year;

                for (int i = 24; i >= 1; i--)
                {
                    var num = 525948.76 * (y - 1900) + STermInfo[i - 1];

                    var newDate = baseDateAndTime.AddMinutes(num);

                    if (newDate.DayOfYear < _date.DayOfYear)
                    {
                        tempStr = string.Format("{0}[{1}]", SolarTerm[i - 1], newDate.ToString("yyyy-MM-dd"));
                        break;
                    }
                }

                return tempStr;
            }

        }

        /// <summary>The next solar term after the current date
        /// </summary>
        public string ChineseTwentyFourNextDay
        {
            get
            {
                DateTime baseDateAndTime = new DateTime(1900, 1, 6, 2, 5, 0); //#1/6/1900 2:05:00 AM#
                DateTime newDate;
                double num;
                int y;
                string tempStr = "";

                y = _date.Year;

                for (int i = 1; i <= 24; i++)
                {
                    num = 525948.76 * (y - 1900) + STermInfo[i - 1];

                    newDate = baseDateAndTime.AddMinutes(num); // Calculate by minutes

                    if (newDate.DayOfYear > _date.DayOfYear)
                    {
                        tempStr = string.Format("{0}[{1}]", SolarTerm[i - 1], newDate.ToString("yyyy-MM-dd"));
                        break;
                    }
                }
                return tempStr;
            }

        }

        #endregion

        #endregion

        #region Constellation

        #region Constellation

        /// <summary>
        /// Calculate the constellation index for a specified date
        /// </summary>
        /// <returns></returns>
        public string Constellation
        {
            get
            {
                int index = 0;
                var m = _date.Month;
                var d = _date.Day;
                var y = m * 100 + d;

                if (((y >= 321) && (y <= 419)))
                {
                    index = 0;
                }
                else if ((y >= 420) && (y <= 520))
                {
                    index = 1;
                }
                else if ((y >= 521) && (y <= 620))
                {
                    index = 2;
                }
                else if ((y >= 621) && (y <= 722))
                {
                    index = 3;
                }
                else if ((y >= 723) && (y <= 822))
                {
                    index = 4;
                }
                else if ((y >= 823) && (y <= 922))
                {
                    index = 5;
                }
                else if ((y >= 923) && (y <= 1022))
                {
                    index = 6;
                }
                else if ((y >= 1023) && (y <= 1121))
                {
                    index = 7;
                }
                else if ((y >= 1122) && (y <= 1221))
                {
                    index = 8;
                }
                else if ((y >= 1222) || (y <= 119))
                {
                    index = 9;
                }
                else if ((y >= 120) && (y <= 218))
                {
                    index = 10;
                }
                else if ((y >= 219) && (y <= 320))
                {
                    index = 11;
                }


                return ConstellationName[index];
            }
        }

        #endregion

        #endregion

        #region Zodiac Animal

        #region Animal

        /// <summary>
        /// Calculate the zodiac animal index. Note: Although zodiac animals are distinguished by lunar year, 
        /// they are currently calculated by solar year in practical use. Rat year is 1, others follow.
        /// </summary>
        public int Animal
        {
            get
            {
                //int offset = _date.Year - AnimalStartYear; // Solar calendar calculation
                int offset = this._cYear - AnimalStartYear; // Lunar calendar calculation
                return (offset % 12) + 1;
            }
        }

        #endregion

        #region AnimalString

        /// <summary>
        /// Get the zodiac animal string
        /// </summary>
        public string AnimalString
        {
            get
            {
                //int offset = _date.Year - AnimalStartYear; // Solar calendar calculation
                int offset = this._cYear - AnimalStartYear; // Lunar calendar calculation
                return AnimalStr[offset % 12].ToString();
            }
        }

        #endregion

        #endregion

        #region GanZhi (Heavenly Stems and Earthly Branches)

        #region GanZhiYearString

        /// <summary>
        /// Get the GanZhi representation of the lunar year, e.g., 乙丑年
        /// </summary>
        public string GanZhiYearString
        {
            get
            {
                int i = (_cYear - GanZhiStartYear) % 60; // Calculate GanZhi
                var tempStr = GanStr[i % 10] + ZhiStr[i % 12].ToString() + "年";
                return tempStr;
            }
        }

        #endregion

        #region GanZhiMonthString

        /// <summary>
        /// Get the GanZhi representation of the month. Note: Leap months in the lunar calendar do not have GanZhi.
        /// </summary>
        public string GanZhiMonthString
        {
            get
            {
                // The earthly branch of each month is fixed, always starting from Yin month
                int zhiIndex;
                if (_cMonth > 10)
                {
                    zhiIndex = _cMonth - 10;
                }
                else
                {
                    zhiIndex = _cMonth + 2;
                }
                var zhi = ZhiStr[zhiIndex - 1].ToString();

                // Calculate the first heavenly stem of the month based on the heavenly stem of the current GanZhi year
                int ganIndex = 1;
                string gan;
                int i = (_cYear - GanZhiStartYear) % 60; // Calculate GanZhi
                switch (i % 10)
                {
                    #region ...

                    case 0: // 甲 (Jia)
                        ganIndex = 3;
                        break;
                    case 1: // 乙 (Yi)
                        ganIndex = 5;
                        break;
                    case 2: // 丙 (Bing)
                        ganIndex = 7;
                        break;
                    case 3: // 丁 (Ding)
                        ganIndex = 9;
                        break;
                    case 4: // 戊 (Wu)
                        ganIndex = 1;
                        break;
                    case 5: // 己 (Ji)
                        ganIndex = 3;
                        break;
                    case 6: // 庚 (Geng)
                        ganIndex = 5;
                        break;
                    case 7: // 辛 (Xin)
                        ganIndex = 7;
                        break;
                    case 8: // 壬 (Ren)
                        ganIndex = 9;
                        break;
                    case 9: // 癸 (Gui)
                        ganIndex = 1;
                        break;

                        #endregion
                }
                gan = GanStr[(ganIndex + _cMonth - 2) % 10].ToString();

                return gan + zhi + "月";
            }
        }

        #endregion

        #region GanZhiDayString

        /// <summary>
        /// Get the GanZhi representation of the day
        /// </summary>
        public string GanZhiDayString
        {
            get
            {
                var ts = _date - GanZhiStartDay;
                var offset = ts.Days;
                var i = offset % 60;
                return GanStr[i % 10] + ZhiStr[i % 12].ToString() + "日";
            }
        }

        #endregion

        #region GanZhiDateString

        /// <summary>
        /// Get the GanZhi representation of the current date, e.g., 甲子年乙丑月丙庚日
        /// </summary>
        public string GanZhiDateString
        {
            get { return GanZhiYearString + GanZhiMonthString + GanZhiDayString; }
        }

        #endregion

        #endregion

        #endregion

        #region Methods

        #region NextDay

        /// <summary>
        /// Get the next day
        /// </summary>
        /// <returns></returns>
        public ChineseCalendar NextDay()
        {
            DateTime nextDay = _date.AddDays(1);
            return new ChineseCalendar(nextDay);
        }

        #endregion

        #region PervDay

        /// <summary>
        /// Get the previous day
        /// </summary>
        /// <returns></returns>
        public ChineseCalendar PervDay()
        {
            var pervDay = _date.AddDays(-1);
            return new ChineseCalendar(pervDay);
        }

        #endregion

        #endregion
    }
}
