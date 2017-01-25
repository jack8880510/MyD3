using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MyD3.Common
{
    public static class MD5Helper
    {
        /// <summary>
        /// 用md5加密
        /// </summary>
        /// <param name="Sourcein">输入的数据</param>
        /// <returns></returns>
        public static string MD5(string Sourcein)
        {
            MD5CryptoServiceProvider MD5CSP = new MD5CryptoServiceProvider();
            byte[] MD5Source = System.Text.Encoding.UTF8.GetBytes(Sourcein);
            byte[] MD5Out = MD5CSP.ComputeHash(MD5Source);
            string sTemp = "";
            for (int i = 0; i < MD5Out.Length; i++)
            {
                sTemp += MD5Out[i].ToString("x").PadLeft(2, '0');
            }
            return sTemp;
        }
    }
}
