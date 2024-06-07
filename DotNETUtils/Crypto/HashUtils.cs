using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Roslan.DotNETUtils.Crypto;
public class HashUtils {



    private HashUtils() {

    }



    /// <summary>
    /// Computes the SHA256 hash of the specified string using the specified encoding.
    /// </summary>
    /// <param name="str">The string to compute the hash for.</param>
    /// <param name="encoding">The encoding to use for the string.</param>
    /// <returns>The computed SHA256 hash as a hexadecimal converted string.</returns>
    public static string Sha256(string str, Encoding encoding) {
        var hashBytes = SHA256.HashData(encoding.GetBytes(str));
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }



    /// <summary>
    /// Computes the SHA256 hash of the specified string using UTF8 encoding.
    /// </summary>
    /// <param name="str">The string to compute the hash for.</param>
    /// <returns>The computed SHA256 hash as a hexadecimal converted string.</returns>
    public static string Sha256(string str) {
        return Sha256(str, Encoding.UTF8);
    }



    /// <summary>
    /// Computes the MD5 hash of the specified string using the specified encoding.
    /// </summary>
    /// <param name="str">The string to compute the hash for.</param>
    /// <param name="encoding">The encoding to use for the string.</param>
    /// <returns>The computed MD5 hash as a hexadecimal converted string.</returns>
    public static string Md5(string str, Encoding encoding) {
        var hashBytes = MD5.HashData(encoding.GetBytes(str));
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }



    /// <summary>
    /// Computes the MD5 hash of the specified string using UTF8 encoding.
    /// </summary>
    /// <param name="str">The string to compute the hash for.</param>
    /// <returns>The computed MD5 hash as a hexadecimal converted string.</returns>
    public static string Md5(string str) {
        return Md5(str, Encoding.UTF8);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="bufferSize"></param>
    /// <returns></returns>
    public static string Md5File(string path, int bufferSize = 4096) {
        using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize);
        return BitConverter.ToString(MD5.HashData(fs)).Replace("-", "").ToLower();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="bufferSize"></param>
    /// <returns></returns>
    public static async Task<string> Md5FileAsync(string path, int bufferSize = 4096) {
        await using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize,useAsync: true);

        var hashData = await MD5.HashDataAsync(fs);

        return BitConverter.ToString(hashData).Replace("-", "").ToLower();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="path1"></param>
    /// <param name="path2"></param>
    /// <param name="bufferSize"></param>
    /// <returns></returns>
    public static bool CompareMd5FileHashes(string path1, string path2, int bufferSize = 4096) {
        var result1 = Md5File(path1, bufferSize);
        var result2 = Md5File(path2, bufferSize);

        return string.Equals(result1, result2, StringComparison.OrdinalIgnoreCase);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="path1"></param>
    /// <param name="path2"></param>
    /// <param name="bufferSize"></param>
    /// <returns></returns>
    public static async Task<bool> CompareMd5FileHashesAsync(string path1, string path2, int bufferSize = 4096) {
        var result1 = await Md5FileAsync(path1, bufferSize);
        var result2 = await Md5FileAsync(path2, bufferSize);

        return string.Equals(result1, result2, StringComparison.OrdinalIgnoreCase);
    }
}
