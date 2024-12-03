using System;
using System.IO;



namespace Roslan.DotNetUtils.IO {



    /// <summary>
    /// 
    /// </summary>
    public class FileCompareFileInfo : ICompareFileInfo {



        #region "Fields"

        private readonly string _filePath;
        private readonly int _readBufferSize;
        #endregion



        #region "Properties

        public long Size {
            get {
                if (_size == null) {
                    _size = new FileInfo(_filePath).Length;
                }
                return _size.Value;
            }
        }
        private long? _size;



        /// <summary>
        /// 
        /// </summary>
        public DateTime LastWriteTimeUtc {
            get {
                if (_lastWriteTimeUtc == null) {
                    _lastWriteTimeUtc = new FileInfo(_filePath).LastWriteTimeUtc;
                }
                return _lastWriteTimeUtc.Value;
            }
        }

        private DateTime? _lastWriteTimeUtc;
        public bool SupportLastWriteTime { get; }
        public bool SupportSize { get; }
        public bool SupportHash { get; }
        #endregion



        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="readBufferSize"></param>
        public FileCompareFileInfo(string filePath, int readBufferSize) {
            _filePath = filePath;
            _readBufferSize = readBufferSize;

            SupportSize = true;
            SupportHash = true;
            SupportLastWriteTime = true;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Stream OpenReadStream() {
            return new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read, _readBufferSize);
        }
    }
}
