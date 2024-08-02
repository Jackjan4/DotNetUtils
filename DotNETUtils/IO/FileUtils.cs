using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Roslan.DotNetUtils.Crypto;

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




		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="bufferSize"></param>
		/// <returns></returns>
		public static async Task<byte[]> Md5HashAsync(string path, int bufferSize = 4096) {
#if NET8_0_OR_GREATER
			await using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan)) {
#elif NETSTANDARD2_0
			using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan)) {
#elif NET46
			using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan)) {
#endif
				return await HashUtils.Md5Async(fs);
			}
		}



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




		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="bufferSize"></param>
		/// <returns></returns>
		public static async Task<string> Md5HashStringAsync(string path, int bufferSize = 4096) {
#if NET8_0_OR_GREATER
			await using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan)) {
#elif NETSTANDARD2_0
			using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan)) {
#elif NET46
			using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan)) {
#endif
				return await HashUtils.Md5StringAsync(fs);
			}
		}



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



		/// <summary>
		/// 
		/// </summary>
		/// <param name="path1"></param>
		/// <param name="path2"></param>
		/// <param name="bufferSize"></param>
		/// <returns></returns>
		public static async Task<bool> CompareMd5FileHashesAsync(string path1, string path2, int bufferSize = 4096) {
#if NET8_0_OR_GREATER
			Task<byte[]> task1 = Md5HashAsync(path1, bufferSize);
			Task<byte[]> task2 = Md5HashAsync(path2, bufferSize);

			await Task.WhenAll(task1, task2);

			return task1.Result.SequenceEqual(task2.Result);
#elif NETSTANDARD2_0
			return CompareMd5FileHashes(path1, path2, bufferSize);
#elif NET46
			return CompareMd5FileHashes(path1, path2, bufferSize);
#endif
		}

		#endregion



		/// <summary>
		/// 
		/// </summary>
		/// <param name="sourceFilePath"></param>
		/// <param name="destinationFilePath"></param>
		/// <param name="options"></param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static void CopyFileDelta(string sourceFilePath, string destinationFilePath, FileCopyDeltaOptions options = null) {
			options = options ?? FileCopyDeltaOptions.Default;

			var filesEqual = false;
			if (File.Exists(destinationFilePath))
				switch (options.CompareMethod) {
					case FileCompareMethod.FileSize:
						filesEqual =
							new FileInfo(sourceFilePath).Length.Equals(new FileInfo(destinationFilePath).Length);
						break;
					case FileCompareMethod.Md5Hash:
						filesEqual = CompareMd5FileHashes(sourceFilePath, destinationFilePath, options.CompareBufferSize);
						break;
					case FileCompareMethod.LastWriteTime:
						filesEqual = File.GetLastWriteTime(sourceFilePath)
							.Equals(File.GetLastWriteTime(destinationFilePath));
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(options.CompareMethod), options.CompareMethod, null);
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
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static async Task CopyFileDeltaAsync(string sourceFilePath, string destinationFilePath, FileCopyDeltaOptions options = null) {
			options = options ?? FileCopyDeltaOptions.Default;

			var filesEqual = false;
			if (File.Exists(destinationFilePath))
				switch (options.CompareMethod) {
					case FileCompareMethod.FileSize:
						var sourceFileSize = new FileInfo(sourceFilePath).Length;
						var destinationFileSize = new FileInfo(destinationFilePath).Length;
						filesEqual = sourceFileSize.Equals(destinationFileSize);
						break;
					case FileCompareMethod.Md5Hash:
#if NETSTANDARD2_0
						filesEqual = CompareMd5FileHashes(sourceFilePath, destinationFilePath, options.CompareBufferSize); // TODO: Check if we maybe are able to make this async sometime
#elif NET8_0_OR_GREATER
						filesEqual = await CompareMd5FileHashesAsync(sourceFilePath, destinationFilePath, options.CompareBufferSize).ConfigureAwait(false);
#endif
						break;
					case FileCompareMethod.LastWriteTime:
						var sourceLastWriteTime = File.GetLastWriteTime(sourceFilePath);
						var destinationLastWriteTime = File.GetLastWriteTime(destinationFilePath);
						filesEqual = sourceLastWriteTime.Equals(destinationLastWriteTime);
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(options.CompareMethod), options.CompareMethod, null);
				}
			if (!filesEqual) {
				await CopyFileWithStreamAsync(sourceFilePath, destinationFilePath, options.CopyBufferSize).ConfigureAwait(false);
			}
		}



		/// <summary>
		/// Copies an existing file to a new file asynchronously.
		/// The reason this method exists is because the built-in File.Copy method has no async version.
		/// There exists FileStream.CopyToAsync(fs) which is used here.
		/// For more information see:
		/// https://github.com/dotnet/runtime/issues/20697
		/// https://github.com/dotnet/runtime/issues/20695
		/// </summary>
		/// <param name="sourceFilePath"></param>
		/// <param name="destinationFilePath"></param>
		/// <param name="bufferSize"></param>
		/// <returns></returns>
		public static async Task CopyFileWithStreamAsync(string sourceFilePath, string destinationFilePath, int bufferSize = 4096) {
#if NETSTANDARD2_0
			using (var sFs = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan)) {
				using (var dFs = new FileStream(destinationFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, bufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan)) {
#elif NET8_0_OR_GREATER
			await using (var sFs = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan)) {
				await using (var dFs = new FileStream(destinationFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, bufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan)) {
#elif NET46
			using (var sFs = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan)) {
				using (var dFs = new FileStream(destinationFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, bufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan)) {
#endif
					await sFs.CopyToAsync(dFs).ConfigureAwait(false);
				}
			}
			// Recreate FileInfo properties
			File.SetLastWriteTime(destinationFilePath, File.GetLastWriteTime(sourceFilePath));
		}
	}
}