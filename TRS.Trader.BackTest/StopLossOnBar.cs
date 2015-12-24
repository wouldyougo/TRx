using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using TRL.Common.Extensions;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Collections;
using TRL.Common;
using TRL.Logging;

namespace TRx.Trader.BackTest
{
    public class StopLossOnBar:MakeClosePositionSignalByPointsOnBar
    {
        public StopLossOnBar(StrategyHeader strategyHeader,
            double points,
            IDataContext tradingData,
            ObservableQueue<Signal> signalQueue,
            ILogger logger)
            : base(strategyHeader,
            points,
            tradingData,
            signalQueue,
            logger) { }

        public override double CalculatePositionClosePrice()
        {
            Trade openTrade = this.tradingData.GetPositionOpenTrade(this.strategyHeader);

            return openTrade.Amount > 0 ? openTrade.Price - this.points : openTrade.Price + this.points;
        }

        public override bool ItsTimeToClosePosition(TRL.Common.Models.Bar item, double closePrice, TRL.Common.Models.TradeAction closeAction)
        {
            if (closeAction == TradeAction.Buy && item.Close < closePrice)
                return false;

            if (closeAction == TradeAction.Sell && item.Close > closePrice)
                return false;

            return true;
        }
    }
}
