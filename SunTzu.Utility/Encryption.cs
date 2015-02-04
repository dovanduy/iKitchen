//********************************************************
//create date: 2006-08-17
//latest update: 2006-08-17
//author: Xiayx
//summary: ���������
//********************************************************


using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace SunTzu.Utility
{
    /// <summary>
    /// Depiction:ʹ�ü����㷨���ַ������м���
    /// Writer:Xiayx 
    /// Create Date;2007-12-14 
    /// </summary>
    public sealed class Encryption
    {
        /// <summary>
        /// MD5����
        /// </summary>
        /// <param name="oldValue">Ҫ�����ܵ��ַ���</param>
        /// <returns>���ܺ��32λ�ַ���</returns>
        public static string MD5Encrypt(string oldValue)
        {
            if (oldValue.IsNullOrEmpty())
            {
                return oldValue;
            }

            try
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] newValue = Encoding.UTF8.GetBytes(oldValue);
                byte[] hashValue = md5.ComputeHash(newValue);

                StringBuilder stringBuilder = new StringBuilder();
                int i = 0;

                for (i = 0; i < hashValue.Length; i++)
                {
                    stringBuilder.Append(hashValue[i].ToString("X").PadLeft(2, '0'));
                }

                return stringBuilder.ToString();
            }
            catch
            {
                throw new Exception("MD5����ʧ��");
            }
        }
    }
}

