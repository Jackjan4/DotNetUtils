using System;

namespace Roslan.DotNetUtils.IO {



    /// <summary>
    /// 
    /// </summary>
    public class FileCopyDeltaOptions {

        public int CompareBufferSize = 4096;

        public int CopyBufferSize = 4096;

        public FileCompareMethod CompareMethod = FileCompareMethod.Md5Hash;


        public static FileCopyDeltaOptions Default => new FileCopyDeltaOptions();

        private FileCopyDeltaOptions() {
        }

        public FileCopyDeltaOptions(int compareBufferSize, int copyBufferSize, FileCompareMethod compareMethod) {
            CompareBufferSize = compareBufferSize;
            CopyBufferSize = copyBufferSize;
            CompareMethod = compareMethod;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="compareMethod"></param>
        /// <returns></returns>
        internal static FileCompareMethod ConvertFileCompareMethodFromDotNetUtils(DotNetUtils.IO.FileCompareMethod compareMethod) {
            return (FileCompareMethod)Enum.Parse(typeof(FileCompareMethod), compareMethod.ToString(), true);
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compareMethod"></param>
        /// <returns></returns>
        internal static DotNetUtils.IO.FileCompareMethod ConvertFileCompareMethodToDotNetUtils(FileCompareMethod compareMethod) {
            return (DotNetUtils.IO.FileCompareMethod)Enum.Parse(typeof(DotNetUtils.IO.FileCompareMethod), compareMethod.ToString(), true);
        }

    }
}
