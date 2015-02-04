using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace SunTzu.Utility
{
    /// <summary>
    /// Provides a set of APIs to encrypt for Aipeks System.
    /// </summary>
    public static class CryptographyUtility
    {
        private const string key = "DiaperssKey";

        private static string Md5Encrypt(string input)
        {
            StringBuilder byteString = new StringBuilder();
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                byte[] datSource = Encoding.Default.GetBytes(input);
                byte[] newSource = md5.ComputeHash(datSource);
                foreach (byte byteValue in newSource)
                {
                    string thisByte = byteValue.ToString("x", CultureInfo.CurrentCulture);
                    if (thisByte.Length == 1)
                    {
                        thisByte = thisByte.PadLeft(2, '0');
                    }
                    byteString.Append(thisByte);
                }
            }
            return byteString.ToString();
        }

        private static string EncryptKey(string input)
        {
            string encrypekey = Md5Encrypt(key);
            int ctr = 0;
            StringBuilder tmp = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                ctr = ctr == encrypekey.Length ? 0 : ctr;
                tmp.Append(((char)(input[i] ^ encrypekey[ctr++])).ToString());
            }
            return tmp.ToString();
        }

        /// <summary>
        /// Encrypt a string.
        /// </summary>
        /// <param name="inputValue">A String to be encrypted.</param>
        /// <returns>Encrypted string.</returns>
        public static string EncryptString(string inputValue)
        {
            if (string.IsNullOrEmpty(inputValue))
            {
                return string.Empty;
            }
            else
            {
                Random random = new Random(unchecked((int)DateTime.Now.Ticks));
                int randomNum = random.Next(0, 32000);
                string encryptkey = Md5Encrypt(randomNum.ToString());
                int ctr = 0;
                StringBuilder tmp = new StringBuilder();
                for (int i = 0; i < inputValue.Length; i++)
                {
                    ctr = ctr == encryptkey.Length ? 0 : ctr;
                    tmp.Append(encryptkey[ctr].ToString() + ((char)(inputValue[i] ^ encryptkey[ctr++])).ToString());
                }
                return Convert.ToBase64String(Encoding.Default.GetBytes(EncryptKey(tmp.ToString())));
            }
        }

        /// <summary>
        /// Decrypt a string.
        /// </summary>
        /// <param name="input">Encrypted string.</param>
        /// <returns>Decrypted string.</returns>
        public static string DecryptString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            else
            {
                input = EncryptKey(Encoding.Default.GetString(Convert.FromBase64String(input)));
                StringBuilder tmp = new StringBuilder();
                for (int i = 0; i < input.Length; i++)
                {
                    tmp.Append(((char)(input[i] ^ input[++i])).ToString());
                }
                return tmp.ToString();
            }
        }

        /// <summary>
        /// Encrypt a string depend on sha256 algorithm
        /// </summary>
        /// <param name="inputValue">A String to be encrypted.</param>
        /// <returns>Encrypted string.</returns>
        public static byte[] GetSHA256(string passwordCustomerNumber)
        {
            if (string.IsNullOrEmpty(passwordCustomerNumber))
            {
                return null;
            }

            System.Security.Cryptography.SHA256Managed hashAlgorithm = new System.Security.Cryptography.SHA256Managed();
            try
            {
                return hashAlgorithm.ComputeHash(Encoding.Default.GetBytes(passwordCustomerNumber));
            }
            finally
            {
                hashAlgorithm.Clear();
            }
        }
   }
}