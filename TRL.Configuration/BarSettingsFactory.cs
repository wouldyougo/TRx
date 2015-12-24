using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Common;

namespace TRL.Configuration
{
    public class BarSettingsFactory:IGenericFactory<BarSettings>
    {
        private StrategyHeader strategyHeader;
        private string prefix;

        private BarSettingsFactory() { }

        public BarSettingsFactory(StrategyHeader strategyHeader, string prefix)
        {
            this.strategyHeader = strategyHeader;
            this.prefix = prefix;
        }

        public BarSettings Make()
        {
            try
            {
                return new BarSettings(this.strategyHeader,
                    AppSettings.GetStringValue(String.Concat(this.prefix, "_BarSettings_Symbol")),
                    AppSettings.GetValue<int>(String.Concat(this.prefix, "_BarSettings_Interval")),
                    AppSettings.GetValue<int>(String.Concat(this.prefix, "_BarSettings_Period")));
            }
            catch
            {
                return null;
            }
        }
    }
}
