using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRL.Csharp.Models
{
    public enum OrderType
    {
        Market,
        Limit,
        Stop
    }

    public class Order:Identified
    {
        private int id;
        public int Id 
        {
            get
            {
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

        private OrderType orderType;
        public OrderType OrderType
        {
            get
            {
                return this.orderType;
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
            get{
                return this.amount;
            }
        }

        public Order(int id, DateTime date, string portfolio, string symbol, OrderType type, double price, double amount)
        {
            this.id = id;
            this.dateTime = date;
            this.portfolio = portfolio;
            this.symbol = symbol;
            this.orderType = type;
            this.price = price;
            this.amount = amount;
        }
    }
}
