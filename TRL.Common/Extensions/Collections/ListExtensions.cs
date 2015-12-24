using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Extensions.Collections
{
    public static class ListExtensions
    {
        public static bool ItemsAreOlderThanSeconds<T>(this List<T> items, int seconds)
            where T:IDateTime
        {
            if (items.Count == 0)
                return false;

            return BrokerDateTime.Make(DateTime.Now).AddSeconds(- seconds) > items.Last().DateTime;
        }
    }
}
