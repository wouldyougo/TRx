using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public enum TrendDirection
    {
        Flat,
        Up,
        Down
    }

    public class Trend
    {
        public int Interval { get; set; }
        public string Symbol { get; set; }
        public TrendDirection Direction { get; set; }
        public double Speed { get; set; }

        public Trend() { }

        public Trend(int interval, string symbol, TrendDirection direction, double speed)
        {
            this.Interval = interval;
            this.Symbol = symbol;
            this.Direction = direction;
            this.Speed = speed;
        }
    }
}
