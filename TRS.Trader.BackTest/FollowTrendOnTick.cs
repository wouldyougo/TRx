using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRL.Common.TimeHelpers;

using TRL.Common;
using TRL.Common.Collections;
using TRL.Common.Extensions.Collections;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Handlers;
using TRL.Common.Models;
using TRL.Logging;
//using TRL.Common.Extensions.Models;
//using TRL.Common.Extensions;


namespace TRx.Trader.BackTest
{
    public class FollowTrendOnTick:AddedItemHandler<Tick>
    {
        private StrategyHeader strategyHeader;
        private OrderBookContext orderBook;
        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;
        private int seconds;
        private double priceSpan;
        private ILogger logger;

        public FollowTrendOnTick(StrategyHeader strategyHeader, 
            int seconds,
            double priceSpan,
            OrderBookContext orderBook, 
            IDataContext tradingData, 
            ObservableQueue<Signal> signalQueue, 
            ILogger logger)
            :base(tradingData.Get<ObservableCollection<Tick>>())
        {
            this.strategyHeader = strategyHeader;
            this.seconds = seconds;
            this.priceSpan = priceSpan;
            this.orderBook = orderBook;
            this.tradingData = tradingData;
            this.signalQueue = signalQueue;
            this.logger = logger;
        }

        public override void OnItemAdded(Tick item)
        {
            if (item.Symbol != this.strategyHeader.Symbol)
                return;

            if (this.tradingData.GetAmount(this.strategyHeader) != 0)
                return;

            IEnumerable<Tick> ticks = 
                this.tradingData.
                Get<IEnumerable<Tick>>().
                Last(item.Symbol, new TimeSpan(0, 0, this.seconds));

            if (ticks.Max(t => t.Price) - ticks.Min(t => t.Price) < this.priceSpan)
                return;

            double price = 0;
            Signal signal = null;

            if (ticks.Last().Price > ticks.First().Price)
            {
                price = this.orderBook.GetOfferPrice(this.strategyHeader.Symbol, 0);
                signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, price, 0, price);
            }
            else if (ticks.Last().Price < ticks.First().Price)
            {
                price = this.orderBook.GetBidPrice(this.strategyHeader.Symbol, 0);
                signal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Limit, price, 0, price);
            }

            this.logger.Log(String.Format("Сгенерирован новый сигнал {0}", signal.ToString()));
            this.signalQueue.Enqueue(signal);

        }
    }
}
