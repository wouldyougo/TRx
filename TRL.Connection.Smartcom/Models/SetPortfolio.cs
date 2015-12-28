using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Connect.Smartcom.Models
{
    public class SetPortfolio
    {
        private string portfolio;
        private DateTime dateTime;
        private double cash;
        private double leverage;
        private double commision;
        private double saldo;

        public SetPortfolio(string portfolio, double cash, double leverage, double commision, double saldo)
        {
            this.dateTime = DateTime.Now;
            this.portfolio = portfolio;
            this.cash = cash;
            this.leverage = leverage;
            this.commision = commision;
            this.saldo = saldo;
        }

        public string Portfolio
        {
            get
            {
                return this.portfolio;
            }
        }

        public DateTime DateTime
        {
            get
            {
                return this.dateTime;
            }
        }

        public double Cash
        {
            get
            {
                return this.cash;
            }
        }

        public double Leverage
        {
            get
            {
                return this.leverage;
            }
        }

        public double Commision
        {
            get
            {
                return this.commision;
            }
        }

        public double Saldo
        {
            get
            {
                return this.saldo;
            }
        }
    }
}
