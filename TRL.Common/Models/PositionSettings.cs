using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public enum PositionType
    {
        Any = 0,
        Long = 1,
        Short = 2
    }

    public class PositionSettings:IIdentified
    {
        public int Id { get; set; }
        public StrategyHeader Strategy { get; set; }
        public int StrategyId { get; set; }
        public PositionType PositionType { get; set; }

        public PositionSettings() { }

        public PositionSettings(StrategyHeader strategyHeader)
            :this(strategyHeader, PositionType.Any)
        {
        }

        public PositionSettings(StrategyHeader strategyHeader, PositionType type)
        {
            this.Id = strategyHeader.Id;
            this.Strategy = strategyHeader;
            this.StrategyId = strategyHeader.Id;
            this.PositionType = type;
        }
    }
}
