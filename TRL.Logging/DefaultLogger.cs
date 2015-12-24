using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Logging
{
    public class DefaultLogger:TextFileLogger
    {
        private static DefaultLogger logger = null;

        public static DefaultLogger Instance
        {
            get
            {
                if (logger == null)
                    logger = new DefaultLogger();

                return logger;
            }
        }

        private DefaultLogger():base("smartcom", 1000000)
        {
        }
    }
}
