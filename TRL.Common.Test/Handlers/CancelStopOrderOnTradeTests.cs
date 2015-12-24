using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;
using TRL.Common.Handlers;
using TRL.Common.Collections;
using TRL.Common.Extensions.Data;
using TRL.Logging;

namespace TRL.Common.Handlers.Test
{
    [TestClass]
    public class CancelStopOrderOnTradeTests
    {
        private IDataContext tradingData;

        [TestInitialize]
        public void Handlers_Setup()
        {
            this.tradingData = new TradingDataContext();
            UpdatePositionOnTrade handler = new UpdatePositionOnTrade(this.tradingData, new NullLogger());
            CancelStopOrderOnTrade cancelHandler = new CancelStopOrderOnTrade(this.tradingData, new NullLogger());
        }

        [TestMethod]
        public void Handlers_cancel_long_position_stop_order_when_take_profit_order_begin_filling()
        {
            // Добавляем новую стратегию
            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-9.13_FT", 15);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(strategyHeader);

            // Добавляем настройки stop loss для стратегии
            StopPointsSettings slSettings = new StopPointsSettings(strategyHeader, 900, false);
            this.tradingData.Get<ICollection<StopPointsSettings>>().Add(slSettings);

            // Добавляем настройки take profit для стратегии
            ProfitPointsSettings tpSettings = new ProfitPointsSettings(strategyHeader, 1000, false);
            this.tradingData.Get<ICollection<ProfitPointsSettings>>().Add(tpSettings);

            // Имитируем генерацию сигнала на открытие позиции
            Signal signal = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(signal);

            Order order = new Order(signal);
            this.tradingData.Get<ICollection<Order>>().Add(order);

            // Брокер исполнил заявку на открытие позиции одной сделкой
            Trade trade = new Trade(order, order.Portfolio, order.Symbol, 150000, order.Amount, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(trade);

            // Позиция существует и ее объем равен объему в исполненной сделке
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Position>>().Count());
            Assert.AreEqual(15, this.tradingData.GetAmount(strategyHeader));

            // Имитируем автоматическую генерацию сигнала на установку stop loss заявки
            Signal slSignal = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Stop, 150000, 149100, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(slSignal);

            Order slOrder = new Order(slSignal);
            this.tradingData.Get<ICollection<Order>>().Add(slOrder);

            // Имитируем автоматическую генерацию сигнала на установку takep profit заявки
            Signal tpSignal = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Limit, 150000, 0, 151000);
            this.tradingData.Get<ICollection<Signal>>().Add(tpSignal);

            Order tpOrder = new Order(tpSignal);
            this.tradingData.Get<ICollection<Order>>().Add(tpOrder);

            // Рынок дошел до take profit но заявка исполнилась лишь частично
            Trade tpTrade = new Trade(tpOrder, tpOrder.Portfolio, tpOrder.Symbol, 151000, -8, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(tpTrade);

            // Объем позиции обновился
            Assert.AreEqual(7, this.tradingData.GetAmount(strategyHeader));

            // Обработчик автоматически отправил запрос на отмену stop loss заявки как только начала исполняться лимитная заявка
            Assert.AreEqual(1, this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Count);
            OrderCancellationRequest request = this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Last();
            Assert.AreEqual(slOrder, request.Order);
        }

        [TestMethod]
        public void Handlers_cancel_short_position_stop_order_when_take_profit_order_begin_filling()
        {
            // Добавляем новую стратегию
            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-9.13_FT", 15);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(strategyHeader);

            // Добавляем настройки stop loss для стратегии
            StopPointsSettings slSettings = new StopPointsSettings(strategyHeader, 900, false);
            this.tradingData.Get<ICollection<StopPointsSettings>>().Add(slSettings);

            // Добавляем настройки take profit для стратегии
            ProfitPointsSettings tpSettings = new ProfitPointsSettings(strategyHeader, 1000, false);
            this.tradingData.Get<ICollection<ProfitPointsSettings>>().Add(tpSettings);

            // Имитируем генерацию сигнала на открытие позиции
            Signal signal = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Limit, 150000, 0, 150000);
            this.tradingData.Get<ICollection<Signal>>().Add(signal);

            Order order = new Order(signal);
            this.tradingData.Get<ICollection<Order>>().Add(order);

            // Брокер исполнил заявку на открытие позиции одной сделкой
            Trade trade = new Trade(order, order.Portfolio, order.Symbol, 150000, -order.Amount, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(trade);

            // Позиция существует и ее объем равен объему в исполненной сделке
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Position>>().Count());
            Assert.AreEqual(-15, this.tradingData.GetAmount(strategyHeader));

            // Имитируем автоматическую генерацию сигнала на установку stop loss заявки
            Signal slSignal = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Stop, 150000, 150900, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(slSignal);

            Order slOrder = new Order(slSignal);
            this.tradingData.Get<ICollection<Order>>().Add(slOrder);

            // Имитируем автоматическую генерацию сигнала на установку takep profit заявки
            Signal tpSignal = new Signal(strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Limit, 150000, 0, 149000);
            this.tradingData.Get<ICollection<Signal>>().Add(tpSignal);

            Order tpOrder = new Order(tpSignal);
            this.tradingData.Get<ICollection<Order>>().Add(tpOrder);

            // Рынок дошел до take profit но заявка исполнилась лишь частично
            Trade tpTrade = new Trade(tpOrder, tpOrder.Portfolio, tpOrder.Symbol, 149000, 8, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(tpTrade);

            // Объем позиции обновился
            Assert.AreEqual(-7, this.tradingData.GetAmount(strategyHeader));

            // Обработчик автоматически отправил запрос на отмену stop loss заявки как только начала исполняться лимитная заявка
            Assert.AreEqual(1, this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Count);
            OrderCancellationRequest request = this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Last();
            Assert.AreEqual(slOrder, request.Order);
        }
    }
}
