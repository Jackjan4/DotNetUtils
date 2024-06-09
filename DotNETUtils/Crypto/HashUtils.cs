using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Roslan.DotNETUtils.Crypto {



    /// <summary>
    /// Provides utility functions for hashing strings and streams with typically used hashing algorithms in a one-shot manner.
    /// </summary>
    public class HashUtils {



        /// <summary>
        /// Keep constructor private to prevent instantiation.
        /// </summary>
        private HashUtils() {

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] Sha256(string str) {
            return Sha256(str, Encoding.UTF8);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] Sha256(string str, Encoding encoding) {
#if NETSTANDARD2_0
            using (SHA256 sha256 = SHA256.Create()) {
                return sha256.ComputeHash(encoding.GetBytes(str));
            }
#elif NET8_0_OR_GREATER
            return SHA256.HashData(encoding.GetBytes(str));
#endif
        }



        /// Create a SHA256 hash from a stream
        public static byte[] Sha256(Stream stream) {
#if NETSTANDARD2_0
            using (SHA256 sha256 = SHA256.Create()) {
                return sha256.ComputeHash(stream);
            }
#elif NET8_0_OR_GREATER
            return SHA256.HashData(stream);
#endif
        }



        /// <summary>
        /// Computes the SHA256 hash of the specified string using UTF8 encoding.
        /// </summary>
        /// <param name="str">The string to compute the hash for.</param>
        /// <returns>The computed SHA256 hash as a hexadecimal converted string.</returns>
        public static string Sha256String(string str) {
            return Sha256String(str, Encoding.UTF8);
        }



        /// <summary>
        /// Computes the SHA256 hash of the specified string using the specified encoding.
        /// </summary>
        /// <param name="str">The string to compute the hash for.</param>
        /// <param name="encoding">The encoding to use for the string.</param>
        /// <returns>The computed SHA256 hash as a hexadecimal converted string.</returns>
        public static string Sha256String(string str, Encoding encoding) {
            byte[] hashBytes = Sha256(str, encoding);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }



        /// <summary>
        /// Computes the MD5 hash of the specified string using the specified encoding.
        /// </summary>
        /// <param name="stream">The string to compute the hash for.</param>
        /// <returns>The computed MD5 hash as a hexadecimal converted string.</returns>
        public static string Sha256String(Stream stream) {
            byte[] hashBytes = Sha256(stream);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] Md5(string str) {
            return Md5(str, Encoding.UTF8);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] Md5(string str, Encoding encoding) {
#if NETSTANDARD2_0
            using (MD5 md5 = MD5.Create()) {
                return md5.ComputeHash(encoding.GetBytes(str));
            }
#elif NET8_0_OR_GREATER
            return MD5.HashData(encoding.GetBytes(str));
#endif
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] Md5(Stream stream) {
#if NETSTANDARD2_0
            using (MD5 md5 = MD5.Create()) {
                return md5.ComputeHash(stream);
            }
#elif NET8_0_OR_GREATER
            return MD5.HashData(stream);
#endif
        }



#if NET8_0_OR_GREATER
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static async Task<byte[]> Md5Async(Stream stream) {
            return await MD5.HashDataAsync(stream);
        }
#endif



        /// <summary>
        /// Computes the MD5 hash of the specified string using UTF8 encoding.
        /// </summary>
        /// <param name="str">The string to compute the hash for.</param>
        /// <returns>The computed MD5 hash as a hexadecimal converted string.</returns>
        public static string Md5String(string str) {
            return Md5String(str, Encoding.UTF8);
        }



        /// <summary>
        /// Computes the MD5 hash of the specified string using the specified encoding.
        /// </summary>
        /// <param name="str">The string to compute the hash for.</param>
        /// <param name="encoding">The encoding to use for the string.</param>
        /// <returns>The computed MD5 hash as a hexadecimal converted string.</returns>
        public static string Md5String(string str, Encoding encoding) {
            byte[] hashBytes = Md5(str, encoding);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }



        /// <summary>
        /// Computes the MD5 hash of the specified string using the specified encoding.
        /// </summary>
        /// <param name="stream">The string to compute the hash for.</param>
        /// <returns>The computed MD5 hash as a hexadecimal converted string.</returns>
        public static string Md5String(Stream stream) {
            byte[] hashBytes = Md5(stream);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }




#if NET8_0_OR_GREATER
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static async Task<string> Md5StringAsync(Stream stream) {
            byte[] hashBytes = await MD5.HashDataAsync(stream);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
#endif
    }
}