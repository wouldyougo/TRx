using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using TRL.Common.Collections;
using TRL.Common.Models;

namespace TRL.Emulation
{
    public static class SampleTradingDataContextFactory
    {
        public static TradingDataContext Make()
        {
            TradingDataContext tdContext = new TradingDataContext();

            AddStrategies(tdContext);
            AddSymbolSettings(tdContext);

            return tdContext;
        }

        private static void AddStrategies(IDataContext context)
        {
            context.Get<ICollection<StrategyHeader>>().Add(new StrategyHeader(1, "Arbitrage left leg", "BP12345-RF-01", "RTS-12.13_FT", 1));
            context.Get<ICollection<StrategyHeader>>().Add(new StrategyHeader(2, "Arbitrage right leg", "BP12345-RF-01", "Si-12.13_FT", 2));
            context.Get<ICollection<StrategyHeader>>().Add(new StrategyHeader(3, "Arbitrage right leg", "BP12345-RF-01", "Eu-12.13_FT", 1));
            context.Get<ICollection<StrategyHeader>>().Add(new StrategyHeader(4, "Sample strategyHeader", "BP12345-RF-01", "SBRF-12.13_FT", 20));
            context.Get<ICollection<StrategyHeader>>().Add(new StrategyHeader(5, "Sample strategyHeader", "BP12345-RF-01", "SBPR-12.13_FT", 21));
        }

        private static void AddSymbolSettings(IDataContext context)
        {
            context.Get<HashSetOfNamedMutable<Symbol>>().Add(new Symbol("RTS-12.13_FT", 1, 6.5692, 10,  new DateTime(2013, 12, 16)));
            context.Get<HashSetOfNamedMutable<Symbol>>().Add(new Symbol("Si-12.13_FT", 1000, 1, 1, new DateTime(2013, 12, 16)));
            context.Get<HashSetOfNamedMutable<Symbol>>().Add(new Symbol("Eu-12.13_FT", 1000, 1, 1, new DateTime(2013, 12, 16)));
            context.Get<HashSetOfNamedMutable<Symbol>>().Add(new Symbol("SBRF-12.13_FT", 100, 1, 1, new DateTime(2013, 12, 16)));
            context.Get<HashSetOfNamedMutable<Symbol>>().Add(new Symbol("SBPR-12.13_FT", 100, 1, 1, new DateTime(2013, 12, 16)));
        }
    }
}
