using Roslan.DotNetUtils.Crypto.Exceptions;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Roslan.DotNetUtils.Crypto {


    /// <summary>
    /// 
    /// </summary>
    public static class AES {
        public const int AES256_KEYLENGTH_BYTE = 32;
        public const int AES256_IVLENGTH_BYTE = 16;





        /// <summary>
        ///  Encrypts the given plaintext with the given key (the key is converted with the GetKey(...) method)
        ///  If true, appendIV will append the IV to the byte-array (first 16 bytes)
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="key"></param>
        /// <param name="appendIV"></param>
        /// <returns></returns>
        public static byte[] Encrypt(String plainText, String key, bool appendIV) {
            return Encrypt(plainText, GetKey(key, 32), appendIV);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="key"></param>
        /// <param name="appendIV"></param>
        /// <returns></returns>
        public static byte[] Encrypt(string plainText, byte[] key, bool appendIV) {
            using (Aes myAes = Aes.Create()) {
                return Encrypt(plainText, key, myAes.IV, appendIV);
            }
        }



        /// <summary>
        /// Enrypts a UTF-8 encoded string with AES
        /// </summary>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        public static byte[] Encrypt(string plainText, byte[] key, byte[] iv, bool appendIV) {
            byte[] result = null;

            using (Aes myAes = Aes.Create()) {

                // Error - Wrong key length
                if (key.Length != myAes.KeySize / 8) {
                    myAes.Key = key;
                    return null;
                }

                // Error - Wrong iv length
                if (iv.Length == 16)
                    myAes.IV = iv;


                ICryptoTransform encryptor = myAes.CreateEncryptor(key, iv);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream()) {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)) {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt)) {

                            // Write all data to the stream.
                            if (appendIV)
                                msEncrypt.Write(iv, 0, iv.Length);
                            swEncrypt.Write(plainText);
                        }
                        result = msEncrypt.ToArray();
                    }
                }
            }

            return result;
        }



        /// <summary>
        /// Constructs a valid AES-256 key with a given string
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static byte[] GetKey(string key, int wishedLength) {
            // Convert string to byte-Array
            byte[] byteKey = Encoding.UTF8.GetBytes(key);

            if (byteKey.Length < wishedLength) 
                return null;

            var resultArray = new byte[wishedLength];
            Array.Copy(byteKey, resultArray, wishedLength);
            return resultArray;
        }


        /// <summary>
        /// Decrypts a byte stream to a UTF-8 encoded string
        /// </summary>
        /// <returns>The decrypted stream</returns>
        /// <exception cref="CryptographicException">Will be thrown when the key is wrong</exception>
        public static string Decrypt(byte[] cipherText, byte[] key, byte[] iv) {
            string result = null;

            // Check arguments.
            if (cipherText != null && cipherText.Length > 0)

                if (key != null && key.Length > 0)

                    if (iv != null || iv.Length > 0)

                        // Create an Aes object
                        // with the specified key and IV.
                        using (Aes aesAlg = Aes.Create()) {
                            aesAlg.Key = key;
                            aesAlg.IV = iv;

                            // Create a decrytor to perform the stream transform.
                            ICryptoTransform decryptor = aesAlg.CreateDecryptor(key, iv);

                            // Create the streams used for decryption.
                            using (MemoryStream msDecrypt = new MemoryStream(cipherText)) {
                                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)) {
                                    try {
                                        using (StreamReader srDecrypt = new StreamReader(csDecrypt)) {

                                            // Read the decrypted bytes from the decrypting stream and place them in a string.
                                            result = srDecrypt.ReadToEnd();
                                        }
                                    } catch (CryptographicException ex) {
                                        throw new BadKeyException();
                                    }
                                }
                            }

                        }

            return result;
        }


        /// <summary>
        /// Decrypts a cypthertext (noted in hex) with the given key. IV must be included in the beginning of the cyphertext!
        /// </summary>
        /// <param name="cyphertext"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Decrypt(string cyphertext, string key) {
            byte[] byteKey = GetKey(key, AES256_KEYLENGTH_BYTE);

            // Get IV
            byte[] iv = new byte[AES256_IVLENGTH_BYTE];
            for (int i = 0; i < iv.Length; i++) {
                iv[i] = Convert.ToByte(cyphertext.Substring(i * 2, 2), 16);
            }

            // Get cypher message
            byte[] cypherBytes = new byte[cyphertext.Length / 2 - AES256_IVLENGTH_BYTE];

            for (int i = AES256_IVLENGTH_BYTE * 2, j = 0; i < cyphertext.Length; i += 2, j++) {
                cypherBytes[j] = Convert.ToByte(cyphertext.Substring(i, 2), 16);
            }


            string result;
            try {
                result = Decrypt(cypherBytes, byteKey, iv);
            } catch (BadKeyException ex) {
				throw ex;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cyphertext"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string Decrypt(string cyphertext, string key, string iv) {
            string result;
            try {
                result = Decrypt(iv + cyphertext, key);
            } catch (BadKeyException ex) {
                throw ex;
            }

            return result;
        }
    }
}
