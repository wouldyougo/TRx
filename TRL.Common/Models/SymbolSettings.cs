using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public class SymbolSettings:INamed,IMutable<SymbolSettings>
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int PricePrecision { get; set; }
        public int LotSize { get; set; }
        public double MinStepPrice { get; set; }
        public double MinPriceStep { get; set; }
        public DateTime ExpirationDate { get; set; }
        public double DaysBeforeExpiration { get; set; }
        public double LastPrice { get; set; }

        public SymbolSettings() { }

        public SymbolSettings(string symbol,
            string shortName,
            string description,
            string type,
            int precision,
            int size,
            double stepPrice,
            double priceStep,
            DateTime expiration,
            double days,
            double lastPrice)
        {
            this.Name = symbol;
            this.ShortName = shortName;
            this.Description = description;
            this.Type = type;
            this.PricePrecision = precision;
            this.LotSize = size;
            this.MinStepPrice = stepPrice;
            this.MinPriceStep = priceStep;
            this.ExpirationDate = expiration;
            this.DaysBeforeExpiration = days;
            this.LastPrice = lastPrice;
        }

        public void Update(SymbolSettings item)
        {
            if (item.Name != this.Name)
                return;

            this.Name = item.Name;
            this.ShortName = item.ShortName;
            this.Description = item.Description;
            this.Type = item.Type;
            this.PricePrecision = item.PricePrecision;
            this.LotSize = item.LotSize;
            this.MinStepPrice = item.MinStepPrice;
            this.MinPriceStep = item.MinPriceStep;
            this.ExpirationDate = item.ExpirationDate;
            this.DaysBeforeExpiration = item.DaysBeforeExpiration;
            this.LastPrice = item.LastPrice;
        }

        public override string ToString()
        {
            return String.Format("SymbolSettings: {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
                this.Name,
                this.ShortName,
                this.Description,
                this.Type,
                this.PricePrecision,
                this.LotSize,
                this.MinStepPrice,
                this.MinPriceStep,
                this.ExpirationDate,
                this.DaysBeforeExpiration,
                this.LastPrice);
        }
    }
}
