using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace DotCommon.Utility
{
    /// <summary>字符相关操作
    /// </summary>
    public class StringUtil
    {

        /// <summary>获取字符串所占直接长度
        /// </summary>
        public static int GetStringByteLength(string source)
        {
            var sourceBytes = Encoding.ASCII.GetBytes(source);
            int byteLength = 0;
            for (int i = 0; i <= sourceBytes.Length - 1; i++)
            {
                if (sourceBytes[i] == 63)
                {
                    byteLength++;
                }
                byteLength++;
            }
            return byteLength;
        }

        /// <summary> 去除末尾的字符串
        /// </summary>
        public static string RemoveEnd(string inputStr, string splitStr)
        {
            if (!string.IsNullOrWhiteSpace(inputStr))
            {
                if (string.IsNullOrWhiteSpace(splitStr))
                {
                    inputStr = inputStr.Substring(0, inputStr.Length - 1);
                }
                else
                {
                    if (inputStr.EndsWith(splitStr))
                    {
                        inputStr = inputStr.Substring(0, inputStr.LastIndexOf(splitStr, StringComparison.Ordinal));
                    }
                }
            }
            return inputStr;
        }

        #region 过滤方法
        /// <summary>过滤'-.\\;:\%   >《》 *@
        /// </summary>
        public static string FilterSpecial(string source)
        {
            if (!string.IsNullOrWhiteSpace(source))
            {
                source = Regex.Replace(source, "'-.\\\\;:\"%<>《》\\s", "", RegexOptions.IgnoreCase);
            }
            return source;
        }

        /// <summary> 过滤'-\\;\>《》 
        /// </summary>
        public static string FilterUrl(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                source = Regex.Replace(source, "'-\\\\;\\<>《》\\s", "", RegexOptions.IgnoreCase);
            }
            return source;
        }


        /// <summary> 过滤A标签
        /// </summary>
        public static string FilterA(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                source = Regex.Replace(source, @"<.?a(.|\n)*?>", "", RegexOptions.IgnoreCase);
            }
            return source;
        }

        /// <summary>过滤DIV标签
        /// </summary>
        public static string FilterDiv(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                source = Regex.Replace(source, @"<.?div(.|\n)*?>", "", RegexOptions.IgnoreCase);
            }
            return source;
        }

        /// <summary>过滤FONT标签
        /// </summary>
        public static string FilterFont(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                source = Regex.Replace(source, "<.?font(.|\n)*?>", "", RegexOptions.IgnoreCase);
            }
            return source;
        }

        /// <summary>过滤IMG标签
        /// </summary>
        public static string FilterImg(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                source = Regex.Replace(source, "<img(.|\n)*?>", "", RegexOptions.IgnoreCase); 
            }
            return source;
        }

        /// <summary>过滤OBJECT标签
        /// </summary>
        public static string FilterObject(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                string pattern = @"<object((?:.|\n)*?)</object>";
                source = Regex.Replace(source, pattern, "", RegexOptions.IgnoreCase);
            }
            return source;
        }

        /// <summary>过滤JavaScript标签
        /// </summary>
        public static string FilterScript(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                string pattern = @"<script((?:.|\n)*?)</script>";
                source = Regex.Replace(source, pattern, "", RegexOptions.IgnoreCase);
            }
            return source;
        }

        /// <summary> 过滤IFRAME标签
        /// </summary>
        public static string FilterIFrame(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                string pattern = @"<iframe((?:.|\n)*?)</iframe>";
                source = Regex.Replace(source, pattern, "", RegexOptions.IgnoreCase);
            }
            return source;
        }

        /// <summary>过滤SPAN标签
        /// </summary>
        public static string FilterSpan(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                source = Regex.Replace(source, "<.?span(.|\n)*?>", "", RegexOptions.IgnoreCase);
            }
            return source;
        }

        /// <summary>过滤STYLE样式标签
        /// </summary>
        public static string FilterStyle(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                string pattern = @"<style((?:.|\n)*?)</style>";
                source = Regex.Replace(source, pattern, "", RegexOptions.IgnoreCase);
            }
            return source;
        }

        /// <summary>过滤TABLE、TR、TD
        /// </summary>
        public static string FilterTableProtery(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                source = Regex.Replace(source, "<.?table(.|\n)*?>", "", RegexOptions.IgnoreCase);
                source = Regex.Replace(source, "<.?tr(.|\n)*?>", "", RegexOptions.IgnoreCase);
                source = Regex.Replace(source, "<.?td(.|\n)*?>", "", RegexOptions.IgnoreCase);
            }
            return source;
        }

        /// <summary>根据传入的正则表达式进行过滤
        /// </summary>
        public static string SuperiorHtml(string source, string pattern)
        {
            if (!string.IsNullOrEmpty(source))
            {
                source = Regex.Replace(source, pattern, "", RegexOptions.IgnoreCase);
            }
            return source;
        }

        /// <summary>过滤&...;的HTML标签
        /// </summary>
        public static string FileterSpec(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                foreach (Match m in Regex.Matches(source, "&.+?;"))
                {
                    source = source.Replace(m.Value, "");
                }
            }
            return source;
        }

        /// <summary>过滤所有HTML标签
        /// </summary>
        public static string StripTags(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                Regex regex = new Regex("<([^<]|\n)+?>");
                source=regex.Replace(source, "");
            }
            return source;
        }
        /// <summary>过滤html的所有标签
        /// </summary>
        public static string FilterHtml(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                foreach (Match m in Regex.Matches(source, @"<(.|\n)+?>"))
                {
                    source = source.Replace(m.Value, "");
                }
            }
            return source;
        }
        /// <summary>HTML转行成TEXT
        /// </summary>
        public static string HtmlToTxt(string strHtml)
        {
            string[] aryReg =
            {
                @"<script[^>]*?>.*?</script>",
                @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
                @"([\r\n])[\s]+",
                @"&(quot|#34);",
                @"&(amp|#38);",
                @"&(lt|#60);",
                @"&(gt|#62);",
                @"&(nbsp|#160);",
                @"&(iexcl|#161);",
                @"&(cent|#162);",
                @"&(pound|#163);",
                @"&(copy|#169);",
                @"&#(\d+);",
                @"-->",
                @"<!--.*\n"
            };
            string strOutput = aryReg.Select(t => new Regex(t, RegexOptions.IgnoreCase))
                .Aggregate(strHtml, (current, regex) => regex.Replace(current, string.Empty));
            strOutput = strOutput.Replace("<", "").Replace(">", "").Replace("\r\n", "");

            return strOutput;
        }
        #endregion

        #region 数据安全

        /// <summary>
        /// SQL 特殊字符过滤,防SQL注入
        /// </summary>
        /// <returns></returns>
        public static string SqlFilter(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                string _pattern = "exec|insert|select|delete|'|update|chr|mid|master|truncate|char|declare|and|--";
                source = Regex.Replace(source.ToLower(), _pattern, " ", RegexOptions.IgnoreCase);
            }
            return source;
        }

        /// <summary>
        /// SQL 特殊字符(%,-,')替换处理,防SQL注入
        /// </summary>
        /// <returns></returns>
        public static string SqlReplace(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                source = source.Replace("'", "''").Replace("%", "[%]").Replace("-", "[-]");
            }
            return source;
        }

        /// <summary>
        /// 转化XML的特殊字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string XmlEncode(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("&", "&amp;");
                str = str.Replace("<", "&lt;");
                str = str.Replace(">", "&gt;");
                str = str.Replace("'", "&apos;");
                str = str.Replace("\"", "&quot;");
            }
            return str;
        }

        /// <summary> 检测是否有Sql危险字符
        /// </summary>
        public static bool IsSafeSqlString(string sql)
        {
            return !Regex.IsMatch(sql, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
        #endregion

        #region 冒泡排序法
        /// <summary>
        /// 冒泡排序法
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static string[] BubbleSort(string[] r)
        {

            for (var i = 0; i < r.Length; i++) //最多做R.Length-1趟排序 
            {
                var exchange = false;

                int j; //交换标志 
                for (j = r.Length - 2; j >= i; j--)
                {//交换条件
                    if (string.CompareOrdinal(r[j + 1], r[j]) < 0)
                    {
                        var temp = r[j + 1];
                        r[j + 1] = r[j];
                        r[j] = temp;

                        exchange = true; //发生了交换，故将交换标志置为真 
                    }
                }

                if (!exchange) //本趟排序未发生交换，提前终止算法 
                {
                    break;
                }
            }
            return r;
        } 
        #endregion

        #region 模拟虚拟数据的方法 可以到小时 返回一个模拟后的结果整数  By Taven

        /// <summary>
        /// 模拟虚拟数据的方法 可以到小时 返回一个模拟后的结果整数  By Taven
        /// </summary>
        /// <param name="realInt">原数值</param>
        /// <param name="startTime">开始模拟的时间</param>
        /// <param name="stopTime">截至日期  可以是当前时间</param>
        /// <param name="k"></param>
        /// <param name="seed">种子数  可以是一个对象的ID</param>
        /// <returns></returns>
        public static int MockInt(int realInt, DateTime startTime, DateTime stopTime, int k, int seed)
        {
            //固定随机数
            // ReSharper disable once InconsistentNaming
            var _seed = seed;
            double randK = 0.314;
            int rand = (int)Math.Floor(((_seed * randK) - Math.Floor(_seed * randK)) * 10);
            rand = rand < 0 ? 3 : rand;


            var tsDiffer = stopTime.Date - startTime.Date;
            int days = tsDiffer.Days < 0 ? 0 : tsDiffer.Days;

            //计算每天需要模拟的数
            int mockInt = rand * k;

            double seedK = 8.6;
            //今天的附加K值
            int todayK = (int)Math.Floor(stopTime.Day * seedK);

            //计算总数的附加K值
            int totalK = 0;
            DateTime tDate = startTime.AddDays(-1);
            while (tDate.AddDays(1) <= stopTime)
            {
                totalK += (int)Math.Floor(tDate.Day * seedK);
                tDate = tDate.AddDays(1);
            }

            //计算当前阶段模拟展现的数
            int currentTimeInt = (int)Math.Floor(((double)(mockInt + todayK) / 24) * DateTime.Now.Hour);
            int tMockTodayInt = mockInt + todayK + realInt - currentTimeInt;

            //计算总模拟数
            int mockTotalInt = mockInt * days + realInt + totalK - tMockTodayInt;

            return mockTotalInt;

        } 
        #endregion

        #region 全局过滤方法，综合了上面的小方法
        /// <summary> 一般的过滤，过滤SQL的危险，同时过滤xml,script等,适用于富文本文档
        /// </summary>
        public static string NormalFilter(string str)
        {
            string result = SqlFilter(SqlReplace(str));  //过滤SQL
            result = XmlEncode(result);  //Xml过滤
            result = FilterScript(FilterIFrame(FilterObject(result)));
            return result;

        }
        /// <summary>
        /// 过滤所有的危险，如果带有富文本文档则不适用
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string HeighFilter(string source)
        {
            var result = SqlFilter(SqlReplace(source));
            result = XmlEncode(result);
            result = FilterHtml(StripTags(result));
            return result;
        }
        #endregion

        /// <summary>过滤对象中的string类型的参数
        /// </summary>
        public static T Filter<T>(T t) where T : class, new()
        {
            var type = typeof (T);
            if (t != null)
            {
                //获取类型为string的属性
                var properties = type.GetTypeInfo().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(x => x.PropertyType == typeof (string));
                foreach (var property in properties)
                {
                    string val = property.GetValue(t, null) == null ? "" : property.GetValue(t, null).ToString();
                    val = FilterHtml(val);
                    property.SetValue(t, val, null);
                }
            }
            return t;
        }
    }
}
