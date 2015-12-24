using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public interface IMutable<T>
    {
        void Update(T item);
    }
}
