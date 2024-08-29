using System;

namespace Roslan.DotNetUtils.IO {



	/// <summary>
	/// 
	/// </summary>
	public class FileCopyDeltaOptions {

		public readonly int CompareBufferSize;

		public readonly int CopyBufferSize;

		public readonly FileCompareMethod CompareMethod;

        public readonly bool ProgressEqualFile;


		public static FileCopyDeltaOptions Default => new FileCopyDeltaOptions();

		private FileCopyDeltaOptions() {
			CompareBufferSize = 4096;
			CopyBufferSize = 4096;
			CompareMethod = FileCompareMethod.LastWriteTime;
            ProgressEqualFile = true;
        }

		public FileCopyDeltaOptions(int compareBufferSize, int copyBufferSize, FileCompareMethod compareMethod, bool progressEqualFile) {
			CompareBufferSize = compareBufferSize;
			CopyBufferSize = copyBufferSize;
			CompareMethod = compareMethod;
            ProgressEqualFile = progressEqualFile;
        }
	}
}
