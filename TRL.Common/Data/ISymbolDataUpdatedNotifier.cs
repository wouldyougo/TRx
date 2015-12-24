using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Data
{
    public delegate void SymbolDataUpdatedNotification(string symbol);

    public interface ISymbolDataUpdatedNotifier
    {
        event SymbolDataUpdatedNotification OnQuotesUpdate;
    }
}
