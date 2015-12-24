using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Data
{
    public interface IQuoteProvider:IQuoteUpdateManager, ISymbolDataUpdatedNotifier
    {
        double GetBidPrice(string name, int position);
        double GetOfferPrice(string name, int position);
        double GetBidVolume(string name, int position);
        double GetOfferVolume(string name, int position);
    }
}
