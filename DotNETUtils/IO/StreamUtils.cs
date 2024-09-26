using System;
#if NET8_0_OR_GREATER
using System.Buffers;
#endif
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Roslan.DotNetUtils.IO {
    public static class StreamUtils {



        /// <summary>
        /// Copies the content of a stream to another stream. While copying, it reports the progress.
        /// While Stream.CopyToAsync(...) exists since .NET Framework 4.5, it does not report the progress.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="bufferSize"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static async Task CopyToAsync(Stream source, Stream destination, int bufferSize = 4096, IProgress<long> progress = null) {
#if (NETSTANDARD2_0 || NET46)
                    var buffer = new byte[bufferSize];
                    int bytesRead;
                    while ((bytesRead = await source.ReadAsync(buffer, 0, bufferSize).ConfigureAwait(false)) != 0) {
                        await destination.WriteAsync(buffer, 0, bufferSize).ConfigureAwait(false);
                        progress?.Report(bytesRead);
                    }
#elif NET8_0_OR_GREATER
                    var buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
                    try {
                        int bytesRead;
                        while ((bytesRead = await source.ReadAsync(new Memory<byte>(buffer)).ConfigureAwait(false)) != 0) {
                            await destination.WriteAsync(new ReadOnlyMemory<byte>(buffer, 0, bytesRead)).ConfigureAwait(false);
                            progress?.Report(bytesRead);
                        }
                    } finally {
                        // This finally block is copied from the .NET source code. (.NET 8 implementation of Stream.CopyToAsync)
                        ArrayPool<byte>.Shared.Return(buffer);
                    }
#endif
        }
    }
}
