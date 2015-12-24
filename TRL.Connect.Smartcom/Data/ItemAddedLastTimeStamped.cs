using TRL.Common.Events;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common;

namespace TRL.Connect.Smartcom.Data
{
    public class ItemAddedLastTimeStamped<T> : IDateTime
    {
        private ItemAddedNotifier<T> notifier;

        public ItemAddedLastTimeStamped(ItemAddedNotifier<T> notifier)
        {
            this.notifier = notifier;
            this.notifier.OnItemAdded += UpdateLastNotificationOnItemAdded;
            this.lastNotification = DateTime.MinValue;
        }

        public void UpdateLastNotificationOnItemAdded(T item)
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
