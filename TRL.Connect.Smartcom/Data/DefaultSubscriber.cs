using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Connect.Smartcom.Data
{
    public class DefaultSubscriber:SmartComSubscriber
    {
        private static DefaultSubscriber subscriber = null;

        public static DefaultSubscriber Instance
        {
            get
            {
                if (subscriber == null)
                    subscriber = new DefaultSubscriber();

                return subscriber;
            }
        }

        private DefaultSubscriber()
        {
        }
    }
}
