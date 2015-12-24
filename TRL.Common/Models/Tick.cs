using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using TRL.Common.Extensions;

namespace TRL.Common.Models
{
    public enum TradeAction
    {
        Buy = 0,
        Sell = 1
    }

    public class Tick : IDateTime
    {
        public string Symbol { get; set; }
        public DateTime DateTime { get; set; }
        public double Price { get; set; }
        public double Volume { get; set; }
        public TradeAction TradeAction { get; set; }

        public Tick() { }

        public Tick(string symbol, DateTime date, TradeAction action, double price, double volume)
        {
            this.Symbol = symbol;
            this.DateTime = date;
            this.TradeAction = action;
            this.Price = price;
            this.Volume = volume;
        }

        public override string ToString()
        {
            return ToString("Symbol: {0}, DateTime: {1}, Price: {2}, Volume: {3}, TradeAction: {4}");
        }

        public string ToImportString()
        {
            return ToString("{0},{1},{2},{3},{4}");
        }

        private string ToString(string format)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            return String.Format(format,
                this.Symbol, this.DateTime.ToString(ci), this.Price.ToString("0.0000", ci), this.Volume.ToString("0.0000", ci), this.TradeAction);
        }


        public static Tick Parse(string tickString)
        {
            string[] parts = tickString.Split(',');

            CultureInfo provider = CultureInfo.InvariantCulture;

            string pattern = "yyyyMMdd HHmmss";

            string dateTime = String.Concat(parts[0].Trim(), " ", parts[1].Trim());

            //Добавлено для разбора строки финам
            if (parts.Length == 6)
                return new Tick(parts[0],
                    DateTime.ParseExact(String.Concat(parts[2].Trim(), " ", parts[3].Trim()), pattern, provider),
                    TradeAction.Buy,
                    Convert.ToDouble(parts[4], provider),
                    Convert.ToDouble(parts[5], provider));

            if (parts.Length == 5)
                return new Tick(parts[0],
                    DateTime.ParseExact(String.Concat(parts[1].Trim(), " ", parts[2].Trim()), pattern, provider),
                    TradeAction.Buy,
                    Convert.ToDouble(parts[3], provider),
                    Convert.ToDouble(parts[4], provider));
            //-----------------------------------------

            //if(parts.Length == 5)
            //    return new Tick(parts[0],
            //        DateTime.Parse(parts[1], provider),
            //        (TradeAction)Enum.Parse(typeof(TradeAction), parts[4]),
            //        Convert.ToDouble(parts[2], provider),
            //        Convert.ToDouble(parts[3], provider));

            if(parts.Length == 4)
                return new Tick(string.Empty,
                    DateTime.ParseExact(dateTime, pattern, provider),
                    TradeAction.Buy,
                    Convert.ToDouble(parts[2], provider),
                    Convert.ToDouble(parts[3], provider));

            return new Tick(parts[2].Trim(),
                DateTime.ParseExact(dateTime, pattern, provider),
                (TradeAction)Convert.ToInt32(parts[5], provider),
                Convert.ToDouble(parts[3], provider),
                Convert.ToDouble(parts[4], provider));
        }
    }
}
