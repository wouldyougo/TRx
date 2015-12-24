using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Collections;

namespace TRL.Common.Data
{
    /// <summary>
    /// Контекст очереди заявок (стакана)
    /// </summary>
    public class OrderBookContext:IQuoteProvider
    {
        private int depth;
        private List<SymbolOrderBook> books;
        
        public OrderBookContext(int depth = 50)
        {
            this.depth = depth;
            this.books = new List<SymbolOrderBook>();
        }

        public int Depth
        {
            get
            {
                return this.depth;
            }
        }

        public double GetOfferPrice(string symbol, int index)
        {
            if(this.books.Any(b=>b.Name.Equals(symbol)))
                return this.books.Single(b => b.Name.Equals(symbol)).GetOfferPrice(index);

            return 0;
        }

        public double GetBidPrice(string symbol, int index)
        {
            if (this.books.Any(b => b.Name.Equals(symbol)))
                return this.books.Single(b => b.Name.Equals(symbol)).GetBidPrice(index);

            return 0;
        }

        public double GetOfferVolume(string symbol, int index)
        {
            if (this.books.Any(b => b.Name.Equals(symbol)))
                return this.books.Single(b => b.Name.Equals(symbol)).GetOfferVolume(index);

            return 0;
        }

        public double GetBidVolume(string symbol, int index)
        {
            if (this.books.Any(b => b.Name.Equals(symbol)))
                return this.books.Single(b => b.Name.Equals(symbol)).GetBidVolume(index);

            return 0;
        }

        public void Update(int index, string symbol, double bid, double bidVolume, double offer, double offerVolume)
        {
            if (!this.books.Any(b => b.Name.Equals(symbol)))
                this.books.Add(new SymbolOrderBook(symbol, this.depth));

            this.books.Single(b => b.Name.Equals(symbol)).Update(index, bid, bidVolume, offer, offerVolume);

            if (this.OnQuotesUpdate != null)
                this.OnQuotesUpdate(symbol);
        }

        public event SymbolDataUpdatedNotification OnQuotesUpdate;
    }
}
