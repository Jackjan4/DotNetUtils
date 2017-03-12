using System;
using System.Collections.Generic;
using System.Text;

namespace De.JanRoslan.NETUtils.Logging
{
    public interface ILoggingProvider
    {

        LogLevel[] LogLevels { get; }

        void Init(params LogLevel[] level);

        void Log(String message, String header = "");

        void Log(String message, LogLevel level, String header = "");

    }
}
