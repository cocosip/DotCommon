using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DotCommon.Utility
{
    /// <summary>字符相关操作
    /// </summary>
    public static class StringUtil
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

        /// <summary>过滤空格符号等HTML标签
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
                source = regex.Replace(source, "");
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

        /// <summary>将字符串中的中文转换成unicode
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

        /// <summary>将Unicode字符串转换成String
        /// </summary>
        public static string UnicodeToString(string source)
        {
            if (!string.IsNullOrWhiteSpace(source))
            {
                return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                     source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
            }
            return source;
        }

    }
}
