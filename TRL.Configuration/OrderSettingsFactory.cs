using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Common;

namespace TRL.Configuration
{
    public class OrderSettingsFactory:IGenericFactory<OrderSettings>
    {
        private StrategyHeader strategyHeader;
        private string prefix;

        public OrderSettingsFactory(StrategyHeader strategyHeader, string prefix)
        {
            this.strategyHeader = strategyHeader;
            this.prefix = prefix;
        }

        public OrderSettings Make()
        {
            try
            {
                return new OrderSettings(this.strategyHeader,
                    AppSettings.GetValue<int>(String.Concat(this.prefix, "_OrderSettings_TimeToLive")));
            }
            catch
            {
                return null;
            }
        }
    }
}
