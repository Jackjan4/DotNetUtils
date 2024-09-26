using Microsoft.AspNetCore.Components.Forms;
using Roslan.DotNetUtils.IO;



namespace Roslan.DotNetUtils.Blazor.IO;


public class FileUtils {



    /// <summary>
    /// Uploads a file to a Blazor application using a delta copy method.
    /// Delta copy means that the file will only be copied if it is different from the destination file
    /// </summary>
    /// <param name="sourceFileHandle"></param>
    /// <param name="destinationFilePath"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static void UploadFileDelta(IBrowserFile sourceFileHandle, string destinationFilePath, FileCopyDeltaOptions? options = null) {
        options = options ?? FileCopyDeltaOptions.Default;

        var filesEqual = false;

        if (File.Exists(destinationFilePath)) {
            var sourceFileInfo = new BlazorCompareFileInfo(sourceFileHandle, options.CompareBufferSize);
            var destinationFileInfo = new FileCompareFileInfo(destinationFilePath, options.CompareBufferSize);

            filesEqual = Roslan.DotNetUtils.IO.FileUtils.CompareFileInfo(sourceFileInfo, destinationFileInfo, options.CompareMethod);
        }

        if (filesEqual) return;

        using (var sourceStream = sourceFileHandle.OpenReadStream(maxAllowedSize: 1024 * 1024 * 400)) {
            using (var destinationStream = File.Open(destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None)) {
                sourceStream.CopyTo(destinationStream, options.CopyBufferSize);
            }
        }

        // Recreate FileInfo properties
        var dateTime = sourceFileHandle.LastModified.DateTime;
        File.SetLastWriteTime(destinationFilePath, dateTime);
    }



    /// <summary>
    /// Uploads a file to a Blazor application using a delta copy method asynchronously.
    /// Delta copy means that the file will only be copied if it is different from the destination file
    /// </summary>
    /// <param name="sourceFileHandle"></param>
    /// <param name="destinationFilePath"></param>
    /// <param name="options"></param>
    /// <param name="progress"></param>
    /// <returns></returns>
    public static async Task UploadFileDeltaAsync(IBrowserFile sourceFileHandle, string destinationFilePath, FileCopyDeltaOptions? options = null, IProgress<long>? progress = null) {
        options ??= FileCopyDeltaOptions.Default;

        var filesEqual = false;

        // We declare sourceFileInfo here to be able to use it in the progress report
        var sourceFileInfo = new BlazorCompareFileInfo(sourceFileHandle, options.CompareBufferSize);

        if (File.Exists(destinationFilePath)) {
            var destinationFileInfo = new FileCompareFileInfo(destinationFilePath, options.CompareBufferSize);

            filesEqual = await Roslan.DotNetUtils.IO.FileUtils.CompareFileInfoAsync(sourceFileInfo, destinationFileInfo, options.CompareMethod);
        }

        if (!filesEqual) {
            await using var sourceStream = sourceFileHandle.OpenReadStream(maxAllowedSize: 1024 * 1024 * 400);
            await using var destinationStream = File.Open(destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await StreamUtils.CopyToAsync(sourceStream, destinationStream, options.CopyBufferSize, progress);
        } else {
            // File is equal
            if (options.ProgressEqualFile) {
                progress?.Report(sourceFileInfo.Size);
            } else {
                progress?.Report(-1); // Report that the file was not copied because it was equal by reporting -1
            }
        }

        // Recreate FileInfo properties
        var dateTime = sourceFileHandle.LastModified.DateTime;
        File.SetLastWriteTime(destinationFilePath, dateTime);
    }

}