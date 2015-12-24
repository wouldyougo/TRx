using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Logging
{
    public class NullLogger:ILogger
    {
        public void Log(string message)
        {
        }
    }
}
