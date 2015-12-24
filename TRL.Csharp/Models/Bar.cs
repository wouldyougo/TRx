using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRL.Csharp.Models
{
    public class Bar
    {
        public DateTime DateTime;
        public int IntervalSeconds;
        public string Symbol;
        public double Open;
        public double High;
        public double Low;
        public double Close;
        public double Volume;

        public Bar(DateTime date, int intervalSeconds, string symbol, double open, double high, double low, double close, double volume)
        {
            DateTime = date;
            IntervalSeconds = intervalSeconds;
            Symbol = symbol;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }
    }
}
