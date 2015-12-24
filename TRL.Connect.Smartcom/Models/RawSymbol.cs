using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Connect.Smartcom.Models
{
    public class RawSymbol
    {
        private string symbol;
        public string Symbol
        {
            get
            {
                return this.symbol;
            }
        }

        private string shortName;
        public string ShortName
        {
            get
            {
                return this.shortName;
            }
        }

        private string longName;
        public string LongName
        {
            get
            {
                return this.longName;
            }
        }

        private string type;
        public string Type
        {
            get
            {
                return this.type;
            }
        }

        private int decimals;
        public int Decimals
        {
            get
            {
                return this.decimals;
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

        private double punkt;
        public double Punkt
        {
            get
            {
                return this.punkt;
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

        private string secExtId;
        public string SecExtId
        {
            get
            {
                return this.secExtId;
            }
        }

        private string secExchName;
        public string SecExchName
        {
            get
            {
                return this.secExchName;
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

        private double daysBeforeExpiration;
        public double DaysBeforeExpiration
        {
            get
            {
                return this.daysBeforeExpiration;
            }
        }

        private double strike;
        public double Strike
        {
            get
            {
                return this.strike;
            }
        }

        public RawSymbol(string symbol, string shortName, string longName, string type, int decimals, int lotSize, double punkt, double step, string secExtId, string secExchName, DateTime expirationDate, double daysBeforeExpiration, double strike)
        {
            this.symbol = symbol;
            this.shortName = shortName;
            this.longName = longName;
            this.type = type;
            this.decimals = decimals;
            this.lotSize = lotSize;
            this.punkt = punkt;
            this.step = step;
            this.secExtId = secExtId;
            this.secExchName = secExchName;
            this.expirationDate = expirationDate;
            this.daysBeforeExpiration = daysBeforeExpiration;
            this.strike = strike;
        }


    }
}
