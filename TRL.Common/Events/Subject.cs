using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Events
{
    public class Subject
    {
        private List<IObserver> observers;

        public Subject()
        {
            this.observers = new List<IObserver>();
        }

        public void NotifyObservers()
        {
            foreach (IObserver observer in this.observers)
                observer.Update();
        }

        public void RegisterObserver(IObserver observer)
        {
            this.observers.Add(observer);
        }
    }
}
