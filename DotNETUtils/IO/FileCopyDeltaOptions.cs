using System;

namespace Roslan.DotNetUtils.IO {



	/// <summary>
	/// 
	/// </summary>
	public class FileCopyDeltaOptions {

		public readonly int CompareBufferSize;

		public readonly int CopyBufferSize;

		public FileCompareMethod CompareMethod;


		public static FileCopyDeltaOptions Default => new FileCopyDeltaOptions();

		private FileCopyDeltaOptions() {
			CompareBufferSize = 4096;
			CopyBufferSize = 4096;
			CompareMethod = FileCompareMethod.Md5Hash;
		}

		public FileCopyDeltaOptions(int compareBufferSize, int copyBufferSize, FileCompareMethod compareMethod) {
			CompareBufferSize = compareBufferSize;
			CopyBufferSize = copyBufferSize;
			CompareMethod = compareMethod;
		}
	}
}
