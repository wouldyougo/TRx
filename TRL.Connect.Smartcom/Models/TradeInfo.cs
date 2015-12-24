using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Connect.Smartcom.Models
{
    public class TradeInfo
    {
        private string portfolio;
        public string Portfolio
        {
            get
            {
                return this.portfolio;
            }
        }

        private string symbol;
        public string Symbol
        {
            get
            {
                return this.symbol;
            }
        }

        private string orderNo;
        public string OrderNo
        {
            get
            {
                return this.orderNo;
            }
        }

        private double price;
        public double Price
        {
            get
            {
                return this.price;
            }
        }

        private double amount;
        public double Amount
        {
            get
            {
                return this.amount;
            }
        }

        private DateTime dateTime;
        public DateTime DateTime
        {
            get
            {
                return this.dateTime;
            }
        }

        private string tradeNo;
        public string TradeNo
        {
            get
            {
                return this.tradeNo;
            }
        }

        protected TradeInfo() { }

        public TradeInfo(string portfolio, string symbol, string orderNo, double price, double amount, DateTime date, string tradeNo)
        {
            this.portfolio = portfolio;
            this.symbol = symbol;
            this.orderNo = orderNo;
            this.price = price;
            this.amount = amount;
            this.dateTime = date;
            this.tradeNo = tradeNo;
        }







    }
}
