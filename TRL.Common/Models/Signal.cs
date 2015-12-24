using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using System.Globalization;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Models
{
    public class Signal:IIdentified, IDateTime
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public TradeAction TradeAction { get; set; }

        public OrderType OrderType { get; set; }

        public double Price { get; set; }

        public double Stop { get; set; }

        public double Limit { get; set; }

        public int StrategyId { get; set; }
        public virtual StrategyHeader Strategy { get; set; }

        public double Amount { get; set; }

        public Signal() { }

        public Signal(StrategyHeader strategyHeader, DateTime date, TradeAction action, OrderType type, double price, double stop, double limit)
        {
            this.Id = SerialIntegerFactory.Make();
            this.StrategyId = strategyHeader.Id;
            this.Strategy = strategyHeader;
            this.DateTime = date;
            this.TradeAction = action;
            this.OrderType = type;
            this.Price = price;
            this.Stop = stop;
            this.Limit = limit;
            this.Amount = strategyHeader.Amount;
        }

        public override string ToString()
        {
            return ToString("Signal Id: {0}, DateTime: {1}, TradeAction: {2}, OrderType: {3}, Price: {4}, Stop: {5}, Limit: {6}, Amount: {7}, StrategyId: {8}");
        }

        public string ToImportString()
        {
            return ToString("{0},{1},{2},{3},{4},{5},{6},{7},{8}");
        }

        private string ToString(string format)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            return String.Format(format,
                this.Id, this.DateTime.ToString(ci), this.TradeAction, this.OrderType, this.Price.ToString("0.0000", ci), this.Stop.ToString("0.0000", ci), this.Limit.ToString("0.0000", ci), this.Amount.ToString("0.0000", ci), this.Strategy.Id);
        }

        public static Signal Parse(string src)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;
            string[] parts = src.Split(',');

            return new Signal
            {
                Id = StructFactory.Make<int>(parts[0].Trim()),
                DateTime = ParseDateTime(parts[1]),
                TradeAction = StructFactory.Make<TradeAction>(parts[2].Trim()),
                OrderType = StructFactory.Make<OrderType>(parts[3].Trim()),
                Price = Convert.ToDouble(parts[4].Trim(), ci),
                Stop = Convert.ToDouble(parts[5].Trim(), ci),
                Limit = Convert.ToDouble(parts[6].Trim(), ci),
                Amount = Convert.ToDouble(parts[7].Trim(), ci),
                StrategyId = Convert.ToInt32(parts[8].Trim(), ci),
            };
        }

        private static DateTime ParseDateTime(string src)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;

            string pattern = "MM/dd/yyyy HH:mm:ss";

            return DateTime.ParseExact(src.Trim(), pattern, provider);
        }

    }
}
