using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public class PointsSettings:IIdentified
    {
        public int Id { get; set; }
        public StrategyHeader Strategy { get; set; }
        public int StrategyId { get; set; }
        public double Points { get; set; }
        public bool Trail { get; set; }

        protected PointsSettings() { }

        public PointsSettings(StrategyHeader strategyHeader, double points, bool trail)
        {
            this.Id = strategyHeader.Id;
            this.Strategy = strategyHeader;
            this.StrategyId = strategyHeader.Id;
            this.Points = points;
            this.Trail = trail;
        }
    }
}
