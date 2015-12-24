using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;

namespace TRL.Common.Collections
{
    public class KeyValuePairArray<TKey, TValue>
    {
        private int length;
        private KeyValuePair<TKey, TValue>[] array;

        public KeyValuePairArray(int length)
        {
            this.length = length;
            this.array = new KeyValuePair<TKey, TValue>[this.length];
        }

        public KeyValuePair<TKey, TValue> this[int index]
        {
            get
            {
                return this.array[index];
            }
            set
            {
                if (index >= 0 && index < this.length)
                    this.array[index] = value;
            }
        }

        public int Length
        {
            get
            {
                return this.length;
            }
        }

        public void Update(int position, TKey key, TValue value)
        {
            this[position] = new KeyValuePair<TKey, TValue>(key, value);
        }
    }
}
