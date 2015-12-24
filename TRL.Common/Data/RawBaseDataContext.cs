using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace TRL.Common.Data
{
    public abstract class RawBaseDataContext:IDataContext
    {
        public T Get<T>()
        {
            PropertyInfo[] properties = this.GetType().GetProperties();

            foreach (PropertyInfo item in properties)
            {
                object value = item.GetValue(this, null);

                if (value is T)
                    return (T)value;
            }

            return default(T);
        }
    }
}
