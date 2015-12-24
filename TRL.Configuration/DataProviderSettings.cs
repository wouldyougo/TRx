using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common;

namespace TRL.Configuration
{
    public class DataProviderSettings
    {
        public bool ListenPortfolio { get; private set; }
        public bool ListenQuotes { get; private set; }
        public bool ListenBidAsk { get; private set; }
        public bool ListenTicks { get; private set; }

        public DataProviderSettings()
        {
            this.ListenPortfolio = AppSettings.GetValue<bool>("ListenPortfolio");
            this.ListenQuotes = AppSettings.GetValue<bool>("ListenQuotes");
            this.ListenTicks = AppSettings.GetValue<bool>("ListenTicks");
            this.ListenBidAsk = AppSettings.GetValue<bool>("ListenBidAsk");
        }
    }
}
