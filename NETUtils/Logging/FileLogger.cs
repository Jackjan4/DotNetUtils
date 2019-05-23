using System;
using System.Collections.Generic;
using System.Text;

namespace De.JanRoslan.NETUtils.Logging
{
    class FileLogger : ILoggingProvider
    {

        public LogLevel[] logLevels;
        public LogLevel[] LogLevels {
            get {
                return logLevels;
            }
        }

        private String file;
        private String currentFile;

        private int maxRolling;

        private object logLock;


        public FileLogger(String file) : this(file, -1) {
        }

        public FileLogger(String file, int maxRolling) {
            logLevels = null;
            this.file = file;
            this.maxRolling = maxRolling;
            this.currentFile = file;
            logLock = new object();
        }



        public void Init(params LogLevel[] level) {
            logLevels = level;
        }

        public void Log(string message, string header = null) {
            Log(message, LogLevel.NONE, header);
        }

        public void Log(string message, LogLevel level, string header = "") {
            String headerBuild = "";

            if (header != "") {
                headerBuild = "[" + header + "] ";
            }

            lock (logLock) {
                try
                {
                    System.IO.File.AppendAllText(file, "[" + DateTime.Now.ToString("HH: mm:ss") + "] : " + headerBuild + "\t" + message + Environment.NewLine, System.Text.Encoding.UTF8);
                } catch(Exception ex) {

                }
                
            }
        }
    }
}
