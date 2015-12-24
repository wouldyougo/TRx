using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;

namespace TRL.Common.Models
{
    public class SMASettings:IIdentified
    {
        public int Id { get; set; }
        public StrategyHeader Strategy { get; set; }
        public int StrategyId { get; set; }
        public int PeriodFast { get; set; }
        public int PeriodSlow { get; set; }

        public SMASettings(StrategyHeader strategyHeader, int periodFast, int periodSlow)
        {
            this.Id = SerialIntegerFactory.Make();
            this.Strategy = strategyHeader;
            this.StrategyId = strategyHeader.Id;
            this.PeriodFast = periodFast;
            this.PeriodSlow = periodSlow;
        }
    }
}
