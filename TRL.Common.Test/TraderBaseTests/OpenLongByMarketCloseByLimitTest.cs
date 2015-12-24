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
    public class OpenLongByMarketCloseByLimitTest:TraderBaseInitializer
    {

        private StrategyHeader strategyHeader;

        [TestInitialize]
        public void Setup()
        {
            Symbol symbol = new Symbol("RTS-9.13_FT", 1, 8, 10, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<HashSetOfNamedMutable<Symbol>>().Add(symbol);

            this.strategyHeader = new StrategyHeader(10, "strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);

            StopPointsSettings slSettings = new StopPointsSettings(this.strategyHeader, 300, false);
            this.tradingData.Get<ICollection<StopPointsSettings>>().Add(slSettings);

            ProfitPointsSettings tpSettings = new ProfitPointsSettings(this.strategyHeader, 500, false);
            this.tradingData.Get<ICollection<ProfitPointsSettings>>().Add(tpSettings);

            StopLossOrderSettings slOrderSettings = new StopLossOrderSettings(this.strategyHeader, 3600);
            this.tradingData.Get<ICollection<StopLossOrderSettings>>().Add(slOrderSettings);

            TakeProfitOrderSettings tpOrderSettings = new TakeProfitOrderSettings(this.strategyHeader, 3600);
            this.tradingData.Get<ICollection<TakeProfitOrderSettings>>().Add(tpOrderSettings);

            StrategyStopLossByPointsOnTick stopLossHandler =
                new StrategyStopLossByPointsOnTick(strategyHeader, this.tradingData, this.signalQueue, new NullLogger());
            StrategyTakeProfitByPointsOnTick takeProfitHandler =
                new StrategyTakeProfitByPointsOnTick(strategyHeader, this.tradingData, this.signalQueue, new NullLogger());

            PlaceStrategyStopLossByPointsOnTrade placeStopOnTradeHandler =
                new PlaceStrategyStopLossByPointsOnTrade(strategyHeader, this.tradingData, this.signalQueue, new NullLogger());
            PlaceStrategyTakeProfitByPointsOnTrade placeTakeProfitOnTradeHandler =
                new PlaceStrategyTakeProfitByPointsOnTrade(strategyHeader, this.tradingData, this.signalQueue, new NullLogger());

        }

        [TestMethod]
        public void open_long_position_with_market_order_protect_it_with_stop_and_limit_and_close_with_limit()
        {
            // Сигнал на открытие позиции
            Signal inputSignal = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 150000, 0, 0);
            EmulateTradeFor(inputSignal, 150050);

            // Заявка исполнена, позиция открыта ровно на запрошенный в заявке объем
            Assert.AreEqual(10, this.tradingData.GetAmount(this.strategyHeader));

            // Для позиции созданы и отправлены брокеру защитные стоп и тейк профит приказы
            Assert.AreEqual(3, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(3, this.tradingData.Get<IEnumerable<Order>>().Count());
            Signal slSignal = this.tradingData.Get<IEnumerable<Signal>>().Single(s => s.OrderType == OrderType.Stop && s.StrategyId == this.strategyHeader.Id);
            Signal tpSignal = this.tradingData.Get<IEnumerable<Signal>>().Single(s => s.OrderType == OrderType.Limit && s.StrategyId == this.strategyHeader.Id);

            // Цена защитных приказов установлена соответственно настройкам
            Assert.AreEqual(149750, slSignal.Stop);
            Assert.AreEqual(150550, tpSignal.Limit);

            // Через некоторое время цена на рынке вырастает, срабатывает take profit приказ и исполняется одной сделкой
            EmulateTradeFor(tpSignal, 145500);

            Order slOrder = 
                this.tradingData.Get<IEnumerable<Order>>().Single(o => o.SignalId == slSignal.Id);
            Order tpOrder = 
                this.tradingData.Get<IEnumerable<Order>>().Single(o => o.SignalId == tpSignal.Id);

            Assert.IsTrue(tpOrder.IsFilled);
            Assert.IsFalse(slOrder.IsFilled);
            Assert.IsFalse(slOrder.IsCanceled);

            // Позиция закрыта
            Assert.AreEqual(0, this.tradingData.GetAmount(this.strategyHeader));
            
            // Брокеру отправлен запрос на отмену stop loss приказа
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<OrderCancellationRequest>>().Count());            
            OrderCancellationRequest slRequest = this.tradingData.Get<IEnumerable<OrderCancellationRequest>>().Single(r => r.OrderId == slOrder.Id);
            Assert.IsNotNull(slRequest);
        }
    }
}
