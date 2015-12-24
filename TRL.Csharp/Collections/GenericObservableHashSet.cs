using TRL.Csharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRL.Csharp.Collections
{
    public class GenericObservableHashSet<T> : HashSet<T>
    {
        public event AddItemNotification<T> OnItemAdded;

        public GenericObservableHashSet(IEqualityComparer<T> comparer)
            :base(comparer)
        {

        }

        public new void Add(T item)
        {
            base.Add(item);

            if(OnItemAdded != null)
                OnItemAdded(item);
        }
    }
}
