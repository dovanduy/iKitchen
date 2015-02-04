using System;
using System.Security.Cryptography;
using System.Text;

namespace SunTzu.Utility
{
    public class TripleDes
    {
        public string GenerateKeyInBase64()
        {
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
            provider.GenerateKey();
            return Convert.ToBase64String(provider.Key);
        }

        public byte[] Encrypt(string plainText)
        {
            TripleDESCryptoServiceProvider provider = Create3DesProvider();
            ICryptoTransform encryptor = provider.CreateEncryptor();
            byte[] plainBytes = new UTF8Encoding().GetBytes(plainText);
            return encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }

        public string Decrypt(byte[] cipherBytes)
        {
            TripleDESCryptoServiceProvider provider = Create3DesProvider();
            ICryptoTransform decryptor = provider.CreateDecryptor();
            byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return new UTF8Encoding().GetString(plainBytes);
        }

        private TripleDESCryptoServiceProvider Create3DesProvider()
        {
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
            provider.Key = Convert.FromBase64String(KeyInBase64);
            provider.IV = new byte[provider.BlockSize / 8];
            return provider;
        }

        public string KeyInBase64;
    }
}
