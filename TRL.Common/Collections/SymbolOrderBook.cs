using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;

namespace TRL.Common.Collections
{
    public class SymbolOrderBook:INamed
    {
        private string name;
        private int depth;
        private OrderBookEntryArray offers, bids;

        public SymbolOrderBook(string name, int depth)
        {
            this.name = name;
            this.depth = depth;
            this.offers = new OrderBookEntryArray(this.depth);
            this.bids = new OrderBookEntryArray(this.depth);
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public int Depth
        {
            get
            {
                return this.depth;
            }
        }

        public double GetOfferPrice(int index)
        {
            return this.offers[index].Price;
        }

        public double GetBidPrice(int index)
        {
            return this.bids[index].Price;
        }

        public double GetOfferVolume(int index)
        {
            return this.offers[index].Volume;
        }

        public double GetBidVolume(int index)
        {
            return this.bids[index].Volume;
        }

        public void Update(int index, double bid, double bidVolume, double offer, double offerVolume)
        {
            this.bids.Update(index, bid, bidVolume);
            this.offers.Update(index, offer, offerVolume);
        }
    }
}
