using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace De.JanRoslan.NETUtils.Crypto
{
    public class HashUtils
    {


        private HashUtils() {

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="randomString"></param>
        /// <returns></returns>
        public static string Sha256(string str) {
            byte[] res = Sha256(System.Text.Encoding.UTF8.GetBytes(str));
            return System.Text.Encoding.UTF8.GetString(res);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] Sha256(byte[] bytes) {
            SHA256 alg = SHA256.Create();
            byte[] result = alg.ComputeHash(bytes);
            return result;
        }
    }
}
