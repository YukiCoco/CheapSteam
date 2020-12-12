using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace ChpStmScraper
{
    public class Helper
    {
        /// <summary>
        /// Unicode To String
        /// </summary>
        /// <param name="unicodeStr"></param>
        /// <returns></returns>
        public static string UnicodeToString(string unicodeStr)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            return utf8.GetString(utf8.GetBytes(unicodeStr));
        }

        /// <summary>
        /// String 转 Double
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double String2Double(string str)
        {
            return string.IsNullOrEmpty(str) ? 0 : double.Parse(str);
        }

        /// <summary>
        /// String 转 int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int String2Int(string str)
        {
            return string.IsNullOrEmpty(str) ? 0 : int.Parse(str);
        }

        /// <summary>
        /// 返回一个链接中的基本URL https://example.com
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetBaseUrl(string url)
        {
            Regex regex = new Regex(@"^http(s?):\/\/.+?(?=\/|\?|#|$)");
            //Debug.WriteLine(regex.Match(url).Value);
            return regex.Match(url).Value;
        }
    }
}