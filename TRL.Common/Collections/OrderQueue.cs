using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;

namespace TRL.Common.Collections
{
    public class OrderQueue:ObservableQueue<Order>
    {
        private static OrderQueue queue = null;

        public static OrderQueue Instance
        {
            get
            {
                if (queue == null)
                    queue = new OrderQueue();

                return queue;
            }
        }

        private OrderQueue() { }
    }
}
