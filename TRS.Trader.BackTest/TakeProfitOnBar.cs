using TRL.Common.Handlers;
using TRL.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using TRL.Common.Extensions;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Collections;
using TRL.Common;
using TRL.Logging;
using TRL.Common.TimeHelpers;

namespace TRx.Trader.BackTest
{
    public class TakeProfitOnBar : MakeClosePositionSignalByPointsOnBar
    {
        public TakeProfitOnBar(StrategyHeader strategyHeader, 
            double points, 
            IDataContext tradingData, 
            ObservableQueue<Signal> signalQueue, 
            ILogger logger)
            : base(strategyHeader, 
            points,
            tradingData,
            signalQueue,
            logger)
        {}

        public override double CalculatePositionClosePrice()
        {
            Trade openTrade = this.tradingData.GetPositionOpenTrade(this.strategyHeader);

            return openTrade.Amount > 0 ? openTrade.Price + this.points : openTrade.Price - this.points;
        }

        public override bool ItsTimeToClosePosition(Bar item, double closePrice, TradeAction closeAction)
        {
            if (closeAction == TradeAction.Buy && item.Close > closePrice)
                return false;

            if (closeAction == TradeAction.Sell && item.Close < closePrice)
                return false;

            return true;
        }

    }

}
