using System;

namespace Roslan.DotNetUtils.Text {
    public static class StringUtils {



        /// There was a method GetExtension(...) here which returned the file extension of a string that represents a file path.
        /// That method is not needed anymore because Path.GetExtension(...) exists in .NET since .NET Framework 1.1 (lol).



        /// <summary>
        /// Returns the byte-array representation of a hex-string that 
        /// </summary>
        /// <param name="hexStr"></param>
        /// <returns></returns>
        public static byte[] HexStrToByteArray(string hexStr) {
            // Check if Hex-string is not valid
            if (hexStr.Length % 2 != 0) {

            }

            var result = new byte[hexStr.Length / 2];

            for (var i = 0; i < hexStr.Length / 2; i++) {
                result[i] = Convert.ToByte(hexStr.Substring(i * 2, 2), 16);
            }
            return result;
        }

    }
}
