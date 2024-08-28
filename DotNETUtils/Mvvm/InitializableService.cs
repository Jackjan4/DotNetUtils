using System;
using System.Collections.Generic;
using System.Text;

namespace Roslan.DotNetUtils.Mvvm {
    public abstract class InitializableService {



        /// <summary>
        /// Determines whether the service has been initialized.
        /// </summary>
        public bool IsInitialized { get; private set; }

        public void Initialize() {
            if (IsInitialized)
                return;

            InitializeService();

            IsInitialized = true;
        }



        protected abstract void InitializeService();
        protected abstract void DeinitializeService();



        public void DeInitialize() {
            if (IsInitialized)
                DeinitializeService();
            IsInitialized = false;
        }
    }
}
