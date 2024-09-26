using System;
using System.IO;



namespace Roslan.DotNetUtils.IO {



    public class FileCompareFileInfo : ICompareFileInfo {

        #region "Fields"
        private readonly string _filePath;
        private readonly int _readBufferSize;
        #endregion

        #region "Properties

        public long Size {
            get {
                if (_size == -1) {
                    _size = new FileInfo(_filePath).Length;
                }

                return _size.Value;
            }
        }
        private long? _size = null;


        public bool SupportSize { get; }
        public bool SupportHash { get; }

        public DateTime LastWriteTime {
            get {
                if (_lastWriteTime == null) {
                    _lastWriteTime = File.GetLastWriteTime(_filePath);
                }
                return _lastWriteTime.Value;
            }
        }
        private DateTime? _lastWriteTime = null;
        public bool SupportLastWriteTime { get; }
        #endregion




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
