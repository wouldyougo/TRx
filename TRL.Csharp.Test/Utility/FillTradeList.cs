using TRL.Csharp.Collections;
using TRL.Csharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRL.Csharp.Test.Utility
{
    public static class FillTradeList
    {
        public static void FillListWithEvenBuyAndOddSale(this GenericObservableList<Trade> tradeList, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (i % 2 == 0)
                    tradeList.Add(new Trade(DateTime.Now, 1, 1));
                else
                    tradeList.Add(new Trade(DateTime.Now, 2, -1));
            }

        }

    }
}
