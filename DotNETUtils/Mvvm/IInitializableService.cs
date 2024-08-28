using System;
using System.Collections.Generic;
using System.Text;

namespace Roslan.DotNetUtils.Mvvm {
    public interface IInitializableService {



        /// <summary>
        /// Determines whether the service has been initialized.
        /// </summary>
        bool IsInitialized { get; }



        void Initialize();



        void Deinitialize();
    }
}
