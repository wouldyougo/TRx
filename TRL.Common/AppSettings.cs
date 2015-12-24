using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using TRL.Common;
using TRL.Common.TimeHelpers;

namespace TRL.Common
{
    public static class AppSettings
    {
        public static string GetStringValue(string keyName)
        {
            string value = ConfigurationManager.AppSettings[keyName];

            if (StringHasValue(value))
                return value;

            throw new ArgumentNullException(keyName, "Указанный конфигурационный параметр отсутствует в разделе appSettings приложения!");
        }

        public static T GetValue<T>(string keyName)where T:struct
        {
            string value = ConfigurationManager.AppSettings[keyName];

            if (StringHasValue(value))
                return StructFactory.Make<T>(value);

            if (typeof(T).Equals(typeof(bool)))
                return default(T);

            throw new ArgumentNullException(keyName, "Указанный конфигурационный параметр отсутствует в разделе appSettings приложения!");
        }

        private static bool StringHasValue(string str)
        {
            return !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);
        }

    }
}
