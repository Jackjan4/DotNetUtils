using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;



namespace Roslan.DotNetUtils.Mvvm {
    public interface IInitializableServiceAsync {



        /// <summary>
        /// Determines whether the service has been initialized.
        /// </summary>
        bool IsInitialized { get; set; }



        Task InitializeAsync();



        Task DeInitializeAsync();
    }
}
