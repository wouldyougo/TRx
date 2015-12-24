using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;
using TRL.Common.Data;
using System.Configuration;
using System.Globalization;
using TRL.Common;

namespace TRL.Configuration
{
    public class ArbitrageLegSettingsFactory:IGenericFactory<IEnumerable<StrategyHeader>>
    {
        private string prefix;

        public ArbitrageLegSettingsFactory(string prefix)
        {
            this.prefix = prefix;
        }

        private IEnumerable<int> MakeIds()
        {
            List<int> result = new List<int>();

            try
            {
                string ids = AppSettings.GetStringValue(String.Concat(this.prefix, "_Strategy_Id"));

                foreach (string part in ids.Split(','))
                {
                    result.Add(Convert.ToInt32(part, CultureInfo.InvariantCulture));
                }
            }
            catch
            {
            }

            return result;
        }

        private IEnumerable<string> MakeSymbols()
        {
            List<string> result = new List<string>();

            try
            {
                string ids = AppSettings.GetStringValue(String.Concat(this.prefix, "_Strategy_Symbol"));

                foreach (string part in ids.Split(','))
                {
                    result.Add(part);
                }
            }
            catch
            {
            }

            return result;
        }

        private IEnumerable<string> MakePortfolios()
        {
            List<string> result = new List<string>();

            try
            {
                string ids = AppSettings.GetStringValue(String.Concat(this.prefix, "_Strategy_Portfolio"));

                foreach (string part in ids.Split(','))
                {
                    result.Add(part);
                }
            }
            catch
            {
            }

            return result;
        }

        private IEnumerable<double> MakeAmounts()
        {
            List<double> result = new List<double>();

            try
            {
                string ids = AppSettings.GetStringValue(String.Concat(this.prefix, "_Strategy_Amount"));

                foreach (string part in ids.Split(','))
                {
                    result.Add(Convert.ToDouble(part, CultureInfo.InvariantCulture));
                }
            }
            catch
            {
            }

            return result;
        }
        private string MakeDescription()
        {
            try
            {
                return AppSettings.GetStringValue(String.Concat(this.prefix, "_Strategy_Description"));
            }
            catch
            {
                return "Arbitrage spread";
            }
        }

        public IEnumerable<StrategyHeader> Make()
        {
            IEnumerable<int> ids = MakeIds();
            string description = MakeDescription();
            IEnumerable<string> portfolios = MakePortfolios();
            IEnumerable<string> symbols = MakeSymbols();
            IEnumerable<double> amounts = MakeAmounts();

            List<StrategyHeader> result = new List<StrategyHeader>();

            if(portfolios.Count() == 1)
            {
                for(int i = 0; i < ids.Count() ; i++)
                    result.Add(new StrategyHeader(ids.ElementAt(i), description, portfolios.ElementAt(0), symbols.ElementAt(i), amounts.ElementAt(i)));
            }
            else
            {
                for (int i = 0; i < ids.Count(); i++)
                    result.Add(new StrategyHeader(ids.ElementAt(i), description, portfolios.ElementAt(i), symbols.ElementAt(i), amounts.ElementAt(i)));
            }

            return result;
        }

    }
}
