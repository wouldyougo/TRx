using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common;

namespace TRL.Configuration
{
    public class ConnectionAwaitingSettings
    {
        public int Attempts { get; private set; }
        public int MillisecondsBetweenAttempts { get; private set; }
        public ConnectionAwaitingSettings()
        {
            this.Attempts = AppSettings.GetValue<int>("ConnectionAwaitingAttempts");
            this.MillisecondsBetweenAttempts = AppSettings.GetValue<int>("SecondsBetweenConnectionAwaitingAttempts") * 1000;
        }
    }
}
