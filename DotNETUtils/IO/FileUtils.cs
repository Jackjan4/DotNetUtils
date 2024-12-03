using System;
#if NET8_0_OR_GREATER
using System.Buffers;
using System.Runtime.InteropServices;
#endif
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Roslan.DotNetUtils.Crypto;

namespace Roslan.DotNetUtils.IO {



    /// <summary>
    /// 
    /// </summary>
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



#if NET8_0_OR_GREATER // Async hashing only exists since .NET 5
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
        /// <param name="options"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void CopyFileDelta(string sourceFilePath, string destinationFilePath, FileCopyDeltaOptions options = null) {
            if (sourceFilePath == null) throw new ArgumentNullException(nameof(sourceFilePath));
            if (destinationFilePath == null) throw new ArgumentNullException(nameof(destinationFilePath));

            options = options ?? FileCopyDeltaOptions.Default;

            var filesEqual = false;

            if (File.Exists(destinationFilePath)) {
                var sourceFileInfo = new FileCompareFileInfo(sourceFilePath, options.CompareBufferSize);
                var destinationFileInfo = new FileCompareFileInfo(destinationFilePath, options.CompareBufferSize);

                filesEqual = CompareFileInfo(sourceFileInfo, destinationFileInfo, options.CompareMethod);
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
        /// <param name="options"></param>
        /// <param name="progress"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static async Task CopyFileDeltaAsync(string sourceFilePath, string destinationFilePath, FileCopyDeltaOptions options = null, IProgress<long> progress = null) {
            if (sourceFilePath == null) throw new ArgumentNullException(nameof(sourceFilePath));
            if (destinationFilePath == null) throw new ArgumentNullException(nameof(destinationFilePath));

            options = options ?? FileCopyDeltaOptions.Default;

            var sourceFileInfo = new FileCompareFileInfo(sourceFilePath, options.CompareBufferSize);

            var filesEqual = false;
            if (File.Exists(destinationFilePath)) {
                var destinationFileInfo = new FileCompareFileInfo(destinationFilePath, options.CompareBufferSize);

                filesEqual = await CompareFileInfoAsync(sourceFileInfo, destinationFileInfo, options.CompareMethod);
            }

            if (!filesEqual) {
                await CopyFileAsync(sourceFilePath, destinationFilePath, options.CopyBufferSize, progress).ConfigureAwait(false);
            } else {
                // File is equal
                if (options.ProgressEqualFile) {
                    progress?.Report(sourceFileInfo.Size);
                } else {
                    progress?.Report(-1); // Report that the file was not copied because it was equal by reporting -1
                }

            }
        }



        /// <summary>
        /// Copies an existing file to a new file asynchronously.
        /// The reason this method exists is because the built-in File.Copy method has no async version.
        /// This method uses FileStreams manually and can report progress
        /// For more information see:
        /// https://github.com/dotnet/runtime/issues/20697
        /// https://github.com/dotnet/runtime/issues/20695
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="destinationFilePath"></param>
        /// <param name="bufferSize"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static async Task CopyFileAsync(string sourceFilePath, string destinationFilePath, int bufferSize = 4096, IProgress<long> progress = null) {

            var sourceFs = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan);
            var destinationFs = new FileStream(destinationFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, bufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan);

            // TODO: Add await using for .NET 8
            using (sourceFs) {
                using (destinationFs) {
                    await StreamUtils.CopyToAsync(sourceFs, destinationFs, bufferSize, progress);
                }
            }

            // Recreate FileInfo properties
            File.SetLastWriteTime(destinationFilePath, File.GetLastWriteTime(sourceFilePath));
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceFileInfo"></param>
        /// <param name="destinationFileInfo"></param>
        /// <param name="compareMethod"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static bool CompareFileInfo(ICompareFileInfo sourceFileInfo, ICompareFileInfo destinationFileInfo, FileCompareMethod compareMethod) {
            var filesEqual = false;

            switch (compareMethod) {
                case FileCompareMethod.FileSize:
                    filesEqual = sourceFileInfo.Size.Equals(destinationFileInfo.Size);
                    break;
                case FileCompareMethod.Md5Hash:
                    if (!sourceFileInfo.SupportHash || !destinationFileInfo.SupportHash)
                        throw new NotSupportedException("Hash comparison is not supported for one or both files.");

                    var sourceStream = sourceFileInfo.OpenReadStream();
                    var destinationStream = destinationFileInfo.OpenReadStream();

                    var sourceHash = HashUtils.Md5(sourceStream);
                    var destinationHash = HashUtils.Md5(destinationStream);

                    filesEqual = sourceHash.SequenceEqual(destinationHash);
                    break;
                case FileCompareMethod.LastWriteTime:
                    filesEqual = sourceFileInfo.LastWriteTimeUtc.Equals(destinationFileInfo.LastWriteTimeUtc);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(compareMethod), compareMethod, null);
            }

            return filesEqual;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceFileInfo"></param>
        /// <param name="destinationFileInfo"></param>
        /// <param name="compareMethod"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static async Task<bool> CompareFileInfoAsync(ICompareFileInfo sourceFileInfo, ICompareFileInfo destinationFileInfo, FileCompareMethod compareMethod) {
            var filesEqual = false;

            switch (compareMethod) {
                case FileCompareMethod.FileSize:
                    filesEqual = sourceFileInfo.Size.Equals(destinationFileInfo.Size);
                    break;
#if NET8_0_OR_GREATER
                case FileCompareMethod.Md5Hash: {
                        if (!sourceFileInfo.SupportHash || !destinationFileInfo.SupportHash)
                            throw new NotSupportedException("Hash comparison is not supported for one or both files.");

                        await using var sourceStream = sourceFileInfo.OpenReadStream();
                        await using var destinationStream = destinationFileInfo.OpenReadStream();

                        var sourceHash = await HashUtils.Md5Async(sourceStream);
                        var destinationHash = await HashUtils.Md5Async(destinationStream);

                        filesEqual = sourceHash.SequenceEqual(destinationHash);
                        break;
                    }
#endif
                case FileCompareMethod.LastWriteTime:
                    filesEqual = sourceFileInfo.LastWriteTimeUtc.Equals(destinationFileInfo.LastWriteTimeUtc);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(compareMethod), compareMethod, null);
            }

            return filesEqual;
        }
    }
}