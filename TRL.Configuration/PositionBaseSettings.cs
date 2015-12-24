using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Configuration
{
    public class PositionBaseSettings
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

        protected PositionBaseSettings() { }

        public PositionBaseSettings(string portfolio, string symbol)
        {
            this.portfolio = portfolio;
            this.symbol = symbol;
        }
    }
}
