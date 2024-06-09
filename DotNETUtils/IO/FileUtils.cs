using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Roslan.DotNETUtils.Crypto;

namespace Roslan.DotNetUtils.IO {
    public static class FileUtils {


        #region "File Hashing"
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
        #endregion



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="destinationFilePath"></param>
        /// <param name="compareMethod"></param>
        /// <param name="bufferSize"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void CopyFileDelta(string sourceFilePath, string destinationFilePath, FileCompareMethod compareMethod, int bufferSize = 4096) {
            // parameter bufferSize should be replaced with DeltaComparionOptions because not every delta comparison needs to open the files
            var filesEqual = false;

            if (File.Exists(destinationFilePath))
                switch (compareMethod) {
                    case FileCompareMethod.FileSize:
                        filesEqual =
                            new FileInfo(sourceFilePath).Length.Equals(new FileInfo(destinationFilePath).Length);
                        break;
                    case FileCompareMethod.Md5Hash:
                        filesEqual = CompareMd5FileHashes(sourceFilePath, destinationFilePath, bufferSize);
                        break;
                    case FileCompareMethod.LastWriteTime:
                        filesEqual = File.GetLastWriteTime(sourceFilePath)
                            .Equals(File.GetLastWriteTime(destinationFilePath));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(compareMethod), compareMethod, null);
                }

            if (!filesEqual) {
                File.Copy(sourceFilePath, destinationFilePath, true);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="destinationFilePath"></param>
        /// <param name="compareMethod"></param>
        /// <param name="bufferSize"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static async Task CopyFileDeltaAsync(string sourceFilePath, string destinationFilePath, FileCompareMethod compareMethod, int bufferSize = 4096) {
            // parameter bufferSize should be replaced with DeltaComparionOptions because not every delta comparison needs to open the files
            var filesEqual = false;

            if (File.Exists(destinationFilePath))
                switch (compareMethod) {
                    case FileCompareMethod.FileSize:
                        filesEqual =
                            new FileInfo(sourceFilePath).Length.Equals(new FileInfo(destinationFilePath).Length);
                        break;
                    case FileCompareMethod.Md5Hash:
#if NETSTANDARD2_0
                        filesEqual = CompareMd5FileHashes(sourceFilePath, destinationFilePath, bufferSize); // TODO: Check if we maybe are able to make this async sometime
#elif NET8_0_OR_GREATER
                        filesEqual = await CompareMd5FileHashesAsync(sourceFilePath, destinationFilePath, bufferSize);
#endif
                        break;
                    case FileCompareMethod.LastWriteTime:
                        filesEqual = File.GetLastWriteTime(sourceFilePath)
                            .Equals(File.GetLastWriteTime(destinationFilePath));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(compareMethod), compareMethod, null);
                }

            if (!filesEqual) {
#if NETSTANDARD2_0
                using (var sFs = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous | FileOptions.SequentialScan)) {
                    using (var dFs = new FileStream(destinationFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous | FileOptions.SequentialScan)) {
#elif NET8_0_OR_GREATER
                await using (var sFs = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous | FileOptions.SequentialScan)) {
                    await using (var dFs = new FileStream(destinationFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous | FileOptions.SequentialScan)) {
#endif
                        await sFs.CopyToAsync(dFs);
                    }
                }
            }
        }
    }
}
