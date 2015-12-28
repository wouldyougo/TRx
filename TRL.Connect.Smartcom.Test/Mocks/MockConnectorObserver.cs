using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Events;

namespace TRL.Connect.Smartcom.Test.Mocks
{
    public class MockConnectorObserver:IObserver
    {
        public bool DisconnectDetected { get; set; }

        public void Update()
        {
            this.DisconnectDetected = true;
        }
    }
}
