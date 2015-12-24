using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Collections;
using System.Reflection;
using TRL.Common.Models;

namespace TRL.Common.Data
{
    public abstract class RawBaseNamedDataContext : INamedDataContext
    {
        public T Get<T>(string name) where T : INamed
        {
            PropertyInfo[] properties = this.GetType().GetProperties();

            foreach (PropertyInfo item in properties)
            {
                object value = item.GetValue(this, null);

                if (value is T)
                {
                    T result = (T)value;

                    if(result.Name.Equals(name))
                        return result;
                }
            }

            return default(T);
        }
    }
}
