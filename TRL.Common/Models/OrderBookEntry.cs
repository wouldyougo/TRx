using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public class OrderBookEntry
    {
        public double Price { get; set; }
        public double Volume { get; set; }

        public OrderBookEntry() : this(0, 0) { }

        public OrderBookEntry(double price, double volume)
        {
            this.Price = price;
            this.Volume = volume;
        }
    }
}
