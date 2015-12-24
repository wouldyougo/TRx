using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public class SymbolSummary:INamed, IMutable<SymbolSummary>
    {
        public string Name { get; set; }
        public DateTime LastDealDate { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double LastDealPrice { get; set; }
        public double Volume { get; set; }
        public double LastDealVolume { get; set; }
        public double Offer { get; set; }
        public double Bid { get; set; }
        public double OfferSize { get; set; }
        public double BidSize { get; set; }
        public double OpenVolume { get; set; }
        public double BuyWarrantySum { get; set; }
        public double SellWarrantySum { get; set; }
        public double HighLimitPrice { get; set; }
        public double LowLimitPrice { get; set; }
        public bool IsTraded { get; set; }

        public SymbolSummary() { }

        public SymbolSummary(string name,
            DateTime lastDeal,
            double open,
            double high,
            double low,
            double close,
            double lastDealPrice,
            double volume,
            double lastDealVolume,
            double offer,
            double bid,
            double offerSize,
            double bidSize,
            double openVolume,
            double buyWarranty,
            double sellWarranty,
            double highLimit,
            double lowLimit,
            bool isTraded)
        {
            this.Name = name;
            this.LastDealDate = lastDeal;
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;
            this.LastDealPrice = lastDealPrice;
            this.LastDealVolume = lastDealVolume;
            this.Volume = volume;
            this.Offer = offer;
            this.Bid = bid;
            this.OfferSize = offerSize;
            this.BidSize = bidSize;
            this.OpenVolume = openVolume;
            this.BuyWarrantySum = buyWarranty;
            this.SellWarrantySum = sellWarranty;
            this.HighLimitPrice = highLimit;
            this.LowLimitPrice = lowLimit;
            this.IsTraded = isTraded;
        }
        public void Update(SymbolSummary item)
        {
            if (this.Name != item.Name)
                return;

            this.Name = item.Name;
            this.LastDealDate = item.LastDealDate;
            this.Open = item.Open;
            this.High = item.High;
            this.Low = item.Low;
            this.Close = item.Close;
            this.LastDealPrice = item.LastDealPrice;
            this.LastDealVolume = item.LastDealVolume;
            this.Volume = item.Volume;
            this.Offer = item.Offer;
            this.Bid = item.Bid;
            this.OfferSize = item.OfferSize;
            this.BidSize = item.BidSize;
            this.OpenVolume = item.OpenVolume;
            this.BuyWarrantySum = item.BuyWarrantySum;
            this.SellWarrantySum = item.SellWarrantySum;
            this.HighLimitPrice = item.HighLimitPrice;
            this.LowLimitPrice = item.LowLimitPrice;
            this.IsTraded = item.IsTraded;
        }

        public override string ToString()
        {
            return String.Format("SymbolSummary: {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}",
                this.Name,
                this.LastDealDate,
                this.Open,
                this.High,
                this.Low,
                this.Close,
                this.LastDealPrice,
                this.Volume,
                this.Offer,
                this.Bid,
                this.OfferSize,
                this.BidSize,
                this.OpenVolume,
                this.BuyWarrantySum,
                this.SellWarrantySum,
                this.HighLimitPrice,
                this.LowLimitPrice,
                this.IsTraded);
        }
    }
}
