using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;

namespace TRL.Common.Collections
{
    public class OrderBookEntryArray
    {
        private int length;
        private OrderBookEntry[] array;

        public OrderBookEntryArray(int length)
        {
            this.length = length;
            this.array = new OrderBookEntry[this.length];

            for (int i = 0; i < this.length; i++)
                this.array[i] = new OrderBookEntry();
        }

        public OrderBookEntry this[int index]
        {
            get
            {
                return this.array[index];
            }
        }

        public int Length
        {
            get
            {
                return this.length;
            }
        }

        public void Update(int position, double price, double volume)
        {
            if (position >= this.array.Length)
                return;

            this[position].Price = price;
            this[position].Volume = volume;
        }
    }
}
