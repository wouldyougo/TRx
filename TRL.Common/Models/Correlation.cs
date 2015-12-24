using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.TimeHelpers;
using System.Globalization;

namespace TRL.Common.Models
{
    public class Correlation:IDateTime
    {
        public DateTime DateTime { get; set; }
        public int StrategyId { get; set; }
        public StrategyHeader Strategy { get; set; }
        public double Value { get; set; }

        public Correlation(StrategyHeader strategyHeader, DateTime date, double value)
        {
            this.Strategy = strategyHeader;
            this.StrategyId = this.Strategy.Id;
            this.DateTime = date;
            this.Value = value;
        }

        public override string ToString()
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            return String.Format("Correlation: {0}, {1}, {2}", this.StrategyId,
                this.DateTime.ToString(ci),
                this.Value.ToString("0.0000", ci));

        }
    }
}
