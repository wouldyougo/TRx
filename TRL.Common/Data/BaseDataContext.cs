using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Collections;
using System.Reflection;

namespace TRL.Common.Data
{
    public abstract class BaseDataContext
    {
        public ObservableCollection<T> GetData<T>() where T : class
        {
            PropertyInfo[] properties = this.GetType().GetProperties();

            foreach (PropertyInfo item in properties)
            {
                object value = item.GetValue(this, null);

                if (value is ObservableCollection<T>)
                    return value as ObservableCollection<T>;
            }

            return null;
        }
    }
}
