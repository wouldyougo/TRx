using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Events
{
    public interface IGenericObserver<T>
    {
        void Update(T item);
    }
}
