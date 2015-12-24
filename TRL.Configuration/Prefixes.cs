using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common;

namespace TRL.Configuration
{
    public static class Prefixes
    {
        public static IEnumerable<string> Make()
        {
            string prefixes = AppSettings.GetStringValue("Prefixes");

            string[] src = prefixes.Split(',');

            List<string> result = new List<string>();

            for (int i = 0; i < src.Length; i++)
            {
                string s = src[i].Trim();

                if (!string.IsNullOrEmpty(s) && !string.IsNullOrWhiteSpace(s))
                    result.Add(s);
            }

            return result;
        }
    }
}
