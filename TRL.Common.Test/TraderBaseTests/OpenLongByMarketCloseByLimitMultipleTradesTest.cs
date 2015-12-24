using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Collections;
using TRL.Common.Models;
using TRL.Common.Test.Mocks;
using TRL.Common.TimeHelpers;
using TRL.Common.Extensions.Data;
using TRL.Emulation;
using TRL.Handlers.StopLoss;
using TRL.Handlers.TakeProfit;
using TRL.Logging;

namespace TRL.Common.Test.TraderBaseTests
{
    [TestClass]
    public class OpenLongByMarketCloseByLimitMultipleTradesTest:TraderBaseInitializer
    {

        [TestMethod]
        public void open_long_position_with_market_order_protect_it_with_stop_and_limit_and_close_with_limit_multiple_trades()
        {
            // Настройки для торгуемой стратегии
            Symbol symbol = new Symbol("RTS-9.13_FT", 1, 8, 10, BrokerDateTime.Make(DateTime.Now));         
            this.tradingData.Get<HashSetOfNamedMutable<Symbol>>().Add(symbol);

            StrategyHeader strategyHeader = new StrategyHeader(6, "strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 10);                
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(strategyHeader);

            StopPointsSettings slSettings = new StopPointsSettings(strategyHeader, 300, false);
            this.tradingData.Get<ICollection<StopPointsSettings>>().Add(slSettings);

            ProfitPointsSettings tpSettings = new ProfitPointsSettings(strategyHeader, 500, false);
            this.tradingData.Get<ICollection<ProfitPointsSettings>>().Add(tpSettings);

            StopLossOrderSettings slOrderSettings = new StopLossOrderSettings(strategyHeader, 3600);
            this.tradingData.Get<ICollection<StopLossOrderSettings>>().Add(slOrderSettings);

            TakeProfitOrderSettings tpOrderSettings = new TakeProfitOrderSettings(strategyHeader, 3600);
            this.tradingData.Get<ICollection<TakeProfitOrderSettings>>().Add(tpOrderSettings);

            StrategyStopLossByPointsOnTick stopLossHandler =
                new StrategyStopLossByPointsOnTick(strategyHeader, this.tradingData, this.signalQueue, new NullLogger());
            StrategyTakeProfitByPointsOnTick takeProfitHandler =
                new StrategyTakeProfitByPointsOnTick(strategyHeader, this.tradingData, this.signalQueue, new NullLogger());

            PlaceStrategyStopLossByPointsOnTrade placeStopOnTradeHandler =
                new PlaceStrategyStopLossByPointsOnTrade(strategyHeader, this.tradingData, this.signalQueue, new NullLogger());
            PlaceStrategyTakeProfitByPointsOnTrade placeTakeProfitOnTradeHandler =
                new PlaceStrategyTakeProfitByPointsOnTrade(strategyHeader, this.tradingData, this.signalQueue, new NullLogger());


            // Сигнал на открытие позиции
            Signal inputSignal = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            EmulateTradeFor(inputSignal, 150050, 3);

            // Копия отправленной брокеру заявки на открытие позиции
            Order inputOrder = this.tradingData.Get<IEnumerable<Order>>().Single(o => o.SignalId == inputSignal.Id);

            // Часть заявки исполнена, позиция открыта ровно на исполненный сделкой объем
            Assert.IsFalse(inputOrder.IsFilled);
            Assert.IsTrue(inputOrder.IsFilledPartially);
            Assert.AreEqual(3, this.tradingData.GetAmount(strategyHeader));

            // Заявки на приказы stop loss и take profit не генерируются, поскольку рыночная заявка не исполнена
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());

            // Следующая сделка
            EmulateTradeFor(inputSignal, 150060, 5);

            // Часть заявки исполнена, позиция увеличилась ровно на исполненный сделкой объем
            Assert.IsFalse(inputOrder.IsFilled);
            Assert.IsTrue(inputOrder.IsFilledPartially);
            Assert.AreEqual(8, this.tradingData.GetAmount(strategyHeader));

            // Заявки на приказы stop loss и take profit не генерируются, поскольку рыночная заявка не исполнена
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Order>>().Count());

            EmulateTradeFor(inputSignal, 150070, 2);

            // Заявка полностью исполнена, позиция увеличилась ровно на исполненный сделкой объем
            Assert.IsTrue(inputOrder.IsFilled);
            Assert.IsFalse(inputOrder.IsFilledPartially);
            Assert.AreEqual(10, this.tradingData.GetAmount(strategyHeader));

            // Для позиции созданы и отправлены брокеру защитные стоп и тейк профит приказы
            Assert.AreEqual(3, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(3, this.tradingData.Get<IEnumerable<Order>>().Count());

            Signal slSignal = this.tradingData.Get<IEnumerable<Signal>>().Single(s => s.OrderType == OrderType.Stop);
            Signal tpSignal = this.tradingData.Get<IEnumerable<Signal>>().Single(s => s.OrderType == OrderType.Limit);
            Order slOrder = this.tradingData.Get<IEnumerable<Order>>().Single(o => o.SignalId == slSignal.Id);
            Order tpOrder = this.tradingData.Get<IEnumerable<Order>>().Single(o => o.SignalId == tpSignal.Id);

            // Цена защитных приказов установлена соответственно настройкам
            Assert.AreEqual(149770, slOrder.Stop);
            Assert.AreEqual(150570, tpOrder.Price);

            // Через некоторое время цена на рынке вырастает, срабатывает приказ take profit и исполняется первая сделка
            EmulateTradeFor(tpSignal, 150500, 4);

            // Take profit приказ исполнен лишь частично. Позиция не закрыта. Stop loss приказ не отменен
            Assert.IsFalse(tpOrder.IsFilled);
            Assert.IsTrue(tpOrder.IsFilledPartially);
            Assert.AreEqual(6, this.tradingData.GetAmount(strategyHeader));
            Assert.IsFalse(slOrder.IsCanceled);

            // Исполняется вторая сделка
            EmulateTradeFor(tpSignal, 150500, 5);

            // Take profit приказ исполнен лишь частично. Позиция не закрыта. Stop loss приказ не отменен
            Assert.IsFalse(tpOrder.IsFilled);
            Assert.IsTrue(tpOrder.IsFilledPartially);
            Assert.AreEqual(1, this.tradingData.GetAmount(strategyHeader));
            Assert.IsFalse(slOrder.IsCanceled);

            // Исполняется третья сделка
            EmulateTradeFor(tpSignal, 150500, 1);

            // Take profit приказ исполнен
            Assert.IsTrue(tpOrder.IsFilled);
            Assert.IsFalse(slOrder.IsFilled);
            Assert.IsFalse(slOrder.IsCanceled);

            // Позиция закрыта
            Assert.AreEqual(0, this.tradingData.GetAmount(strategyHeader));

            // Брокеру отправлен запрос на отмену приказа stop loss
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<OrderCancellationRequest>>().Count());

            // Брокер подтверждает получение заявки на отмену приказа
            this.tradingData.Get<ObservableHashSet<OrderCancellationConfirmation>>().Add(new OrderCancellationConfirmation(slOrder, BrokerDateTime.Make(DateTime.Now), "Заявка снята"));
            Assert.IsTrue(slOrder.IsCanceled);

        }
    }
}
