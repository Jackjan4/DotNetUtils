using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Roslan.DotNETUtils.Crypto;

namespace Roslan.DotNETUtils.Files {
    public static class FileUtils {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public static byte[] Md5Hash(string path, int bufferSize = 4096) {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize)) {
                return HashUtils.Md5(fs);
            }
        }



#if NET8_0_OR_GREATER
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public static async Task<byte[]> Md5HashAsync(string path, int bufferSize = 4096) {
            await using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan);
            return await HashUtils.Md5Async(fs);
        }
#endif



        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public static string Md5HashString(string path, int bufferSize = 4096) {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize)) {
                return HashUtils.Md5String(fs);
            }
        }



#if NET8_0_OR_GREATER
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public static async Task<string> Md5HashStringAsync(string path, int bufferSize = 4096) {
            await using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan);
            return await HashUtils.Md5StringAsync(fs);
        }
#endif



        /// <summary>
        /// 
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public static bool CompareMd5FileHashes(string path1, string path2, int bufferSize = 4096) {
            byte[] result1 = Md5Hash(path1, bufferSize);
            byte[] result2 = Md5Hash(path2, bufferSize);

            return result1.SequenceEqual(result2);
        }



#if NET8_0_OR_GREATER
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public static async Task<bool> CompareMd5FileHashesAsync(string path1, string path2, int bufferSize = 4096) {
            Task<byte[]> task1 = Md5HashAsync(path1, bufferSize);
            Task<byte[]> task2 = Md5HashAsync(path2, bufferSize);

            await Task.WhenAll(task1, task2);

            return task1.Result.SequenceEqual(task2.Result);
        }
#endif
    }
}
