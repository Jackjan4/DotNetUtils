using System;
using System.Text;

namespace De.JanRoslan.NETUtils.Logging
{
    public class ConsoleLogger : ILoggingProvider
    {

        private LogLevel[] logLevels;

        public LogLevel[] LogLevels {
            get {
                return logLevels;
            }
        }

        public void Init(params LogLevel[] level) {
            this.logLevels = level;
        }


        public void Log(string message, string header = "") {
            Log(message, LogLevel.NONE, header);
        }

        public void Log(string message, LogLevel level, string header = "") {
            String headerBuild = "";

            if (header != "") {
                headerBuild = "[" + header + "] ";
            }
            System.Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] : "  + headerBuild + "\t" + message, Encoding.UTF8);
        }
    }
}
