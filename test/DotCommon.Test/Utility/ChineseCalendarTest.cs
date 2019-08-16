using DotCommon.Utility;
using System;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class ChineseCalendarTest
    {
        [Fact]
        public void ChineseCalendarException_Test()
        {
            var exception = new ChineseCalendarException("error1");
            Assert.Equal("error1", exception.Message);
        }

        [Fact]
        public void ChineseCalendar_Test()
        {
            var c1 = new ChineseCalendar(new DateTime(2019, 1, 5, 12, 10, 10));
            Assert.Equal(11, c1.Animal);
            Assert.Equal("狗", c1.AnimalString);
            Assert.Equal(2018, c1.ChineseYear);
            Assert.Equal("二零一八年", c1.ChineseYearString);
            Assert.Equal(11, c1.ChineseMonth);
            Assert.Equal("十一月", c1.ChineseMonthString);
            Assert.Equal(30, c1.ChineseDay);
            Assert.Equal("三十", c1.ChineseDayString);
            Assert.Equal("戊戌年", c1.GanZhiYearString);
            Assert.Equal("甲子月", c1.GanZhiMonthString);
            Assert.Equal("壬寅日", c1.GanZhiDayString);
            Assert.Equal("丙午", c1.ChineseHour);
            Assert.Equal("戊戌年甲子月壬寅日", c1.GanZhiDateString);
            Assert.Equal("", c1.ChineseCalendarHoliday);
            Assert.Equal("胃土彘", c1.ChineseConstellation);
            Assert.Equal("农历二零一八年十一月三十", c1.ChineseDateString);
            Assert.Equal("小寒", c1.ChineseTwentyFourDay);
            Assert.Equal("星期六", c1.WeekDayStr);
            Assert.Equal("", c1.WeekDayHoliday);
            Assert.Equal("摩羯座", c1.Constellation);
            Assert.Equal("大寒[2019-01-20]", c1.ChineseTwentyFourNextDay);
            Assert.Equal("", c1.ChineseTwentyFourPrevDay);
            Assert.Equal("公元2019-01-05 00:00:00", c1.DateString);
            Assert.Equal(DayOfWeek.Saturday, c1.WeekDay);
            Assert.False(c1.IsChineseLeapYear);
            Assert.False(c1.IsChineseLeapMonth);
            Assert.Equal(new DateTime(2019, 1, 5), c1.Date);
            Assert.Equal("", c1.DateHoliday);
            Assert.False(c1.IsLeapYear);

            var c2 = c1.NextDay();
            Assert.Equal("癸卯日", c2.GanZhiDayString);

            var c3 = c1.PervDay();
            Assert.Equal("辛丑日", c3.GanZhiDayString);

            var c4 = new ChineseCalendar(2018, 1, 1, false);
            Assert.Equal("二零一八年", c4.ChineseYearString);
            Assert.Equal("春节", c4.ChineseCalendarHoliday);

            var c5 = new ChineseCalendar(2017, 6, 2, true);
            Assert.True(c5.IsChineseLeapYear);
            Assert.True(c5.IsChineseLeapMonth);

            var c6 = new ChineseCalendar(new DateTime(2019, 8, 15));
            Assert.Equal("壬申月", c6.GanZhiMonthString);

            var c7 = new ChineseCalendar(new DateTime(2019, 5, 12));
            Assert.Equal("母亲节", c7.WeekDayHoliday);

            var c8 = new ChineseCalendar(new DateTime(2019, 5, 14));
            Assert.Equal("初十", c8.ChineseDayString);

            var c9 = new ChineseCalendar(new DateTime(2019, 5, 24));
            Assert.Equal("二十", c9.ChineseDayString);

            var c10 = new ChineseCalendar(new DateTime(2018, 2, 15));
            Assert.Equal("除夕", c10.ChineseCalendarHoliday);

            var cn1 = new ChineseCalendar(new DateTime(2019, 7, 1));
            var cn2 = new ChineseCalendar(new DateTime(2019, 7, 2));
            var cn3 = new ChineseCalendar(new DateTime(2019, 7, 3));
            var cn4 = new ChineseCalendar(new DateTime(2019, 7, 4));
            var cn5 = new ChineseCalendar(new DateTime(2019, 7, 5));
            var cn6 = new ChineseCalendar(new DateTime(2019, 7, 6));
            var cn7 = new ChineseCalendar(new DateTime(2019, 7, 7));

            Assert.Equal("星期一", cn1.WeekDayStr);
            Assert.Equal("星期二", cn2.WeekDayStr);
            Assert.Equal("星期三", cn3.WeekDayStr);
            Assert.Equal("星期四", cn4.WeekDayStr);
            Assert.Equal("星期五", cn5.WeekDayStr);
            Assert.Equal("星期六", cn6.WeekDayStr);
            Assert.Equal("星期日", cn7.WeekDayStr);

            Assert.Equal("", cn1.WeekDayHoliday);
            Assert.Equal("", cn2.WeekDayHoliday);
            Assert.Equal("", cn3.WeekDayHoliday);
            Assert.Equal("", cn4.WeekDayHoliday);
            Assert.Equal("", cn5.WeekDayHoliday);
            Assert.Equal("", cn6.WeekDayHoliday);
            Assert.Equal("", cn7.WeekDayHoliday);


            var cons1 = new ChineseCalendar(new DateTime(2011, 1, 1));
            var cons2 = new ChineseCalendar(new DateTime(2012, 2, 10));
            var cons3 = new ChineseCalendar(new DateTime(2013, 3, 5));
            var cons4 = new ChineseCalendar(new DateTime(2014, 4, 10));
            var cons5 = new ChineseCalendar(new DateTime(2015, 5, 10));
            var cons6 = new ChineseCalendar(new DateTime(2016, 6, 10));
            var cons7 = new ChineseCalendar(new DateTime(2017, 7, 10));
            var cons8 = new ChineseCalendar(new DateTime(2018, 8, 10));
            var cons9 = new ChineseCalendar(new DateTime(2019, 9, 10));
            var cons10 = new ChineseCalendar(new DateTime(2019, 10, 10));
            var cons11 = new ChineseCalendar(new DateTime(2019, 11, 10));
            var cons12 = new ChineseCalendar(new DateTime(2019, 12, 10));

            Assert.Equal("摩羯座", cons1.Constellation);
            Assert.Equal("二零一零年", cons1.ChineseYearString);
            Assert.Equal("戊子月", cons1.GanZhiMonthString);

          

            Assert.Equal("水瓶座", cons2.Constellation);
            Assert.Equal("二零一二年", cons2.ChineseYearString);
            Assert.Equal("壬寅月", cons2.GanZhiMonthString);

            Assert.Equal("双鱼座", cons3.Constellation);
            Assert.Equal("二零一三年", cons3.ChineseYearString);
            Assert.Equal("甲寅月", cons3.GanZhiMonthString);

            Assert.Equal("白羊座", cons4.Constellation);
            Assert.Equal("二零一四年", cons4.ChineseYearString);
            Assert.Equal("戊辰月", cons4.GanZhiMonthString);

            Assert.Equal("金牛座", cons5.Constellation);
            Assert.Equal("二零一五年", cons5.ChineseYearString);
            Assert.Equal("庚辰月", cons5.GanZhiMonthString);

            Assert.Equal("双子座", cons6.Constellation);
            Assert.Equal("二零一六年", cons6.ChineseYearString);
            Assert.Equal("甲午月", cons6.GanZhiMonthString);

            Assert.Equal("巨蟹座", cons7.Constellation);
            Assert.Equal("二零一七年", cons7.ChineseYearString);
            Assert.Equal("丁未月", cons7.GanZhiMonthString);

            Assert.Equal("狮子座", cons8.Constellation);
            Assert.Equal("二零一八年", cons8.ChineseYearString);

           

            Assert.Equal("处女座", cons9.Constellation);
            Assert.Equal("二零一九年", cons9.ChineseYearString);
            Assert.Equal("癸酉月", cons9.GanZhiMonthString);

            Assert.Equal("天秤座", cons10.Constellation);
            Assert.Equal("天蝎座", cons11.Constellation);

            Assert.Equal("射手座", cons12.Constellation);

            Assert.Throws<ChineseCalendarException>(() =>
            {
                new ChineseCalendar(2016, 13, 1, false);
            });
            Assert.Throws<ChineseCalendarException>(() =>
            {
                new ChineseCalendar(2016, 2, 31, false);
            });

        }
    }
}
