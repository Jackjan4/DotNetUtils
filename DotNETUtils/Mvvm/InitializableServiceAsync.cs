using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;



namespace Roslan.DotNetUtils.Mvvm {
    public abstract class InitializableServiceAsync {



        /// <summary>
        /// Determines whether the service has been initialized.
        /// </summary>
        public bool IsInitialized { get; private set; }

        public async Task InitializeAsync() {
            if (IsInitialized)
                return;

            await InitializeServiceAsync();

            IsInitialized = true;
        }



        protected abstract Task InitializeServiceAsync();
        protected abstract Task DeinitializeServiceAsync();


        public async Task DeInitializeAsync() {
            if (IsInitialized)
                await DeinitializeServiceAsync();
            IsInitialized = false;
        }
    }
}
