using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;

namespace TRL.Common.Collections
{
    public class SignalQueue:ObservableQueue<Signal>
    {
        private static SignalQueue queue = null;

        public static SignalQueue Instance
        {
            get
            {
                if (queue == null)
                    queue = new SignalQueue();

                return queue;
            }
        }

        private SignalQueue() { }
    }
}
