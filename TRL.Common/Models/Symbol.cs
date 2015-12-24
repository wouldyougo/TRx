using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace TRL.Common.Models
{
    public class Symbol:INamed, IMutable<Symbol>
    {
        private string name;
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        private int lotSize;
        public int LotSize
        {
            get
            {
                return this.lotSize;
            }
        }

        private double stepPrice;
        public double StepPrice
        {
            get 
            {
                return this.stepPrice;
            }
        }

        private double step;
        public double Step
        {
            get
            {
                return this.step;
            }
        }

        private double margin;
        public double Margin
        {
            get
            {
                return this.margin;
            }
        }

        private double transactionFee;
        public double TransactionFee
        {
            get
            {
                return this.transactionFee;
            }
        }

        private double lowLimit;
        public double LowLimit
        {
            get
            {
                return this.lowLimit;
            }
        }

        private double highLimit;
        public double HighLimit
        {
            get
            {
                return this.highLimit;
            }
        }

        private DateTime expirationDate;
        public DateTime ExpirationDate
        {
            get
            {
                return this.expirationDate;
            }
        }

        public Symbol(string symbol, int lotSize, double stepPrice, double step, DateTime expirationDate)
            : this(symbol, lotSize, stepPrice, step, 0, 0, 0, 0, expirationDate) { }

        public Symbol(string symbol, int lotSize, double stepPrice, double step, double margin, double fee, double low, double high, DateTime expirationDate)
        {
            this.name = symbol;
            this.lotSize = lotSize;
            this.stepPrice = stepPrice;
            this.step = step;
            this.margin = margin;
            this.transactionFee = fee;
            this.lowLimit = low;
            this.highLimit = high;
            this.expirationDate = expirationDate;
        }

        public override string ToString()
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            return String.Format("Symbol: {0}, LotSize: {1}, StepPrice: {2}, PriceStep: {3}, Margin: {4}, TransactionFee: {5}, LowLimit: {6}, HighLimit: {7}, ExpirationDate: {8}",
                this.Name, 
                this.LotSize, 
                this.StepPrice.ToString("0.0000", ci), 
                this.Step.ToString("0.0000", ci),                
                this.Margin.ToString("0.0000", ci),
                this.TransactionFee.ToString("0.0000", ci),
                this.LowLimit.ToString("0.0000", ci),
                this.HighLimit.ToString("0.0000", ci),
                this.ExpirationDate.ToString(ci));
        }

        public void Update(Symbol item)
        {
            if (!this.name.Equals(item.Name))
                return;

            this.lotSize = item.LotSize;
            this.stepPrice = item.stepPrice;
            this.step = item.Step;
            this.margin = item.Margin;
            this.transactionFee = item.TransactionFee;
            this.lowLimit = item.LowLimit;
            this.highLimit = item.HighLimit;
            this.expirationDate = item.ExpirationDate;
        }
    }
}
