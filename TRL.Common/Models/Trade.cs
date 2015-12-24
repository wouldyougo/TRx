using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using System.Globalization;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Models
{
    public class Trade:IIdentified, IDateTime
    {
        public int Id { get; set; }
        public string Portfolio { get; set; }
        public string Symbol { get; set; }
        public DateTime DateTime { get; set; }
        public double Price { get; set; }
        public double Amount { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public Trade() { }

        public Trade(Order order, string portfolio, string symbol, double price, double amount, DateTime date)
        {
            this.Id = SerialIntegerFactory.Make();
            this.Portfolio = portfolio;
            this.Symbol = symbol;
            this.Price = price;
            this.Amount = amount;
            this.DateTime = date;
            this.Order = order;
            
            if(order != null)
                this.OrderId = order.Id;
        }

        public Trade(Trade trade)
            :this(trade.Order, trade.Portfolio, trade.Symbol, trade.Price, trade.Amount, trade.DateTime)
        {
            this.Id = trade.Id;
        }

        public override string ToString()
        {
            return ToString("Trade Id: {0}, DateTime: {1}, Portfolio: {2}, Symbol: {3}, Price: {4}, Amount: {5}, Order.Id: {6}");
        }

        public string ToImportString()
        {
            return ToString("{0},{1},{2},{3},{4},{5},{6}");
        }

        private string ToString(string format)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            int orderId = 0;

            if (this.Order != null)
                orderId = this.Order.Id;

            return String.Format(format, this.Id, this.DateTime.ToString(ci), this.Portfolio, this.Symbol, this.Price.ToString("0.0000", ci), this.Amount.ToString("0.0000", ci), orderId);
        }

        public static Trade Parse(string src)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;
            string[] parts = src.Split(',');

            return new Trade
            {
                Id = StructFactory.Make<int>(parts[0].Trim()),
                DateTime = ParseDateTime(parts[1]),
                Portfolio = parts[2].Trim(),
                Symbol = parts[3].Trim(),
                Price = Convert.ToDouble(parts[4].Trim(), ci),
                Amount = Convert.ToDouble(parts[5].Trim(), ci),
                OrderId = Convert.ToInt32(parts[6].Trim(), ci),
            };
        }

        private static DateTime ParseDateTime(string src)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;

            string pattern = "MM/dd/yyyy HH:mm:ss";

            return DateTime.ParseExact(src.Trim(), pattern, provider);
        }

        public bool Buy
        {
            get
            {
                return this.Amount > 0;
            }
        }

        public bool Sell
        {
            get
            {
                return this.Amount < 0;
            }
        }

        public double Sum
        {
            get
            {
                return Math.Abs(this.Amount) * this.Price;
            }
        }

        public double Sign
        {
            get
            {
                return Math.Abs(this.Amount) / this.Amount;
            }
        }

        public TradeAction Action
        {
            get
            {
                return (this.Amount > 0) ? TradeAction.Buy : TradeAction.Sell;
            }
        }

        public double AbsoluteAmount
        {
            get
            {
                return Math.Abs(this.Amount);
            }
        }
    }
}
