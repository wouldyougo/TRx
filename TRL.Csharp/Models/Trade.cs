using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRL.Csharp.Models
{
    public class Trade:Identified
    {
        private int id;
        public int Id { 
            get {
                return this.id;
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

        private Order order;
        public Order Order
        {
            get
            {
                return this.order;
            }
            set
            {
                if (this.order != null)
                    return;

                if (value.Portfolio != this.portfolio)
                    return;

                if (value.Symbol != this.symbol)
                    return;

                this.order = value;
            }
        }

        public Trade(DateTime date, double price, double amount)
            : this(String.Empty, String.Empty, date, price, amount) { }

        public Trade(string portfolio, string symbol, DateTime date, double price, double amount)
            :this(0, portfolio, symbol, date, price, amount)
        {
        }

        public Trade(int id, string portfolio, string symbol, DateTime date, double price, double amount)
        {
            this.id = id;
            this.portfolio = portfolio;
            this.symbol = symbol;
            this.dateTime = date;
            this.price = price;
            this.amount = amount;
        }

        public double Sum
        {
            get{
                return this.Price * this.Amount;
            }
        }
    }
}
