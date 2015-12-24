using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Data
{
    public class OrderBook:OrderBookContext
    {
        private static OrderBook orderBook = null;

        public static OrderBook Instance
        {
            get
            {
                if (orderBook == null)
                    orderBook = new OrderBook();

                return orderBook;
            }
        }

        private OrderBook() { }
        
    }
}
