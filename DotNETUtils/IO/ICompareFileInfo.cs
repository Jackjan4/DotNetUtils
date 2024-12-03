using System;
using System.Collections.Generic;
using System.IO;
using System.Text;



namespace Roslan.DotNetUtils.IO {



    /// <summary>
    /// 
    /// </summary>
    public interface ICompareFileInfo {



        /// <summary>
        /// 
        /// </summary>
        long Size { get; }



        /// <summary>
        /// Blazor calls this IBrowserFile.LastModified
        /// IO.FileInfo calls this LastWriteTime
        /// </summary>
        DateTime LastWriteTimeUtc { get; }
        bool SupportLastWriteTime { get; }
        bool SupportSize { get; }

        bool SupportHash { get; }


        Stream OpenReadStream();

    }
}
