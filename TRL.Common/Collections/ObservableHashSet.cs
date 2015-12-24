using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Events;

namespace TRL.Common.Collections
{
    public class ObservableHashSet<T> : HashSet<T>, ItemAddedNotifier<T>
    {
        private List<IGenericObserver<T>> observers;

        public ObservableHashSet()
        {
            this.observers = new List<IGenericObserver<T>>();
        }

        public ObservableHashSet(IEqualityComparer<T> ec)
            : base(ec)
        {
            this.observers = new List<IGenericObserver<T>>();
        }

        public ObservableHashSet(EqualityComparer<T> ec)
            :base(ec)
        {            
            this.observers = new List<IGenericObserver<T>>();
        }

        public void NotifyObservers(T item)
        {
            foreach (IGenericObserver<T> observer in this.observers)
                observer.Update(item);
        }

        public void RegisterObserver(IGenericObserver<T> observer)
        {
            this.observers.Add(observer);
        }

        public new void Add(T item)
        {
            base.Add(item);
            this.NotifyObservers(item);

            if (this.OnItemAdded != null)
                this.OnItemAdded(item);
        }

        public event ItemAddedNotification<T> OnItemAdded;
    }
}
