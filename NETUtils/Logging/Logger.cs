using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Diagnostics.Tracing;
using System.Diagnostics;

namespace De.JanRoslan.NETUtils.Logging
{
    public class Logger
    {

        private List<LogEntry> log;


        private Logger()
        {

        }

        private Logger(Builder builder)
        {
           
        }


        private void Log(String message, LogLevel level = LogLevel.NONE, String header = null)
        {

        }


        public class Builder
        {


            public Builder()
            {

            }

            public Builder RegisterConsole(params LogLevel[] level)
            {
                // adswqadadwd

                return this;
            }


            public Builder RegisterFile(String file, params LogLevel[] level)
            {
                return this;
            }

            public Builder RegisterFile(String file, int rollingMax, params LogLevel[] level)
            {

                return this;
            }

            public Logger Build()
            {
                return new Logger(this);
            }


        }
    }


    
}
