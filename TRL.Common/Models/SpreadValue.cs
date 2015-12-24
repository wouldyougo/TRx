using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace TRL.Common.Models
{
    public class SpreadValue
    {
        public int Id { get; set; }

        public double SellAfterPrice { get; set; }
        public double BuyBeforePrice { get; set; }

        public DateTime DateTime { get; set; }

        public SpreadValue(int id, DateTime dateTime, double sellAfterPrice, double buyBeforePrice)
        {
            Id = id;
            DateTime = dateTime;
            SellAfterPrice = sellAfterPrice;
            BuyBeforePrice = buyBeforePrice;
        }

        public override string ToString()
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            return String.Format("SpreadValue: {0},{1:dd/MM/yyyy H:mm:ss.fff},{2},{3}",
                this.Id,
                this.DateTime,
                this.SellAfterPrice.ToString("0.0000", ci),
                this.BuyBeforePrice.ToString("0.0000", ci));
        }
    }
}
