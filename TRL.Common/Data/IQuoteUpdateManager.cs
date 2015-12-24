using TRL.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Data
{
    public interface IQuoteUpdateManager
    {
        void Update(int index, string symbol, double bid, double bidVolume, double offer, double offerVolume);
    }
}
