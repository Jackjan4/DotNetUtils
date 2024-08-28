using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;



namespace Roslan.DotNetUtils.Mvvm {
    public interface IInitializableServiceAsync {



        /// <summary>
        /// Determines whether the service has been initialized.
        /// </summary>
        public bool IsInitialized { get; set; }



        public Task InitializeAsync();



        public Task DeInitializeAsync();
    }
}
