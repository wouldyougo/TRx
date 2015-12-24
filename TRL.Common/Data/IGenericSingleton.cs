using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Data
{
    public interface IGenericSingleton<T>
        where T : class
    {
        T Instance { get; }
        void Destroy();
        bool IsNull { get; }
    }
}
