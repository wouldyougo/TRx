using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Events
{
    public interface IGenericBinder
    {
        void Bind<T>(T method);
        void Unbind<T>(T method);
    }
}
