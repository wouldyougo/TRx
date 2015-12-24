using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRL.Common.Models;
using System.Globalization;

namespace TRL.Common.Statistics
{
    public class Deal
    {
        public string Symbol { get; private set; }
        public TradeAction TradeAction { get; private set; }
        public Trade BuyTrade { get; private set; }
        public Trade SellTrade { get; private set; }

        public Trade OpenTrade { get; private set; }
        public Trade CloseTrade { get; private set; }

        public DateTime OpenDate
        {
            get
            {
                return OpenTrade.DateTime;
            }
        }
        public DateTime CloseDate
        {
            get
            {
                return CloseTrade.DateTime;
            }
        }
        public double OpenPrice
        {
            get
            {
                return OpenTrade.Price;
            }
        }
        public double ClosePrice
        {
            get
            {
                return CloseTrade.Price;
            }
        }
        public bool IsProfit
        {
            get
            {
                return this.PnL > 0 ? true : false;
            }
        }
        public double PnL { get; private set; }       
        public double CumulativePnL { get; set; }
        public double InSeries { get; set; }
        public double InSeriesMax { get; set; }

        public Deal(TradeAction tradeAction, Trade buyTrade, Trade sellTrade)
        {
            Symbol = buyTrade.Symbol;
            if (Symbol != sellTrade.Symbol)
            {
                throw new Exception("Deal Symbol Exception");
            }

            TradeAction = tradeAction;
            BuyTrade = buyTrade;
            SellTrade = sellTrade;
            if (tradeAction == TradeAction.Buy)
            {
                OpenTrade = buyTrade;
                CloseTrade = sellTrade;
                PnL = CloseTrade.Sum - OpenTrade.Sum;
            }
            else 
            if (tradeAction == TradeAction.Sell)
            {
                OpenTrade = sellTrade;
                CloseTrade = buyTrade;
                PnL = OpenTrade.Sum - CloseTrade.Sum;
            }
            if (OpenTrade.DateTime > CloseTrade.DateTime)
                throw new Exception("Deal DateTime Exception");
        }
        public override string ToString()
        {
            return ToString("Action= {1}; OpenDateTime= {2}; OpenPrice= {3}; OpenAmount= {4}; CloseDateTime= {5}; ClosePrice= {6}; CloseAmount= {7}; PnL= {8}; cumPnL= {9}; inSeries= {10}; inSeriesMax= {11}");
        }
        private string ToString(string format)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;
            return String.Format(format,
                                this.Symbol,
                                this.TradeAction,
                                this.OpenTrade.DateTime.ToString(),
                                this.OpenTrade.Price.ToString("0.0000", ci),
                                this.OpenTrade.Amount.ToString("0.0000", ci),
                                this.CloseTrade.DateTime.ToString(),
                                this.CloseTrade.Price.ToString("0.0000", ci),
                                this.CloseTrade.Amount.ToString("0.0000", ci),
                                this.PnL,
                                this.CumulativePnL,
                                this.InSeries,
                                this.InSeriesMax);
        }
    }
}
