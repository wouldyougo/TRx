using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace TRL.Common
{
    public static class StructFactory
    {
        public static Nullable<T> MakeNullable<T>(string source) where T : struct
        {
            if (IsEmptyString(source))
                throw new ArgumentException(String.Format("Невозможно преобразовать пустую строку в значение типа {0}!", typeof(T)), "source");
            try{
                return (Nullable<T>)MakeTypeConverter(typeof(T)).ConvertFrom(source);
            }
            catch
            {
                return (Nullable<T>)MakeTypeConverter(typeof(T)).ConvertFrom(null, CultureInfo.InvariantCulture, source);
            }
        }

        public static T Make<T>(string source) where T : struct
        {
            if (IsEmptyString(source))
                throw new ArgumentException(String.Format("Невозможно преобразовать пустую строку в значение типа {0}!", typeof(T)), "source");
            try
            {
                return (T)MakeTypeConverter(typeof(T)).ConvertFrom(source);
            }
            catch
            {
                return (T)MakeTypeConverter(typeof(T)).ConvertFrom(null, CultureInfo.InvariantCulture, source);
            }
        }

        private static bool IsEmptyString(string source)
        {
            return String.IsNullOrEmpty(source) || String.IsNullOrWhiteSpace(source);
        }

        private static TypeConverter MakeTypeConverter(Type type)
        {
            return TypeDescriptor.GetConverter(type);
        }

    }
}
