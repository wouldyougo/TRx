using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Logging
{
    /// <summary>
    /// Логгер
    /// </summary>
    public interface ILogger
    {
        void Log(string message);
    }
}
