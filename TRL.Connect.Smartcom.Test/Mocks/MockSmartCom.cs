using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartCOM3Lib;

namespace TRL.Connect.Smartcom.Test.Mocks
{
    public class MockSmartCom
    {
        private static StServerClassMock stServer = null;

        public static StServer Instance
        {
            get
            {
                if (stServer == null)
                    stServer = new StServerClassMock();

                return stServer;
            }
        }

        public static void Destroy()
        {
            stServer = null;
        }

        public static bool IsNull
        {
            get
            {
                return stServer == null;
            }
        }

        private MockSmartCom() { }
    }
}
