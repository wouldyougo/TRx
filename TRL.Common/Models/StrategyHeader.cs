using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace TRL.Common.Models
{
    /// <summary>
    /// Стратегия
    /// </summary>
    public class StrategyHeader:IIdentified
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Portfolio { get; set; }
        public string Symbol { get; set; }
        public double Amount { get; set; }

        public StrategyHeader(int id, string description, string portfolio, string symbol, double amount)
        {
            this.Id = id;
            this.Description = description;
            this.Portfolio = portfolio;
            this.Symbol = symbol;
            this.Amount = amount;
        }

        public override string ToString()
        {
            return ToString("Strategy Id: {0}, Description: {1}, Portfolio: {2}, Symbol: {3}, Amount: {4}");
        }

        public string ToImportString()
        {
            return ToString("{0},{1},{2},{3},{4}");
        }

        private string ToString(string format)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            return String.Format(format, this.Id, this.Description, this.Portfolio, this.Symbol, this.Amount.ToString("0.0000", ci));
        }
    }
}
