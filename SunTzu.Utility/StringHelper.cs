using System;
using System.Text;

namespace SunTzu.Utility
{
    // 待整理
    public class StringHelper
    {
        /// <summary>
        /// 截取字符串长度，多了用...
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string GetStrLen(string str, int len)
        {
            if (len <= 0 || str.IsNullOrEmpty())
                return str;
            if (str.Trim().Length > len + 1)
                return str.Substring(0, len) + "..";
            else
                return str;
        }

        #region 字符串过滤，过滤所有HTML标记(含JS、样式)
        /// <summary>
        /// 过滤所有页面自定义的样式
        /// 算法精要，把"<style" 到 "style>"间的所有字符过滤掉
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetStyleFilter(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return "";
            }
            string temp = str.ToLower();
            string temp1 = str;
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                int index = temp.IndexOf("<style");
                int index1 = temp.IndexOf("style>");
                if (index < 0)
                    break;
                if (index > 0)
                    sb.Append(temp1.Substring(0, index));
                if (index1 > 0)
                {
                    temp = temp.Substring(index1 + 6, temp.Length - index1 - 6);
                    temp1 = temp1.Substring(index1 + 6, temp1.Length - index1 - 6);
                }
            }

            sb.Append(temp1);
            return sb.ToString();
        }
        /// <summary>
        /// 过滤所有HTML标记
        /// 算法精要，只要把"<"与">"间的所有字符过滤掉限可
        /// </summary>
        /// <returns></returns>
        public static string GetLgtFilter(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return "";
            }
            string temp = str;
            StringBuilder sb = new StringBuilder();
            //过滤HTML标记 "<"与">"间的所有字符
            while (true)
            {
                int index = temp.IndexOf("<");
                int index1 = temp.IndexOf(">");
                if (index < 0)
                    break;
                if (index > 0)
                    sb.Append(temp.Substring(0, index));
                if (index1 > 0)
                    temp = temp.Substring(index1 + 1, temp.Length - index1 - 1);
            }
            sb.Append(temp);
            return sb.ToString();
        }

        /// <summary>
        /// 过滤所有脚本
        /// 算法精要，把"<script" 到 "script>"间的所有字符过滤掉
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetScriptFilter(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return "";
            }
            string temp = str.ToLower();
            string temp1 = str;
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                int index = temp.IndexOf("<script");
                int index1 = temp.IndexOf("script>");
                if (index < 0)
                    break;
                if (index > 0)
                    sb.Append(temp1.Substring(0, index));
                if (index1 > 0)
                {
                    temp = temp.Substring(index1 + 7, temp.Length - index1 - 7);
                    temp1 = temp1.Substring(index1 + 7, temp1.Length - index1 - 7);
                }
            }

            sb.Append(temp1);
            return sb.ToString();
        }

        /// <summary>
        /// 过滤所有strl与strr间的字符（通用方法） 
        /// 注意strl与strr不区分大小写。前提是strl与strr会配对出现
        /// </summary>
        /// <param name="str"></param>
        /// <param name="strl"></param>
        /// <param name="strr"></param>
        /// <returns></returns>
        public static string GetStringFilter(string str, string strl, string strr)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return "";
            }
            string temp = str.ToLower();
            string temp1 = str;
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                int index = temp.IndexOf(strl.ToLower());
                int index1 = temp.IndexOf(strr.ToLower());
                if (index < 0)
                    break;
                if (index > 0)
                    sb.Append(temp1.Substring(0, index));
                if (index1 > 0)
                {
                    temp = temp.Substring(index1 + strr.Length, temp.Length - index1 - strr.Length);
                    temp1 = temp1.Substring(index1 + strr.Length, temp1.Length - index1 - strr.Length);
                }
            }

            sb.Append(temp1);
            return sb.ToString();
        }

        /// <summary>
        /// 过滤所有HTML标记(含JS、样式)
        /// 算法精要，只要把"<script"与"</script>"间 
        /// "<style"与"</style>"间 
        /// "<"与">"间的所有字符过滤掉限可
        /// </summary>
        /// <returns></returns>
        public static string GetAllHtmlFilter(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return "";
            }
            string temp = str;
            //过滤注释
            temp = GetStringFilter(temp, "<!--", "-->");
            //过滤JS
            temp = GetStringFilter(temp, "<script", "</script>");
            //过滤样式
            temp = GetStringFilter(temp, "<style", "</style>");
            //过滤HTML标记 "<"与">"间的所有字符
            temp = GetStringFilter(temp, "<", ">");
            return temp;
        }
        #endregion

        /// <summary>
        /// 根据长度返回指定长度的随机字符
        /// </summary>
        /// <param name="p_iLength"></param>
        /// <returns></returns>
        public static string MakeRandomString(int p_iLength)
        {
            string strFountain = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random rnd = new Random();
            string strRandom = strFountain;
            string strReturn = "";
            int iRandNum;

            for (int i = 0; i < p_iLength; i++)
            {
                iRandNum = rnd.Next(strRandom.Length);
                strReturn += strRandom[iRandNum];
            }

            return strReturn;
        }

        /// <summary>
        /// 根据长度返回指定长度的随机字符
        /// </summary>
        /// <param name="p_iLength"></param>
        /// <returns></returns>
        public static string MakeRandomStringSmall(int p_iLength)
        {
            string strFountain = "abcdefghijklmnopqrstuvwxyz0123456789";
            Random rnd = new Random();
            string strRandom = strFountain;
            string strReturn = "";
            int iRandNum;

            for (int i = 0; i < p_iLength; i++)
            {
                iRandNum = rnd.Next(strRandom.Length);
                strReturn += strRandom[iRandNum];
            }

            return strReturn;
        }

        /// <summary>
        /// 根据长度返回指定长度的随机字符
        /// </summary>
        /// <param name="p_iLength"></param>
        /// <returns></returns>
        public static string MakeRandomStringUpper(int p_iLength)
        {
            string strFountain = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random rnd = new Random();
            string strRandom = strFountain;
            string strReturn = "";
            int iRandNum;

            for (int i = 0; i < p_iLength; i++)
            {
                iRandNum = rnd.Next(strRandom.Length);
                strReturn += strRandom[iRandNum];
            }

            return strReturn;
        }

        /// <summary>
        /// 根据时间刻度生成随机码
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string CreateRandomCode(int codeCount)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;

            Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(35);
                if (temp == t)
                {
                    return CreateRandomCode(codeCount);
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }

        /// <summary>
        /// 根据时间刻度生成随机码
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string CreateRandomNumCode(int codeCount)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;

            Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(10);
                if (temp == t)
                {
                    return CreateRandomNumCode(codeCount);
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }

        public static string CreateGiftOrderNum()
        {
            string strNum = "GF";

            strNum += DateTime.Now.ToString("yyyyMMddHHmm") + CreateRandomNumCode(4);

            return strNum;
        }
    }
}
