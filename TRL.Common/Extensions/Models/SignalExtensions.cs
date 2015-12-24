using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;

namespace TRL.Common.Extensions.Models
{
    public static class SignalExtensions
    {
        public static double GetSignedAmount(this Signal signal)
        {
            if (signal.TradeAction == TradeAction.Buy)
                return signal.Strategy.Amount;

            return signal.Strategy.Amount * -1;
        }
    }
}
