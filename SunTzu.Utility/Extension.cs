using System;
using System.Reflection;
using System.Text;

namespace SunTzu.Utility
{
    /// <summary>
    /// string 扩展方法
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// 将string转换为int，若转换失败，则返回0。不抛出异常。 by 夏勇兴
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Int64 ParseToInt64(this string str)
        {
            try
            {
                return Convert.ToInt64(str);
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// 将string转换为short，若转换失败，则返回0。不抛出异常。 by 夏勇兴
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ParseToInt(this string str)
        {
            try
            {
                return int.Parse(str);
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// 将string转换为int，若转换失败，则返回指定值。不抛出异常。 by 夏勇兴
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ParseToInt(this string str, int defaultValue)
        {
            try
            {
                return int.Parse(str);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 将string转换为int，若转换失败，则返回0。不抛出异常。 by 方敏
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static short ParseToShort(this string str)
        {
            try
            {
                return short.Parse(str);
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// 将string转换为int，若转换失败，则返回指定值。不抛出异常。 by 方敏
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static short ParseToShort(this string str, short defaultValue)
        {
            try
            {
                return short.Parse(str);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 将string转换为demical，若转换失败，则返回指定值。不抛出异常。 by 夏勇兴
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ParseToDecimal(this string str, decimal defaultValue)
        {
            try
            {
                return decimal.Parse(str);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 将string转换为demical，若转换失败，则返回0。不抛出异常。 by 夏勇兴
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ParseToDecimal(this string str)
        {
            try
            {
                return decimal.Parse(str);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将string转换为double，若转换失败，则返回指定值。不抛出异常。 by 夏勇兴
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ParseToDouble(this string str, double defaultValue)
        {
            try
            {
                return double.Parse(str);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 将string转换为double，若转换失败，则返回0。不抛出异常。 by 夏勇兴
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ParseToDouble(this string str)
        {
            try
            {
                return double.Parse(str);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将string转换为bool，若转换失败，则返回false。不抛出异常。 by 夏勇兴
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ParseToBool(this string str)
        {
            try
            {
                return bool.Parse(str);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 将string转换为float，若转换失败，则返回0。不抛出异常。 by 夏勇兴
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float ParseToFloat(this string str)
        {
            try
            {
                return float.Parse(str);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将string转换为Guid，若转换失败，则返回Guid.Empty。不抛出异常。 by 夏勇兴
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Guid ParseToGuid(this string str)
        {
            try
            {
                return new Guid(str);
            }
            catch
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// 将string转换为DateTime?，若转换失败，则返回null。不抛出异常。 by 夏勇兴
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime? ParseToDateTime(this string str)
        {
            try
            {
                return DateTime.Parse(str);
            }
            catch
            {
                return null;
            }
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNotNullOrEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        public static string SafeTrim(this string str)
        {
            if (str != null)
            {
                str = str.Trim();
            }
            return str;
        }

        /// <summary>
        /// 清除 SQL 的不规范字符。 by 夏勇兴
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Clean(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            StringBuilder stringBuilder = new StringBuilder();
            str = str.Trim();
            stringBuilder.Insert(0, str);
            stringBuilder.Replace("'", "");
            stringBuilder.Replace("\"", "");
            stringBuilder.Replace("=", "");
            stringBuilder.Replace("%", "");
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 清除 SQL 的不规范字符并截取指定长度。 by 夏勇兴
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxLength">字符串最大长度</param>
        /// <returns></returns>
        public static string Clean(this string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            StringBuilder stringBuilder = new StringBuilder();
            str = str.Trim();
            stringBuilder.Insert(0, str);
            stringBuilder.Replace("'", "");
            stringBuilder.Replace("\"", "");
            stringBuilder.Replace("=", "");
            stringBuilder.Replace("%", "");
            var length = stringBuilder.Length;
            if (length < 1)
                return "";

            if (length > maxLength)
                length = maxLength;

            return stringBuilder.ToString(0, length);
        }

        /// <summary>
        /// 格式化文本为html by 夏勇兴
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FormatText(this string str)
        {
            var inputText = Clean(str);

            if (string.IsNullOrEmpty(inputText))
                return "";

            StringBuilder sb = new StringBuilder();
            sb.Append(inputText);
            sb.Replace("\r\n", "<br />");
            sb.Replace("'", "&prime;"); //单引号
            sb.Replace("<", "&lt;");    //左尖括号
            sb.Replace(">", "&gt;");    //右尖括号
            sb.Replace("\"", "&quot;"); //双引号

            return sb.ToString();
        }

        /// <summary>
        /// 格式化标签
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static string FormatTags(this string tags)
        {
            if (tags == null || tags.Trim() == "")
            {
                return "";
            }
            return tags.Trim().Replace('，', ',').Replace('；', ',').Replace('、', ',').Replace(' ', ',').Replace(' ', ',').Replace('　', ',');
        }

        /// <summary>
        /// 四舍五入到整数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int Round45(this double num)
        {
            return (int)(num + 0.5);
        }

        public static void CopyTo(this object source, object target)
        {
            foreach (PropertyInfo p in source.GetType().GetProperties())
            {
                var p1 = target.GetType().GetProperty(p.Name);
                if (p1 != null)
                {
                    p1.SetValue(target, p.GetValue(source, null), null);
                }
            }
        }
    }
}