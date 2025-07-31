using System.Text;
using System;

namespace DotCommon.Utility
{
    /// <summary>
    /// Provides utility methods for converting monetary values into Chinese currency representation (RMB).
    /// </summary>
    public static class RMBConverter
    {
        private static readonly string[] ChineseDigits = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
        private static readonly string[] Units = { "", "拾", "佰", "仟" };
        private static readonly string[] GroupUnits = { "", "万", "亿", "兆" };

        public static string ToRmb(string input)
        {
            if (!decimal.TryParse(input, out decimal amount))
                return string.Empty;

            return ToRmb(amount);
        }

        public static string ToRmb(decimal amount)
        {
            if (amount < 0)
                return "负数不支持";
            if (amount > 9999999999999.99m)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "金额超过最大限制");
            }

            if (amount == 0)
                return "零元整";

            long integral = (long)Math.Floor(amount);
            int fraction = (int)((amount - integral) * 100 + 0.0001m); // 避免浮点误差

            var integralStr = ConvertIntegral(integral);
            var fractionStr = ConvertFraction(fraction, integral == 0);

            return integralStr + fractionStr;
        }

        private static string ConvertIntegral(long num)
        {
            if (num == 0) return "";

            var sb = new StringBuilder();

            int groupIndex = 0;
            bool zeroFlag = false;

            while (num > 0)
            {
                int part = (int)(num % 10000);
                if (part == 0)
                {
                    if (!zeroFlag && sb.Length > 0)
                    {
                        sb.Insert(0, "零");
                        zeroFlag = true;
                    }
                }
                else
                {
                    var section = ConvertFourDigit(part);
                    if (groupIndex > 0)
                        section += GroupUnits[groupIndex];
                    sb.Insert(0, section);
                    zeroFlag = false;
                }

                num /= 10000;
                groupIndex++;
            }

            sb.Append("元");
            return sb.ToString().Replace("零零", "零").TrimStart('零');
        }

        private static string ConvertFourDigit(int num)
        {
            var sb = new StringBuilder();
            int[] digits = [num / 1000, num % 1000 / 100, num % 100 / 10, num % 10];

            bool zeroFlag = false;

            for (int i = 0; i < 4; i++)
            {
                if (digits[i] == 0)
                {
                    zeroFlag = true;
                }
                else
                {
                    if (zeroFlag)
                    {
                        sb.Append("零");
                        zeroFlag = false;
                    }
                    sb.Append(ChineseDigits[digits[i]]);
                    sb.Append(Units[3 - i]);
                }
            }

            return sb.ToString().TrimEnd('零');
        }

        private static string ConvertFraction(int fraction, bool integerIsZero)
        {
            int jiao = fraction / 10;
            int fen = fraction % 10;

            if (jiao == 0 && fen == 0)
                return "整";

            var sb = new StringBuilder();

            if (jiao > 0)
                sb.Append(ChineseDigits[jiao] + "角");

            if (fen > 0)
            {
                if (jiao == 0 && !integerIsZero)
                    sb.Append("零");
                sb.Append(ChineseDigits[fen] + "分");
            }

            return sb.ToString();
        }
    }
}
