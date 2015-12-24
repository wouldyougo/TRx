using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;
using TRL.Common.Extensions.Data;
//using TRL.Common.Extensions;

namespace TRL.Common.Data
{
    public class PositionTradesFactory:IGenericFactory<ICollection<Trade>>
    {
        private IDataContext tradingData;
        private StrategyHeader strategyHeader;
        private double positionAmount;
        private PositionType positionType;
        private Trade[] strategyTrades;

        public PositionTradesFactory(IDataContext tradingData, StrategyHeader strategyHeader)
        {
            this.tradingData = tradingData;
            this.strategyHeader = strategyHeader;

            this.positionAmount = this.tradingData.GetAmount(this.strategyHeader);

            this.positionType = GetPositionType(this.positionAmount);

            this.strategyTrades = this.tradingData.GetTrades(this.strategyHeader).ToArray();
        }

        private PositionType GetPositionType(double amount)
        {
            if (amount > 0)
                return PositionType.Long;

            if (amount < 0)
                return PositionType.Short;

            return PositionType.Any;
        }

        public ICollection<Trade> Make()
        {
            List<Trade> result = new List<Trade>();

            double resultAmount = 0;

            if (this.positionAmount == 0)
                return result;

            for (int i = this.strategyTrades.Length - 1; i >= 0; i--)
            {
                if (resultAmount == this.positionAmount)
                    break;

                if (this.positionType == PositionType.Long && this.strategyTrades[i].Sell)
                    continue;

                if (this.positionType == PositionType.Short && this.strategyTrades[i].Buy)
                    continue;
                
                Trade clone = MakeTradeClone(this.strategyTrades[i], this.positionAmount - resultAmount);
                result.Add(clone);
                resultAmount += clone.Amount;
            }

            return result;
        }

        private Trade MakeTradeClone(Trade src, double restAmount)
        {
            Trade clone = new Trade(src);

            if (Math.Abs(src.Amount) > Math.Abs(restAmount))
                clone.Amount = restAmount;

            return clone;
        }
    }
}
