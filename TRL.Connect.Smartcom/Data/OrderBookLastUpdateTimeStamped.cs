using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common;

namespace TRL.Connect.Smartcom.Data
{
    public class OrderBookLastUpdateTimeStamped : IDateTime
    {
        private ISymbolDataUpdatedNotifier notifier;

        public OrderBookLastUpdateTimeStamped(ISymbolDataUpdatedNotifier notifier)
        {
            this.notifier = notifier;
            this.lastNotification = DateTime.MinValue;
            this.notifier.OnQuotesUpdate += UpdateLastNotificationOnQuotesUpdate;
        }

        private void UpdateLastNotificationOnQuotesUpdate(string symbol)
        {
            this.lastNotification = BrokerDateTime.Make(DateTime.Now);
        }

        private DateTime lastNotification;
        public DateTime DateTime
        {
            get { return this.lastNotification; }
        }
    }
}
