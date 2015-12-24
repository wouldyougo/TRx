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
using TRL.Emulation;
using TRL.Handlers.StopLoss;
using TRL.Handlers.TakeProfit;
using TRL.Common.Extensions.Data;
using TRL.Logging;
using TRL.Common.Handlers;

namespace TRL.Common.Test.TraderBaseTests
{
    [TestClass]
    public class OpenLongByMarketLimitFailedCloseByEmergencyMarketTest
    {
        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;
        private ObservableQueue<Order> orderQueue;
        private IOrderManager orderManager;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            this.signalQueue = new ObservableQueue<Signal>();
            this.orderQueue = new ObservableQueue<Order>();
            this.orderManager = new MockOrderManager();

            TraderBase traderBase = new TraderBase(this.tradingData, this.signalQueue, this.orderQueue, this.orderManager, new AlwaysTimeToTradeSchedule(), new NullLogger());
        }

        [TestMethod]
        public void open_long_position_with_market_order_protect_it_with_stop_limit_rejected_close_with_emergency_market()
        {
            // Настройки для торгуемой стратегии
            Symbol symbol = new Symbol("RTS-9.13_FT", 1, 8, 10, BrokerDateTime.Make(DateTime.Now));         
            this.tradingData.Get<ICollection<Symbol>>().Add(symbol);

            StrategyHeader strategyHeader = new StrategyHeader(1, "strategyHeader", "BP12345-RF-01", "RTS-9.13_FT", 10);                
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

            //// Брокер исполнил заявку одной сделкой
            Trade inputTrade = this.tradingData.AddSignalAndItsOrderAndTrade(inputSignal);

            // Заявка исполнена, позиция открыта ровно на запрошенный в заявке объем
            Assert.IsTrue(inputTrade.Order.IsFilled);
            double amount = this.tradingData.GetAmount(strategyHeader);
            Assert.AreEqual(amount, inputSignal.Amount);

            // Для позиции созданы и отправлены брокеру защитные стоп и тейк профит приказы
            Assert.AreEqual(3, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(3, this.tradingData.Get<IEnumerable<Order>>().Count());
            Order slOrder = this.tradingData.Get<IEnumerable<Order>>().Single(o => o.OrderType == OrderType.Stop);
            Order tpOrder = this.tradingData.Get<IEnumerable<Order>>().Single(o => o.OrderType == OrderType.Limit);

            // Цена защитных приказов установлена соответственно настройкам
            Assert.AreEqual(149700, slOrder.Stop);
            Assert.AreEqual(150500, tpOrder.Price);

            // Брокер подтверждает только получение стоп приказа и отклоняет тейк профит приказ
            this.tradingData.Get<ObservableHashSet<OrderDeliveryConfirmation>>().Add(new OrderDeliveryConfirmation(slOrder, BrokerDateTime.Make(DateTime.Now)));
            this.tradingData.Get<ObservableHashSet<OrderDeliveryConfirmation>>().Add(new OrderDeliveryConfirmation(tpOrder, BrokerDateTime.Make(DateTime.Now)));
            this.tradingData.Get<ObservableHashSet<OrderRejection>>().Add(new OrderRejection(tpOrder, BrokerDateTime.Make(DateTime.Now), "заявка отклонена"));
            Assert.IsTrue(slOrder.IsDelivered);
            Assert.IsTrue(tpOrder.IsDelivered);
            Assert.IsTrue(tpOrder.IsRejected);

            // Тик не дошел до цены закрытия, поэтому сигнал экстренного закрытия по тейк профиту не срабатывает
            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick("RTS-9.13_FT", BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, 150490, 100));
            Assert.AreEqual(3, this.tradingData.Get<IEnumerable<Order>>().Count());

            // Тик дошел до цены закрытия, поэтому срабатывает сигнал экстренного закрытия по тейк профиту 
            this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick("RTS-9.13_FT", BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, 150500, 150));
            Assert.AreEqual(4, this.tradingData.Get<IEnumerable<Order>>().Count());

            // Извлекаем копию отправленной брокеру заявки чтобы убедиться в том, что отправлена нужная нам заявка
            Order etpOrder = this.tradingData.Get<IEnumerable<Order>>().Last();
            Assert.AreEqual(TradeAction.Sell, etpOrder.TradeAction);
            Assert.AreEqual(OrderType.Market, etpOrder.OrderType);
            Assert.AreEqual(amount, etpOrder.Amount);

            // Заявка экстренного закрытия исполняется одной сделкой
            Trade outputTrade = this.tradingData.AddSignalAndItsOrderAndTrade(etpOrder.Signal);

            // Приказ экстренного закрытия по тейк профиту исполнен
            Assert.IsTrue(etpOrder.IsFilled);

            // Стоп лосс приказ не исполнен 
            Assert.IsFalse(slOrder.IsFilled);

            // Система автоматически сгенерировала и отправила заявку на отмену стоп лосс приказа
            Assert.AreEqual(1, this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Count);
            OrderCancellationRequest slOrderCancel = this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Last();
            Assert.AreEqual(slOrder.Id, slOrderCancel.OrderId);

            // Брокер присылает подтверждение отмены стоп лосс приказа
            this.tradingData.Get<ObservableHashSet<OrderCancellationConfirmation>>().Add(new OrderCancellationConfirmation(slOrder, BrokerDateTime.Make(DateTime.Now), "Отмена приказа подтверждена"));
            Assert.IsTrue(slOrder.IsCanceled);

            // Позиция закрыта
            Assert.AreEqual(0, this.tradingData.GetAmount(strategyHeader));

        }
    }
}
