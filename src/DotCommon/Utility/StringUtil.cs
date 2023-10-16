using DotCommon.Extensions;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DotCommon.Utility
{
    /// <summary>
    /// 字符工具类
    /// </summary>
    public static class StringUtil
    {

        /// <summary>
        /// 获取字符串所占直接长度
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

        /// <summary>
        /// 去除末尾的字符串
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

        /// <summary>
        /// 过滤'-\\;\>《》 
        /// </summary>
        public static string FilterUrl(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                source = Regex.Replace(source, "'-\\\\;\\<>《》\\s", "", RegexOptions.IgnoreCase);
            }
            return source;
        }


        /// <summary>
        /// 过滤A标签
        /// </summary>
        public static string FetchA(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                source = Regex.Replace(source, @"<.?a(.|\n)*?>", "", RegexOptions.IgnoreCase);
            }
            return source;
        }

        /// <summary>
        /// 过滤DIV标签
        /// </summary>
        public static string FetchDiv(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                source = Regex.Replace(source, @"<.?div(.|\n)*?>", "", RegexOptions.IgnoreCase);
            }
            return source;
        }

        /// <summary>
        /// 过滤FONT标签
        /// </summary>
        public static string FetchFont(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                source = Regex.Replace(source, "<.?font(.|\n)*?>", "", RegexOptions.IgnoreCase);
            }
            return source;
        }

        /// <summary>
        /// 过滤SPAN标签
        /// </summary>
        public static string FetchSpan(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                source = Regex.Replace(source, "<.?span(.|\n)*?>", "", RegexOptions.IgnoreCase);
            }
            return source;
        }

        /// <summary>
        /// 过滤TABLE、TR、TD
        /// </summary>
        public static string FetchTableProtery(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                source = Regex.Replace(source, "<.?table(.|\n)*?>", "", RegexOptions.IgnoreCase);
                source = Regex.Replace(source, "<.?tr(.|\n)*?>", "", RegexOptions.IgnoreCase);
                source = Regex.Replace(source, "<.?td(.|\n)*?>", "", RegexOptions.IgnoreCase);
            }
            return source;
        }

        /// <summary>
        /// 过滤所有HTML标签
        /// </summary>
        public static string FetchStripTags(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                Regex regex = new Regex("<([^<]|\n)+?>");
                source = regex.Replace(source, "");
            }
            return source;
        }

        /// <summary>
        /// 过滤IMG标签
        /// </summary>
        public static string FilterImg(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                source = Regex.Replace(source, "<img(.|\n)*?>", "", RegexOptions.IgnoreCase);
            }
            return source;
        }

        /// <summary>
        /// 过滤OBJECT标签
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

        /// <summary>
        /// 过滤JavaScript标签
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

        /// <summary> 
        /// 过滤IFRAME标签
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


        /// <summary>
        /// 过滤STYLE样式标签
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

        /// <summary>
        /// 根据传入的正则表达式进行过滤
        /// </summary>
        public static string SuperiorHtml(string source, string pattern)
        {
            if (!string.IsNullOrEmpty(source))
            {
                source = Regex.Replace(source, pattern, "", RegexOptions.IgnoreCase);
            }
            return source;
        }

        /// <summary>
        /// 过滤html的所有标签
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

        /// <summary>
        /// HTML转行成TEXT
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
        /// 转化XML的特殊字符
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string XmlEncode(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                source = source.Replace("&", "&amp;");
                source = source.Replace("<", "&lt;");
                source = source.Replace(">", "&gt;");
                source = source.Replace("'", "&apos;");
                source = source.Replace("\"", "&quot;");
            }
            return source;
        }

        /// <summary> 
        /// 检测是否有Sql危险字符
        /// </summary>
        public static bool IsSafeSqlString(string sql)
        {
            return !Regex.IsMatch(sql, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
        #endregion

        /// <summary>
        /// 将字符串中的中文转换成unicode
        /// </summary>
        public static string StringToUnicode(string source, bool onlyConvertChinese = true)
        {
            if (!string.IsNullOrWhiteSpace(source))
            {
                char[] charbuffers = source.ToCharArray();
                byte[] buffer;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < charbuffers.Length; i++)
                {
                    if (onlyConvertChinese)
                    {
                        //非中文
                        if (!(charbuffers[i] >= 0x4e00 && charbuffers[i] <= 0x9fbb))
                        {
                            sb.Append(charbuffers[i].ToString());
                            continue;
                        }
                    }
                    buffer = Encoding.Unicode.GetBytes(charbuffers[i].ToString());
                    sb.AppendFormat("\\u{0:x2}{1:x2}", buffer[1], buffer[0]);
                }
                return sb.ToString();
            }
            return source;
        }

        /// <summary>
        /// 将Unicode字符串转换成String
        /// </summary>
        public static string UnicodeToString(string source)
        {
            if (!string.IsNullOrWhiteSpace(source))
            {
                source = new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                     source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
            }
            return source;
        }

        /// <summary>
        /// 对字符串进行匿名处理
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="length">长度</param>
        /// <param name="replace">替换字符char</param>
        /// <returns></returns>
        public static string Anonymous(string source, int length, char replace = '*')
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return "";
            }

            var chars = new char[length];
            for (var i = 0; i < chars.Length; i++)
            {
                chars[i] = replace;
            }

            var valueChars = source.ToCharArray();
            if (valueChars.Length == 2)
            {
                chars[chars.Length - 1] = valueChars[valueChars.Length - 1];
            }
            else if (valueChars.Length > 2)
            {
                if (valueChars.Length < length)
                {
                    chars[chars.Length - 1] = valueChars[valueChars.Length - 1];
                }
                else
                {
                    chars[0] = valueChars[0];
                    chars[chars.Length - 1] = valueChars[valueChars.Length - 1];
                }
            }

            return new string(chars);
        }

    }
}
