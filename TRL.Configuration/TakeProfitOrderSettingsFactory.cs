using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Common;

namespace TRL.Configuration
{
    public class TakeProfitOrderSettingsFactory:IGenericFactory<TakeProfitOrderSettings>
    {
        private StrategyHeader strategyHeader;
        private string prefix;

        public TakeProfitOrderSettingsFactory(StrategyHeader strategyHeader, string prefix)
        {
            this.strategyHeader = strategyHeader;
            this.prefix = prefix;
        }

        public TakeProfitOrderSettings Make()
        {
            try
            {
                return new TakeProfitOrderSettings(this.strategyHeader,
                    AppSettings.GetValue<int>(String.Concat(this.prefix, "_TakeProfitOrderSettings_TimeToLive")));
            }
            catch
            {
                return null;
            }
        }
    }
}
