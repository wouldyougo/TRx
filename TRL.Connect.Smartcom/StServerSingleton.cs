using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartCOM3Lib;
using TRL.Common.Data;

namespace TRL.Connect.Smartcom
{
    public class StServerSingleton:IGenericSingleton<StServer>
    {
        public StServer Instance
        {
            get { return SmartCom.Instance; }
        }

        public void Destroy()
        {
            SmartCom.Destroy();
        }

        public bool IsNull
        {
            get { return SmartCom.IsNull; }
        }
    }
}
