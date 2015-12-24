using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Common;

namespace TRL.Configuration
{
    public class StrategyFactory:IGenericFactory<StrategyHeader>
    {
        private string prefix;

        private StrategyFactory() { }

        public StrategyFactory(string prefix)
        {
            this.prefix = prefix;
        }

        public StrategyHeader Make()
        {
            try
            {
                return new StrategyHeader(AppSettings.GetValue<int>(String.Concat(this.prefix, "_Strategy_Id")),
                    AppSettings.GetStringValue(String.Concat(this.prefix, "_Strategy_Description")),
                    AppSettings.GetStringValue(String.Concat(this.prefix, "_Strategy_Portfolio")),
                    AppSettings.GetStringValue(String.Concat(this.prefix, "_Strategy_Symbol")),
                    AppSettings.GetValue<double>(String.Concat(this.prefix, "_Strategy_Amount")));
            }
            catch
            {
                return null;
            }
        }
    }
}
