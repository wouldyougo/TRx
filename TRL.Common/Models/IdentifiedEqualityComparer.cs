using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public class IdentifiedEqualityComparer<T>:IEqualityComparer<T>
        where T : IIdentified
    {
        public bool Equals(T left, T right)
        {
            if (left.Id == right.Id)
                return true;

            return false;
        }

        public int GetHashCode(T item)
        {
            return item.Id.GetHashCode();
        }
    }
}
