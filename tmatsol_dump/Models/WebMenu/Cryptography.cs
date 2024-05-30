using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace Table_Cart_V2k20.Models
{
    public static class Cryptography
    {
        public static string hash = "Think@2021";
        public static string EncCode;
        public static string DecCode;
        public static string EncData(string i)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(i);

            using (MD5CryptoServiceProvider md5=new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes=new TripleDESCryptoServiceProvider() { Key=keys,Mode=CipherMode.ECB,Padding=PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] result = transform.TransformFinalBlock(data, 0, data.Length);
                    EncCode = Convert.ToBase64String(result,0,result.Length);
                }
            }
            return EncCode;
        }
        public static string DecData(string i)
        {
            byte[] data = Convert.FromBase64String(i.ToString());

            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] result = transform.TransformFinalBlock(data, 0, data.Length);
                    DecCode = UTF8Encoding.UTF8.GetString(result);
                }
            }
            return DecCode;
        }
        public static string GenerateNewRandom()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            if (r.Distinct().Count() == 1)
            {
                r = GenerateNewRandom();
            }
            return r;
        }
    }
}