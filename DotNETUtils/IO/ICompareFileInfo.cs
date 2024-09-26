using System;
using System.Collections.Generic;
using System.IO;
using System.Text;



namespace Roslan.DotNetUtils.IO {



    public interface ICompareFileInfo {

        long Size { get; }
        bool SupportSize { get; }

        bool SupportHash { get; }



        /// <summary>
        /// Blazor calls this IBrowserFile.LastModified
        /// IO.FileInfo calls this LastWriteTime
        /// </summary>
        DateTime LastWriteTime { get; }
        bool SupportLastWriteTime { get; }


        Stream OpenReadStream();

    }
}
