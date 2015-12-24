using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;

namespace TRL.Common.Models
{
    public class StopPointsSettings:PointsSettings
    {
        private StopPointsSettings() { }

        public StopPointsSettings(StrategyHeader strategyHeader, double points, bool trail)
            : base(strategyHeader, points, trail) { }
    }
}
