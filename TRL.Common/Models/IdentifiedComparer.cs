using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public class IdentifiedComparer : EqualityComparer<IIdentified>
    {
        public override bool Equals(IIdentified x, IIdentified y)
        {
            if (x.Id == y.Id)
                return true;

            return false;
        }

        public override int GetHashCode(IIdentified obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
