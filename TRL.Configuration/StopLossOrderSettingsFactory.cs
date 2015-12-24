using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Common;

namespace TRL.Configuration
{
    public class StopLossOrderSettingsFactory:IGenericFactory<StopLossOrderSettings>
    {
        private StrategyHeader strategyHeader;
        private string prefix;

        public StopLossOrderSettingsFactory(StrategyHeader strategyHeader, string prefix)
        {
            this.strategyHeader = strategyHeader;
            this.prefix = prefix;
        }

        public StopLossOrderSettings Make()
        {
            try
            {
                return new StopLossOrderSettings(this.strategyHeader,
                    AppSettings.GetValue<int>(String.Concat(this.prefix, "_StopLossOrderSettings_TimeToLive")));
            }
            catch
            {
                return null;
            }
        }
    }
}
