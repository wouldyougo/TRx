using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public class StopLossOrderSettings:OrderSettings
    {
        private StopLossOrderSettings() { }

        public StopLossOrderSettings(StrategyHeader strategyHeader, int timeToLiveSeconds) :
            base(strategyHeader, timeToLiveSeconds) { }
    }
}
