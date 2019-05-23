using System;
using System.Collections.Generic;
using System.Text;

namespace De.JanRoslan.NETUtils.Encoding
{
    public class EncodingUtils
    {


        public static string Base64Decode(string base64Data)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64Data);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
