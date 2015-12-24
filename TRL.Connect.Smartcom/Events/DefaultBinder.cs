using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Connect.Smartcom.Events
{
    public class DefaultBinder:SmartComBinder
    {
        private static DefaultBinder binder = null;

        public static DefaultBinder Instance
        {
            get
            {
                if (binder == null)
                    binder = new DefaultBinder();

                return binder;
            }
        }

        private DefaultBinder()
        {
        }
    }
}
