using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using System.Globalization;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Models
{
    public class Position:IIdentified
    {
        public int Id { get; set; }
        public string Portfolio { get; set; }
        public string Symbol { get; set; }
        public double Amount { get; set; }

        public Position(string portfolio, string symbol, double amount)
            :this(SerialIntegerFactory.Make(), portfolio, symbol, amount){}

        public Position(int id, string portfolio, string symbol, double amount)
        {
            this.Id = id;
            this.Portfolio = portfolio;
            this.Symbol = symbol;
            this.Amount = amount;
        }

        public override int GetHashCode()
        {
            return this.Portfolio.GetHashCode() ^ this.Symbol.GetHashCode();
        }

        public override string ToString()
        {
            return ToString("Position Id: {0}, Portfolio: {1}, Symbol: {2}, Amount: {3}");
        }

        public string ToImportString()
        {
            return ToString("{0},{1},{2},{3}");
        }

        private string ToString(string format)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            return String.Format(format,
                this.Id, this.Portfolio, this.Symbol, this.Amount.ToString("0.0000", ci));
        }

        public static Position Parse(string src)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;
            string[] parts = src.Split(',');

            return new Position(StructFactory.Make<int>(parts[0].Trim()),
                parts[1].Trim(),
                parts[2].Trim(),
                Convert.ToDouble(parts[3].Trim(), ci));
        }

        public bool IsLong
        {
            get
            {
                return this.Amount > 0;
            }
        }

        public bool IsShort
        {
            get
            {
                return this.Amount < 0;
            }
        }


    }
}
