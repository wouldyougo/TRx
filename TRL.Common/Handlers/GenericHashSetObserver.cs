using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using TRL.Common.Collections;
using TRL.Common.Models;
using TRL.Common.Events;

namespace TRL.Common.Handlers
{
    public abstract class GenericHashSetObserver<T>:IGenericObserver<T>
    {
        protected IObservableHashSetFactory tradingData;

        public GenericHashSetObserver(IObservableHashSetFactory tradingData)
        {
            this.tradingData = tradingData;
            this.tradingData.Make<T>().RegisterObserver(this);
        }

        public abstract void Update(T item);
    }
}
