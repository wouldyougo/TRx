using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TRL.Csharp.Data
{
    public abstract class BaseDataContext:DataContext
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
