using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartCOM3Lib;
using TRL.Common.Data;

namespace TRL.Connect.Smartcom.Test.Mocks
{
    public class StServerMockSingleton:IGenericSingleton<StServer>
    {
        public StServer Instance
        {
            get { return MockSmartCom.Instance; }
        }

        public void Destroy()
        {
            MockSmartCom.Destroy();
        }

        public bool IsNull
        {
            get { return MockSmartCom.IsNull; }
        }
    }
}
