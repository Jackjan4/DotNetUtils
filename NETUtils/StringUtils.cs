using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace De.JanRoslan.NETUtils
{
    public static class StringUtils
    {


        /// <summary>
        /// If this string represents a path to a file, returns the extension of the file
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetExtension(string s)
        {

            Match match = Regex.Match(new string(s.Reverse().ToArray()), @"[a-zA-Z0-9]+\.");
            return new string(match.Value.Reverse().ToArray());
        }


        /// <summary>
        /// Returns the byte-array representation of a hex-string that 
        /// </summary>
        /// <param name="hexStr"></param>
        /// <returns></returns>
        public static byte[] HexStrToByteArray(string hexStr)
        {
            // Check if Hex-string is not valid
            if (hexStr.Length % 2 != 0)
            {

            }

            byte[] result = new byte[hexStr.Length / 2];

            for (int i = 0; i < hexStr.Length/2; i++)
            {
                result[i] = Convert.ToByte(hexStr.Substring(i*2, 2), 16);
            }
            return result;
        }

    }
}
