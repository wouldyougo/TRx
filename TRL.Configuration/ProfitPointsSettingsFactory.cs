using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Common;

namespace TRL.Configuration
{
    public class ProfitPointsSettingsFactory:IGenericFactory<ProfitPointsSettings>
    {
        private StrategyHeader strategyHeader;
        private string prefix;

        private ProfitPointsSettingsFactory() { }

        public ProfitPointsSettingsFactory(StrategyHeader strategyHeader, string prefix)
        {
            this.strategyHeader = strategyHeader;
            this.prefix = prefix;
        }

        public ProfitPointsSettings Make()
        {
            try
            {
                return new ProfitPointsSettings(this.strategyHeader,
                    AppSettings.GetValue<double>(String.Concat(this.prefix, "_ProfitPointsSettings_Points")),
                    AppSettings.GetValue<bool>(String.Concat(this.prefix, "_ProfitPointsSettings_Trail")));
            }
            catch
            {
                return null;
            }
        }
    }
}
