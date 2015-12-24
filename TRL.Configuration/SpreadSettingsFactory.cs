using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Common;

namespace TRL.Configuration
{
    public class SpreadSettingsFactory:IGenericFactory<SpreadSettings>
    {
        private string prefix;

        public SpreadSettingsFactory(string prefix)
        {
            this.prefix = prefix;
        }

        public SpreadSettings Make()
        {
            try
            {
                return new SpreadSettings(AppSettings.GetValue<double>(String.Concat(this.prefix, "_FairPrice")),
                    AppSettings.GetValue<double>(String.Concat(this.prefix, "_SellAfter")),
                    AppSettings.GetValue<double>(String.Concat(this.prefix, "_BuyBefore")));
            }
            catch
            {
                return null;
            }
        }
    }
}
