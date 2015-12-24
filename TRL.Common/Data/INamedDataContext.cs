using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;

namespace TRL.Common.Data
{
    public interface INamedDataContext
    {
        T Get<T>(string name) where T : INamed;
    }
}
