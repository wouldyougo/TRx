using TRL.Csharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRL.Csharp.Collections
{
    public delegate void AddItemNotification<T>(T item);

    public class GenericObservableList<T>:List<T>
    {
        public event AddItemNotification<T> OnItemAdded;

        public new void Add(T item)
        {
            base.Add(item);

            if(OnItemAdded != null)
                OnItemAdded(item);
        }
    }
}
