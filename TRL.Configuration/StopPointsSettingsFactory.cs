using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Common;

namespace TRL.Configuration
{
    public class StopPointsSettingsFactory:IGenericFactory<StopPointsSettings>
    {
        private StrategyHeader strategyHeader;
        private string prefix;

        private StopPointsSettingsFactory() { }

        public StopPointsSettingsFactory(StrategyHeader strategyHeader, string prefix)
        {
            this.strategyHeader = strategyHeader;
            this.prefix = prefix;
        }

        public StopPointsSettings Make()
        {
            try
            {
                return new StopPointsSettings(this.strategyHeader,
                    AppSettings.GetValue<double>(String.Concat(this.prefix, "_StopPointsSettings_Points")),
                    AppSettings.GetValue<bool>(String.Concat(this.prefix, "_StopPointsSettings_Trail")));
            }
            catch
            {
                return null;
            }
        }
    }
}
