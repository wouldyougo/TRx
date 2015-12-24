using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;

namespace TRL.Common.Models
{
    public class ArbitrageSettings:IIdentified
    {
        public ICollection<StrategyHeader> LeftLeg { get; set; }
        public ICollection<StrategyHeader> RightLeg { get; set; }

        public SpreadSettings SpreadSettings { get; set; }

        public int Id { get; set; }

        public ArbitrageSettings()
            :this(SerialIntegerFactory.Make(),
            new List<StrategyHeader>(),
            new List<StrategyHeader>(),
            new SpreadSettings())
        {
        }

        public ArbitrageSettings(int id, ICollection<StrategyHeader> leftLeg, ICollection<StrategyHeader> rightLeg, SpreadSettings spreadSettings)
        {
            Id = id;
            LeftLeg = leftLeg;
            RightLeg = rightLeg;
            SpreadSettings = spreadSettings;
        }

        public bool HasSymbol(string symbol)
        {
            return LeftLeg.Any(s => s.Symbol.Equals(symbol)) || RightLeg.Any(s => s.Symbol.Equals(symbol));
        }
    }
}
