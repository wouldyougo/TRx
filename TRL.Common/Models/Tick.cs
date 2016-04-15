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
        public long Id { get; set; }
        //public string StringId { get; set; }
        public string Symbol { get; set; }
        public DateTime DateTime { get; set; }
        public double Price { get; set; }
        public double Volume { get; set; }
        public TradeAction TradeAction { get; set; }

        public Tick() { }
        //void AddTick(string symbol, 
        //    System.DateTime datetime, 
        //    double price, 
        //    double volume, 
        //    string tradeno, 
        //    StClientLib.StOrder_Action action)
        public Tick(string symbol, DateTime date, double price, double volume)
            : this(symbol, date, price, volume, TradeAction.Buy){}
        public Tick(string symbol, DateTime date, double price, double volume, TradeAction action)
        {
            this.Symbol = symbol;
            this.DateTime = date;
            this.Price = price;
            this.Volume = volume;
            this.TradeAction = action;
        }
        public Tick(string symbol, DateTime date, double price, double volume, TradeAction action, long id)
            :this(symbol, date, price, volume, action)
        {
            this.Id = id;
        }

        public override string ToString()
        {
            return ToString("Symbol: {0}, DateTime: {1}, Price: {2}, Volume: {3}, TradeAction: {4}, Id: {5}");
        }

        public string ToImportString()
        {
            return ToString("{0},{1},{2},{3},{4},{5}");
        }

        private string ToString(string format)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            return String.Format(format,
                //this.Symbol, this.DateTime.ToString(ci), this.Price.ToString("0.0000", ci), this.Volume.ToString("0.0000", ci), this.TradeAction);
                this.Symbol, this.DateTime, this.Price.ToString("0.0000", ci), this.Volume.ToString("0.0000", ci), this.TradeAction, this.Id);
        }


        public static Tick Parse(string tickString)
        {
            string[] parts = tickString.Split(',');

            CultureInfo provider = CultureInfo.InvariantCulture;

            string pattern = "yyyyMMdd HHmmss";

            string dateTime = String.Concat(parts[0].Trim(), " ", parts[1].Trim());

            // Собственный формат
            //  < TICKER >,< DATETIME >,< LAST >,< VOL >,< ACTION >,< ID > 
            //  "RTS-6.14,20160405 00:00:00,10.0000,1.0000,Sell,101112";
            if (parts.Length == 6)
                return new Tick(parts[0],
                    //DateTime.Parse(parts[1], provider),
                    DateTime.ParseExact(parts[1], "yyyyMMdd HH:mm:ss", provider),
                    Convert.ToDouble(parts[2], provider),
                    Convert.ToDouble(parts[3], provider),
                    (TradeAction)Enum.Parse(typeof(TradeAction), parts[4]),
                    Convert.ToInt64(parts[5], provider));

            //-----------------------------------------
            //Добавлено для разбора строки финам
            //-----------------------------------------
            //  < TICKER >,< PER >,< DATE >,< TIME >,< LAST >,< VOL >
            //  SPFB.Si - 9.16,0,20160405,100000,71707.000000000,1
            //if (parts.Length == 6)
            //    return new Tick(parts[0],
            //        DateTime.ParseExact(String.Concat(parts[2].Trim(), " ", parts[3].Trim()), pattern, provider),
            //        //TradeAction.Buy,
            //        Convert.ToDouble(parts[4], provider),
            //        Convert.ToDouble(parts[5], provider));

            // < TICKER >,< DATE >,< TIME >,< LAST >,< VOL >
            // SPFB.Si - 9.16,20160405,100000,71707.000000000,1
            if (parts.Length == 5)
                return new Tick(parts[0],
                    DateTime.ParseExact(String.Concat(parts[1].Trim(), " ", parts[2].Trim()), pattern, provider),
                    //TradeAction.Buy,
                    Convert.ToDouble(parts[3], provider),
                    Convert.ToDouble(parts[4], provider));

            //< DATE >,< TIME >,< LAST >,< VOL >,< ID >
            //20160405,100000,71707.000000000,1,1445235052
            //-----------------------------------------

            if (parts.Length == 4)
                return new Tick(string.Empty,
                    DateTime.ParseExact(dateTime, pattern, provider),
                    Convert.ToDouble(parts[2], provider),
                    Convert.ToDouble(parts[3], provider));

            return new Tick(parts[2].Trim(),
                DateTime.ParseExact(dateTime, pattern, provider),
                Convert.ToDouble(parts[3], provider),
                Convert.ToDouble(parts[4], provider),
                (TradeAction)Convert.ToInt32(parts[5], provider));
        }
    }
}
