using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRL.Csharp.Data
{
    public interface DataContext
    {
        T Get<T>();
    }
}
