using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Timetable.Security
{
    public abstract class DES
    {
        /// <summary>
        /// Des解密方法
        /// </summary>
        /// <param name="val">要解密的值</param>
        /// <param name="key">8位解密秘钥</param>
        /// <param name="IV">8位向量</param>
        /// <returns></returns>
        public static string Decrypt(string val, string key, string IV)
        {
            try
            {
                byte[] buffer1 = System.Text.Encoding.Default.GetBytes(key);
                byte[] buffer2 = System.Text.Encoding.Default.GetBytes(IV);
                DESCryptoServiceProvider provider1 = new DESCryptoServiceProvider();
                provider1.Mode = CipherMode.CBC;
                provider1.Padding = PaddingMode.PKCS7;
                provider1.Key = buffer1;
                provider1.IV = buffer2;
                ICryptoTransform transform1 = provider1.CreateDecryptor(provider1.Key, provider1.IV);
                byte[] buffer3 = Convert.FromBase64String(val);
                MemoryStream stream1 = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream1, transform1, CryptoStreamMode.Write);
                stream2.Write(buffer3, 0, buffer3.Length);
                stream2.FlushFinalBlock();
                stream2.Close();
                return Encoding.Default.GetString(stream1.ToArray());
            }
            catch (System.Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// Des加密方法
        /// </summary>
        /// <param name="val">要加密的值</param>
        /// <param name="key">加密秘钥</param>
        /// <param name="IV">加密向量</param>
        /// <returns></returns>
        public static string Encrypt(string val, string key, string IV)
        {
            try
            {
                byte[] buffer1 = System.Text.Encoding.Default.GetBytes(key);
                byte[] buffer2 = System.Text.Encoding.Default.GetBytes(IV);

                DESCryptoServiceProvider provider1 = new DESCryptoServiceProvider();
                provider1.Mode = CipherMode.CBC;
                provider1.Padding = PaddingMode.PKCS7;
                provider1.Key = buffer1;
                provider1.IV = buffer2;
                ICryptoTransform transform1 = provider1.CreateEncryptor(provider1.Key, provider1.IV);
                byte[] buffer3 = Encoding.Default.GetBytes(val);
                MemoryStream stream1 = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream1, transform1, CryptoStreamMode.Write);
                stream2.Write(buffer3, 0, buffer3.Length);
                stream2.FlushFinalBlock();
                stream2.Close();
                return Convert.ToBase64String(stream1.ToArray());
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        ///     解密
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public abstract string Decrypt(string val);

        /// <summary>
        ///  加密
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public abstract String Encrypt(string val);
    }

}
