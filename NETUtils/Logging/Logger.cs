using System;
using System.Collections.Generic;

namespace De.JanRoslan.NETUtils.Logging
{
    public class Logger
    {

        private Dictionary<LogLevel, List<ILoggingProvider>> logProvider;
        public IEnumerable<ILoggingProvider> LogProvider {
            get {
                List<ILoggingProvider> enume = new List<ILoggingProvider>();

                foreach (List<ILoggingProvider> element in logProvider.Values) {
                    foreach (ILoggingProvider l in element) {
                        enume.Add(l);
                    }
                }
                return enume;
            }
        }


        private List<LogEntry> log;
        public IEnumerable<LogEntry> LogEntries {
            get { return log; }
        }

        public int LogCapacity { get; }



        private Logger() {

        }

        private Logger(Builder builder) {
            this.log = new List<LogEntry>();
            this.LogCapacity = builder.LogCapacity;
            this.logProvider = builder.LogProvider;
        }


        public void Log(String message, LogLevel level = LogLevel.NONE, String header = "") {

            if (logProvider.ContainsKey(level)) {
                foreach(ILoggingProvider provider in logProvider[level]) {
                    provider.Log(message, header);
                }
            }
        }



        /// <summary>
        /// Builds a logger that can be configured to log on different outputs
        /// Since there are many logger frameworks out there it is recommended to use this framework only when the target application is not able to use the standard logger inside .NET 4.7/Core 1.0
        /// </summary>
        public class Builder
        {

            internal int LogCapacity;
            internal Dictionary<LogLevel, List<ILoggingProvider>> LogProvider;

            public Builder() {
                LogCapacity = -1;
                LogProvider = new Dictionary<LogLevel, List<ILoggingProvider>>();
            }



            /// <summary>
            /// 
            /// </summary>
            /// <param name="level"></param>
            /// <returns></returns>
            public Builder RegisterConsole(params LogLevel[] level) {
                ConsoleLogger logger = new ConsoleLogger();

                AddLogprovider(logger, level);
                return this;
            }



            /// <summary>
            /// 
            /// </summary>
            /// <param name="prov"></param>
            /// <param name="level"></param>
            private void AddLogprovider(ILoggingProvider prov, params LogLevel[] level) {

                foreach (LogLevel lvl in level) {
                    if (!LogProvider.ContainsKey(lvl)) {
                        LogProvider[lvl] = new List<ILoggingProvider>();
                    }

                    LogProvider[lvl].Add(prov);
                }
            }



            /// <summary>
            /// 
            /// </summary>
            /// <param name="capacity"></param>
            /// <returns></returns>
            public Builder SetLogCapacity(int capacity) {

                return this;
            }



            /// <summary>
            /// 
            /// </summary>
            /// <param name="file"></param>
            /// <param name="level"></param>
            /// <returns></returns>
            public Builder RegisterFile(String file, params LogLevel[] level) {
                FileLogger logger = new FileLogger(file);

                AddLogprovider(logger, level);
                return this;
            }



            /// <summary>
            /// 
            /// </summary>
            /// <param name="file"></param>
            /// <param name="rollingMax"></param>
            /// <param name="level"></param>
            /// <returns></returns>
            public Builder RegisterFile(String file, int rollingMax, params LogLevel[] level) {
                FileLogger logger = new FileLogger(file, rollingMax);

                AddLogprovider(logger, level);

                return this;
            }



            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public Logger Build() {
                return new Logger(this);
            }


        }
    }



}
