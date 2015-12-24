using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Events;

namespace TRL.Common.Collections
{
    /// <summary>
    /// Обозреваемая очередь каких-либо объектов. Очередь уведомляет
    /// зарегистрированных обозревателей о том что в нее поместили
    /// новый объект.
    /// </summary>
    /// <typeparam name="T">Тип "какого-либо" объекта</typeparam>
    public class ObservableQueue<T> : Queue<T>
    {
        private List<IObserver> observers;

        public ObservableQueue()
        {
            this.observers = new List<IObserver>();
        }

        /// <summary>
        /// Зарегистрировать нового обозревателя очереди.
        /// </summary>
        /// <param name="observer">Ссылка на обозревателя.</param>
        public void RegisterObserver(IObserver observer)
        {
            this.observers.Add(observer);
        }

        /// <summary>
        /// Вызов метода отправляет уведомления всем обозревателям.
        /// </summary>
        public void NotifyObservers()
        {
            foreach (IObserver observer in this.observers)
                observer.Update();
        }

        public new void Enqueue(T item)
        {
            base.Enqueue(item);
            this.NotifyObservers();
        }
    }
}
