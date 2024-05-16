using System;
using System.Security.Cryptography;
using System.Text;

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
        return BitConverter.ToString(hashBytes);
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
        return BitConverter.ToString(hashBytes);
    }



    /// <summary>
    /// Computes the MD5 hash of the specified string using UTF8 encoding.
    /// </summary>
    /// <param name="str">The string to compute the hash for.</param>
    /// <returns>The computed MD5 hash as a hexadecimal converted string.</returns>
    public static string Md5(string str) {
        return Md5(str, Encoding.UTF8);
    }
}
