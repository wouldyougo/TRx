using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public class TakeProfitOrderSettings:OrderSettings
    {
        private TakeProfitOrderSettings() { }

        public TakeProfitOrderSettings(StrategyHeader strategyHeader, int timeToLiveSeconds) :
            base(strategyHeader, timeToLiveSeconds) { }
    }
}
