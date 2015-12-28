using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Connect.Smartcom.Models
{
    public class UpdateBidAsk
    {
        private string symbol;
        private int row;
        private int nrows;
        private double bid;
        private double bidSize;
        private double ask;
        private double askSize;

        public UpdateBidAsk(string symbol, int row, int nrows, double bid, double bidsize, double ask, double asksize)
        {
            this.symbol = symbol;
            this.row = row;
            this.nrows = nrows;
            this.bid = bid;
            this.bidSize = bidsize;
            this.ask = ask;
            this.askSize = asksize;
        }

        public string Symbol
        {
            get
            {
                return this.symbol;
            }
        }

        public int Row
        {
            get
            {
                return this.row;
            }
        }

        public int NRows
        {
            get
            {
                return this.nrows;
            }
        }

        public double Bid
        {
            get
            {
                return this.bid;
            }
        }

        public double BidSize
        {
            get
            {
                return this.bidSize;
            }
        }

        public double Ask
        {
            get
            {
                return this.ask;
            }
        }

        public double AskSize
        {
            get
            {
                return this.askSize;
            }
        }
    }
}
