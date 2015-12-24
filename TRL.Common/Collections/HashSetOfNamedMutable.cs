using TRL.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Collections
{
    public class HashSetOfNamedMutable<T> : HashSet<T>
        where T : INamed, IMutable<T>
    {
        public HashSetOfNamedMutable()
            : this(new NamedEqualityComparer<T>()) { }

        public HashSetOfNamedMutable(IEqualityComparer<T> comparer)
            :base(comparer)
        {            
        }

        public new void Add(T item)
        {
            this.Update(item);
        }

        public void Update(T item)
        {
            T existing = this.Find(item);

            if (existing != null)
                existing.Update(item);
            else
                base.Add(item);
        }

        private T Find(T item)
        {
            try
            {
                return this.Single(i => i.Name == item.Name);
            }
            catch
            {
                return default(T);
            }
        }
    }
}
