using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using TRL.Common.TimeHelpers;
using System.Globalization;

namespace TRL.Common.Models
{
    public enum OrderType
    {
        Stop,
        Limit,
        Market
    }

    public class Order:IIdentified, IDateTime
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Portfolio { get; set; }
        public string Symbol { get; set; }
        public TradeAction TradeAction { get; set; }
        public OrderType OrderType { get; set; }
        public double Amount { get; set; }
        public double FilledAmount { get; set; }
        public double Price { get; set; }
        public double Stop { get; set; }
        
        public DateTime CancellationDate { get; set; }
        public string CancellationReason { get; set; }

        public DateTime RejectedDate { get; set; }
        public string RejectReason { get; set; }
        
        public DateTime ExpirationDate { get; set; }

        public int SignalId { get; set; }
        public virtual Signal Signal { get; set; }

        /// <summary>
        /// BarDateId на котором был создан ордер
        /// </summary>
        public long BarDateId { get; set; }
        /// <summary>
        /// Bar на котором был создан ордер
        /// </summary>
        public virtual Bar Bar { get; set; }

        public Order()
        {
            this.Id = SerialIntegerFactory.Make();
            this.ExpirationDate = new FortsTradingSchedule().SessionEnd;
        }

        public Order(Signal signal)
            :this(SerialIntegerFactory.Make(), BrokerDateTime.Make(DateTime.Now), signal.Strategy.Portfolio, signal.Strategy.Symbol, signal.TradeAction, signal.OrderType, signal.Amount, signal.Limit, signal.Stop)
        {
            this.Signal = signal;
            this.SignalId = signal.Id;
        }

        public Order(Signal signal, int timeToLiveSeconds)
            : this(SerialIntegerFactory.Make(), BrokerDateTime.Make(DateTime.Now), signal.Strategy.Portfolio, signal.Strategy.Symbol, signal.TradeAction, signal.OrderType, signal.Amount, signal.Limit, signal.Stop)
        {
            this.Signal = signal;
            this.SignalId = signal.Id;
            this.ExpirationDate = BrokerDateTime.Make(DateTime.Now).AddSeconds(timeToLiveSeconds);
        }

        public Order(int id, DateTime date, string portfolio, string symbol, TradeAction action, OrderType type, double amount, double price, double stop)
        {
            this.Id = id;
            this.DateTime = date;
            this.Portfolio = portfolio;
            this.Symbol = symbol;
            this.TradeAction = action;
            this.OrderType = type;
            this.Amount = amount;
            this.Price = price;
            this.Stop = stop;
            this.ExpirationDate = new FortsTradingSchedule().SessionEnd;
            this.SignalId = 0;
            this.Signal = null;
            this.BarDateId = 0;
            this.Bar = null;
        }
        public Order(int id, DateTime date, string portfolio, string symbol, TradeAction action, OrderType type, double amount, double price, double stop, long barDateId, Bar bar)
        {
            this.Id = id;
            this.DateTime = date;
            this.Portfolio = portfolio;
            this.Symbol = symbol;
            this.TradeAction = action;
            this.OrderType = type;
            this.Amount = amount;
            this.Price = price;
            this.Stop = stop;
            this.ExpirationDate = new FortsTradingSchedule().SessionEnd;
            this.SignalId = 0;
            this.Signal = null;
            this.BarDateId = barDateId;
            this.Bar = bar;
        }
        /// <summary>
        ///
        /// </summary>
        public bool IsExpired
        {
            //TODO переделать BrokerDateTime
            //get { return BrokerDateTime.Make(DateTime.Now) > ExpirationDate; }
            get { return DateTime.Now > ExpirationDate; }
        }


        public bool IsFilled
        {
            get
            {
                return this.Amount == this.FilledAmount;
            }
        }

        public bool IsFilledPartially
        {
            get
            {
                if (this.FilledAmount == 0)
                    return false;

                return this.Amount > this.FilledAmount;
            }
        }


        public bool IsRejected
        {
            get
            {
                return this.RejectedDate != DateTime.MinValue;
            }
        }

        public void Reject(DateTime dateTime, string reason)
        {
            this.RejectedDate = dateTime;
            this.RejectReason = reason;
        }

        public bool IsLike(Order item)
        {
            return this.Portfolio == item.Portfolio &&
                this.Symbol == item.Symbol &&
                this.TradeAction == item.TradeAction &&
                !this.IsExpired;
        }

        public override string ToString()
        {
            return ToString("Order Id: {0}, DateTime: {1}, Portfolio: {2}, Symbol: {3}, Action: {4}, Type: {5}, Price: {6}, Amount: {7}, Stop: {8}, FilledAmount: {9}, DeliveryDate: {10}, RejectDate: {11}, RejectReason: {12}, ExpirationDate: {13}, CancellationDate: {14}, CancellationReason: {15}, Signal: {16}");
        }

        public string ToImportString()
        {
            return ToString("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}");
        }

        private string ToString(string format)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            return String.Format(format, this.Id, this.DateTime.ToString(ci), this.Portfolio, 
                this.Symbol, this.TradeAction, this.OrderType,
                this.Price.ToString("0.0000", ci), this.Amount.ToString("0.0000", ci), this.Stop.ToString("0.0000", ci), 
                this.FilledAmount.ToString("0.0000", ci), this.DeliveryDate.ToString(ci),
                this.RejectedDate.ToString(ci), this.RejectReason, 
                this.ExpirationDate.ToString(ci), 
                this.CancellationDate.ToString(ci), this.CancellationReason, this.Signal.Id);
        }


        public void Cancel(DateTime cancellationDate, string cancellationReason)
        {
            this.CancellationDate = cancellationDate;
            this.CancellationReason = cancellationReason;
        }

        public bool IsCanceled
        {
            get
            {
                return this.CancellationDate > DateTime.MinValue;
            }
        }

        public double UnfilledSignedAmount
        {
            get
            {
                double m = this.TradeAction == TradeAction.Buy ? 1 : -1;

                return (this.Amount - this.FilledAmount) * m;
            }
        }

        public DateTime DeliveryDate { get; set; }
        public bool IsDelivered
        {
            get
            {
                return this.DeliveryDate != DateTime.MinValue;
            }
        }

        public static Order Parse(string src)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;
            string[] parts = src.Split(',');

            return new Order
            {
                Id = StructFactory.Make<int>(parts[0].Trim()),
                DateTime = ParseDateTime(parts[1]),
                Portfolio = parts[2].Trim(),
                Symbol = parts[3].Trim(),
                TradeAction = StructFactory.Make<TradeAction>(parts[4].Trim()),
                OrderType = StructFactory.Make<OrderType>(parts[5].Trim()),
                Price = Convert.ToDouble(parts[6].Trim(), ci),
                Amount = Convert.ToDouble(parts[7].Trim(), ci),
                Stop = Convert.ToDouble(parts[8].Trim(), ci),
                FilledAmount = Convert.ToDouble(parts[9].Trim(), ci),
                DeliveryDate = ParseDateTime(parts[10]),
                RejectedDate = ParseDateTime(parts[11]),
                RejectReason = parts[12].Trim(),
                ExpirationDate = ParseDateTime(parts[13]),
                CancellationDate = ParseDateTime(parts[14]),
                CancellationReason = parts[15].Trim(),
                SignalId = Convert.ToInt32(parts[16].Trim(), ci)
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
