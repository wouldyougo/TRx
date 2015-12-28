using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;

namespace TRL.Connect.Smartcom.Data
{
    public class SmartComHandlers:SmartComHandlersDatabase
    {
        private static SmartComHandlers theInstance = null;

        private SmartComHandlers() { }

        public static SmartComHandlers Instance
        {
            get
            {
                if (theInstance == null)
                    theInstance = new SmartComHandlers();

                return theInstance;
            }
        }
    }
}
