using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public class StrategyVolumeChangeStep: IIdentified
    {
        public StrategyHeader Strategy { get; set; }

        public double Amount { get; set; }

        public int StrategyId { get; set; }

        public int Id { get; set; }

        public StrategyVolumeChangeStep(StrategyHeader strategyHeader, double amount)
        {
            this.Strategy = strategyHeader;
            this.StrategyId = strategyHeader.Id;
            this.Amount = amount;
            this.Id = strategyHeader.Id;
        }
    }
}
