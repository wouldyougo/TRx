using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Events
{
    public delegate void ItemAddedNotification<T>(T item);

    public interface ItemAddedNotifier<T>
    {
        event ItemAddedNotification<T> OnItemAdded;
    }
}
