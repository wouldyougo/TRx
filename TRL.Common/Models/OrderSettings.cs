using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;

namespace TRL.Common.Models
{
    public class OrderSettings:IIdentified
    {
        public int Id { get; set; }
        public int TimeToLive { get; set; }
        public StrategyHeader Strategy { get; set; }
        public int StrategyId { get; set; }

        protected OrderSettings() { }

        public OrderSettings(StrategyHeader strategyHeader, int timeToLiveSeconds)
        {
            this.Id = strategyHeader.Id;
            this.TimeToLive = timeToLiveSeconds;
            this.Strategy = strategyHeader;
            this.StrategyId = strategyHeader.Id;
        }
    }
}
