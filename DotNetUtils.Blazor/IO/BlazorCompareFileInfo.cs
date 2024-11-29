using Microsoft.AspNetCore.Components.Forms;
using Roslan.DotNetUtils.IO;



namespace Roslan.DotNetUtils.Blazor.IO;



public class BlazorCompareFileInfo : ICompareFileInfo {



    #region "Fields"
    private readonly IBrowserFile _fileHandle;
    private readonly int _readBufferSize;

    private readonly int _maxAllowedSize;
    #endregion



    #region "Properties"
    public long Size => _size ??= _fileHandle.Size;
    private long? _size;

    public bool SupportSize { get; }
    public bool SupportHash { get; }

    public DateTime LastWriteTime => _lastWriteTime ??= _fileHandle.LastModified.DateTime;
    private DateTime? _lastWriteTime;
    public bool SupportLastWriteTime { get; }
    #endregion



    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileHandle"></param>
    /// <param name="readBufferSize"></param>
    /// <param name="maxAllowedSize"></param>
    public BlazorCompareFileInfo(IBrowserFile fileHandle, int readBufferSize, int maxAllowedSize = 512000) {
        _fileHandle = fileHandle;
        _readBufferSize = readBufferSize;
        _maxAllowedSize = maxAllowedSize;

        SupportSize = true;
        SupportHash = true;
        SupportLastWriteTime = true;
    }



    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Stream OpenReadStream() {
        return _fileHandle.OpenReadStream(_maxAllowedSize);
    }
}