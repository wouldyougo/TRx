using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public class NamedEqualityComparer<T> : IEqualityComparer<T>
        where T : INamed
    {
        public bool Equals(T left, T right)
        {
            if (left.Name == right.Name)
                return true;

            return false;
        }

        public int GetHashCode(T item)
        {
            return item.Name.GetHashCode();
        }
    }
}
