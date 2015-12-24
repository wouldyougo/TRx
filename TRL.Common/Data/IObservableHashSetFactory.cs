using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Collections;

namespace TRL.Common.Data
{
    public interface IObservableHashSetFactory
    {
        ObservableHashSet<T> Make<T>();
    }
}
