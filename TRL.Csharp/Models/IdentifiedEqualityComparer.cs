using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRL.Csharp.Models
{
    public class IdentifiedEqualityComparer:IEqualityComparer<Identified>
    {
        public bool Equals(Identified x, Identified y)
        {
            if (x.Id == y.Id)
                return true;

            return false;
        }

        public int GetHashCode(Identified obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
