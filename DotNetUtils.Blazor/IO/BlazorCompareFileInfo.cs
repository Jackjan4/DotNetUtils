using Microsoft.AspNetCore.Components.Forms;
using Roslan.DotNetUtils.IO;



namespace Roslan.DotNetUtils.Blazor.IO;



public class BlazorCompareFileInfo : ICompareFileInfo {



    #region "Fields"
    private readonly IBrowserFile _fileHandle;
    private readonly int _readBufferSize;
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
    public BlazorCompareFileInfo(IBrowserFile fileHandle, int readBufferSize) {
        _fileHandle = fileHandle;

        _readBufferSize = readBufferSize;
        SupportSize = true;

        SupportHash = false;

        SupportLastWriteTime = true;
    }



    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Stream OpenReadStream() {
        return _fileHandle.OpenReadStream(maxAllowedSize: 1024 * 1024 * 400);
    }
}