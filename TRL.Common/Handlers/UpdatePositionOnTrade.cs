using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;
//using TRL.Common.Extensions.Models;
using TRL.Common.Collections;
using TRL.Common.Extensions.Collections;
using TRL.Common.Extensions;
using TRL.Common.Events;
using TRL.Common.Data;
using TRL.Logging;

namespace TRL.Common.Handlers
{
    public class UpdatePositionOnTrade:AddedItemHandler<Trade>
    {
        private IDataContext tradingData;
        private ILogger logger;


        public UpdatePositionOnTrade(IDataContext tradingData, ILogger logger)
            :base(tradingData.Get<ObservableHashSet<Trade>>())
        {
            this.logger = logger;
            this.tradingData = tradingData;
        }

        public override void OnItemAdded(Trade item)
        {
            if (!StrategyExists(item))
                return;

            if (item.Order.IsFilled)
            {
                if (Amount(GetOrderTrades(item.OrderId)) > item.Order.Amount)
                    this.tradingData.Get<ICollection<Trade>>().Remove(item);

                return;
            }

            if (!PositionExists(item))
            {
                CreatePosition(item);    
            }
            else
            {
                UpdatePosition(item);
            }

            UpdateOrderAmount(item);
        }

        private bool StrategyExists(Trade item)
        {

            if (item.Order == null)
                return false;

            if (item.Order.Signal == null)
                return false;

            if (item.Order.Signal.Strategy == null)
                return false;

            StrategyHeader strategyHeader = item.Order.Signal.Strategy;

            return this.tradingData.Get<IEnumerable<StrategyHeader>>().Any(s => s.Id == strategyHeader.Id
                && s.Description == strategyHeader.Description
                && s.Portfolio == strategyHeader.Portfolio
                && s.Symbol == strategyHeader.Symbol
                && s.Amount == strategyHeader.Amount);
        }

        private bool PositionExists(Trade item)
        {
            return this.tradingData.Get<ICollection<Position>>().Exists(item.Portfolio, item.Symbol);
        }

        private void CreatePosition(Trade item)
        {
            this.tradingData.Get<ObservableHashSet<Position>>().Add(new Position(item.Portfolio, item.Symbol, item.Amount));
            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сделкой {2} инициализирована позиция.", DateTime.Now, this.GetType().Name, item.ToString()));
        }

        private void UpdatePosition(Trade item)
        {
            try
            {
                Position position = this.tradingData.Get<IEnumerable<Position>>().Single(p => p.Portfolio == item.Portfolio && p.Symbol == item.Symbol);

                position.Amount += item.Amount;

                this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, позиция изменена {2}.", DateTime.Now, this.GetType().Name, position.ToString()));
            }
            catch
            {
                this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, при попытке изменить информацию о позиции {2} произошла ошибка.", DateTime.Now, this.GetType().Name, item.ToString()));
            }
        }

        private void UpdateOrderAmount(Trade item)
        {
            if(!item.Order.IsFilled)
                item.Order.FilledAmount += Math.Abs(item.Amount);
        }

        private double Amount(IEnumerable<Trade> trades)
        {
            return Math.Abs(trades.Sum(t => t.Amount));
        }

        private IEnumerable<Trade> GetOrderTrades(int orderId)
        {
            try
            {
                return this.tradingData.Get<IEnumerable<Trade>>().Where(t => t.OrderId == orderId);
            }
            catch
            {
                return null;
            }
        }
    }
}
