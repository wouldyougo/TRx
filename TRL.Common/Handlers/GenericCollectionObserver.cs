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
    public abstract class GenericCollectionObserver<T>:IGenericObserver<T>
        where T:class
    {
        protected BaseDataContext dataContext;

        public GenericCollectionObserver(BaseDataContext dataContext)
        {
            this.dataContext = dataContext;
            this.dataContext.GetData<T>().RegisterObserver(this);
        }

        public abstract void Update(T item);
    }
}
