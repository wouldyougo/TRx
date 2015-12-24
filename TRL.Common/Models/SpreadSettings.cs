using TRL.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public class SpreadSettings
    {
        public double FairPrice { get; set; }
        public double SellAfterPrice { get; set; }
        public double BuyBeforePrice { get; set; }

        public SpreadSettings() { }

        public SpreadSettings(double fairPrice, double sellAfterPrice, double buyBeforePrice)
        {
            this.FairPrice = fairPrice;
            this.SellAfterPrice = sellAfterPrice;
            this.BuyBeforePrice = buyBeforePrice;
        }
    }
}
