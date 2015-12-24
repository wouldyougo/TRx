using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public class BidAsk : IIdentified, IDateTime
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public string Symbol { get; set; }

        public int Row { get; set; }

        public int NRows { get; set; }

        public double Bid { get; set; }

        public double BidSize { get; set; }

        public double Ask { get; set; }

        public double AskSize { get; set; }

        public BidAsk() { }

        public BidAsk(int id, DateTime date, string symbol, int row, int rows, double bid, double bidVolume, double ask, double askVolume)
        {
            this.Id = id;
            this.DateTime = date;
            this.Symbol = symbol;
            this.Row = row;
            this.NRows = rows;
            this.Bid = bid;
            this.BidSize = bidVolume;
            this.Ask = ask;
            this.AskSize = askVolume;
        }
    }
}
