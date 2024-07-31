using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;



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



        /// <summary>
        /// Returns the value of a command-line option.
        /// This function can detect:
        /// --optionName=value
        /// -o=value
        /// </summary>
        /// <returns></returns>
        public static string GetCommandLineOption(IEnumerable<string> args, string optionName) {
            // This function can detect:
            // --optionName=value
            // -o=value
            //
            // TODO: This function can't detect:
            // --optionName value (because they would probably be in different indexes)
            // -o value (because they would probably be in different indexes)
            string result;

            var prefix = "";

            // Check if optionName exists and detect prefix
            var filtered = args.FirstOrDefault(arg => {
                if (arg.StartsWith("--" + optionName)) {
                    prefix = "--";
                    return true;
                }
                if (arg.StartsWith("-" + optionName)) {
                    prefix = "-";
                    return true;
                }
                return false;
            });

            if (filtered == null || string.IsNullOrEmpty(prefix)) {
                return null;
            }

            result = filtered.Substring((prefix.Length == 1 ? 2 : 3) + optionName.Length);
            return result;
        }

    }
}
