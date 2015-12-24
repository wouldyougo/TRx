using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Events
{
    public interface IBinder
    {
        void Bind();
        void Unbind();
        int BindedHandlersCounter { get; }
    }
}
