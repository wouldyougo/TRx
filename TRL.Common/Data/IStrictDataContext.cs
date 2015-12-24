using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Data
{
    public interface IStrictDataContext
    {
        HashSet<T> GetData<T>() where T : class;
    }
}
