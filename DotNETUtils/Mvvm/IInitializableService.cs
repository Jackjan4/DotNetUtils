using System;
using System.Collections.Generic;
using System.Text;

namespace Roslan.DotNetUtils.Mvvm {
    public interface IInitializableService {



        /// <summary>
        /// Determines whether the service has been initialized.
        /// </summary>
        public bool IsInitialized { get; }



        public void Initialize();



        public void Deinitialize();
    }
}
