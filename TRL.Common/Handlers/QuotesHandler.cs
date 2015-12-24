using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;

namespace TRL.Common.Handlers
{
    public abstract class QuotesHandler
    {
        protected ISymbolDataUpdatedNotifier quoteUpdateNotifier;

        public QuotesHandler(ISymbolDataUpdatedNotifier quoteUpdateNotifier)
        {
            this.quoteUpdateNotifier = quoteUpdateNotifier;
            this.quoteUpdateNotifier.OnQuotesUpdate += new SymbolDataUpdatedNotification(OnQuotesUpdate);
        }

        public abstract void OnQuotesUpdate(string symbol);
    }
}
