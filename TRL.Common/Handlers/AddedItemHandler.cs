using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Events;

namespace TRL.Common.Handlers
{
    public abstract class AddedItemHandler<T>
    {
        protected ItemAddedNotifier<T> notifier;

        public AddedItemHandler(ItemAddedNotifier<T> notifier)
        {
            this.notifier = notifier;
            this.notifier.OnItemAdded += new ItemAddedNotification<T>(OnItemAdded);
        }

        public abstract void OnItemAdded(T item);
    }
}
