using System;
using System.Collections.Generic;
using System.Text;

namespace De.JanRoslan.NETUtils.Logging
{
    interface ILoggingProvider
    {

        void Init(params LogLevel[] level);

        void Log(String message, String header = null);

    }
}
