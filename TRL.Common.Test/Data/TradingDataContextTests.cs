using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.Collections;
using TRL.Configuration;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Test.Data
{
    [TestClass]
    public class TradingDataContextTests
    {
        private IDataContext context;

        [TestInitialize]
        public void Setup()
        {
            this.context = new TradingDataContext();
        }

        [TestMethod]
        public void TradingDataContext_Is_ObservableHashSetFactory()
        {
            Assert.IsInstanceOfType(this.context, typeof(IObservableHashSetFactory));
        }

        [TestMethod]
        public void TradingDataContext_contains_collection_of_positions()
        {
            Assert.IsNotNull(this.context.Get<IEnumerable<Position>>());
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<Position>>(), typeof(ObservableHashSet<Position>));
        }

        [TestMethod]
        public void TradingDataContext_contains_collection_of_strategies()
        {
            Assert.IsNotNull(this.context.Get<IEnumerable<StrategyHeader>>());
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<StrategyHeader>>(), typeof(ObservableHashSet<StrategyHeader>));
        }

        [TestMethod]
        public void TradingDataContext_contains_collection_of_Signals()
        {
            Assert.IsNotNull(this.context.Get<IEnumerable<Signal>>());
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<Signal>>(), typeof(ObservableHashSet<Signal>));
        }

        [TestMethod]
        public void TradingDataContext_contains_collection_of_Orders()
        {
            Assert.IsNotNull(this.context.Get<IEnumerable<Order>>());
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<Order>>(), typeof(ObservableHashSet<Order>));
        }

        [TestMethod]
        public void TradingDataContext_contains_collection_of_Trades()
        {
            Assert.IsNotNull(this.context.Get<IEnumerable<Trade>>());
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<Trade>>(), typeof(ObservableHashSet<Trade>));
        }

        [TestMethod]
        public void TradingDataContext_contains_collection_of_OrderCancellationRequests()
        {
            Assert.IsNotNull(this.context.Get<IEnumerable<OrderCancellationRequest>>());
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<OrderCancellationRequest>>(), typeof(ObservableHashSet<OrderCancellationRequest>));
        }

        [TestMethod]
        public void TradingDataContext_contains_collection_of_OrderCancellationConfirmation()
        {
            Assert.IsNotNull(this.context.Get<IEnumerable<OrderCancellationConfirmation>>());
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<OrderCancellationConfirmation>>(), typeof(ObservableHashSet<OrderCancellationConfirmation>));
        }

        [TestMethod]
        public void TradingDataContext_contains_collection_of_OrderRejection()
        {
            Assert.IsNotNull(this.context.Get<IEnumerable<OrderRejection>>());
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<OrderRejection>>(), typeof(ObservableHashSet<OrderRejection>));
        }

        [TestMethod]
        public void TradingData_is_singleton()
        {
            TradingData td = TradingData.Instance;
            TradingData td2 = TradingData.Instance;

            Assert.IsNotNull(td);
            Assert.IsNotNull(td2);
            Assert.AreSame(td, td2);
        }

        [TestMethod]
        public void TradingData_is_ObservableHashSetFactory()
        {
            Assert.IsTrue(TradingData.Instance is IObservableHashSetFactory);
        }

        [TestMethod]
        public void TradingData_is_TradingDataContext()
        {
            Assert.IsTrue(TradingData.Instance is TradingDataContext);
        }

        [TestMethod]
        public void TradingData_contains_OrderSettings()
        {
            Assert.IsNotNull(this.context.Get<IEnumerable<OrderSettings>>());
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<OrderSettings>>(), typeof(ObservableHashSet<OrderSettings>));
        }

        [TestMethod]
        public void TradingData_contains_OrderCancellationFailedNotification()
        {
            Assert.IsNotNull(this.context.Get<IEnumerable<OrderCancellationFailedNotification>>());
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<OrderCancellationFailedNotification>>(), typeof(ObservableHashSet<OrderCancellationFailedNotification>));
        }

        [TestMethod]
        public void TradingData_contains_BarSettings()
        {
            Assert.IsNotNull(this.context.Get<IEnumerable<BarSettings>>());
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<BarSettings>>(), typeof(ObservableHashSet<BarSettings>));
        }

        [TestMethod]
        public void TradingData_contains_StopLossSettings()
        {
            Assert.IsNotNull(this.context.Get<IEnumerable<StopPointsSettings>>());
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<StopPointsSettings>>(), typeof(ObservableHashSet<StopPointsSettings>));
        }

        [TestMethod]
        public void TradingData_contains_OrderDeliveryConfirmations()
        {
            Assert.IsNotNull(this.context.Get<IEnumerable<OrderDeliveryConfirmation>>());
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<OrderDeliveryConfirmation>>(), typeof(ObservableHashSet<OrderDeliveryConfirmation>));
        }

        [TestMethod]
        public void TradingData_contains_TakeProfitSettings()
        {
            Assert.IsNotNull(this.context.Get<IEnumerable<ProfitPointsSettings>>());
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<ProfitPointsSettings>>(), typeof(ObservableHashSet<ProfitPointsSettings>));
        }

        [TestMethod]
        public void TradingData_contains_SMASettings()
        {
            Assert.IsNotNull(this.context.Get<IEnumerable<SMASettings>>());
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<SMASettings>>(), typeof(ObservableHashSet<SMASettings>));
        }

        [TestMethod]
        public void TradingData_contains_StopLossOrderSettings()
        {
            Assert.IsNotNull(this.context.Get<IEnumerable<StopLossOrderSettings>>());
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<StopLossOrderSettings>>(), typeof(ObservableHashSet<StopLossOrderSettings>));
        }

        [TestMethod]
        public void TradingData_contains_TakeProfitOrderSettings()
        {
            Assert.IsNotNull(this.context.Get<IEnumerable<TakeProfitOrderSettings>>());
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<TakeProfitOrderSettings>>(), typeof(ObservableHashSet<TakeProfitOrderSettings>));
        }

        [TestMethod]
        public void TradingData_contains_OpenOrders()
        {
            Assert.IsNotNull(this.context.Get<IEnumerable<OpenOrder>>());
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<OpenOrder>>(), typeof(ObservableHashSet<OpenOrder>));
        }

        [TestMethod]
        public void TradingData_contains_CloseOrders()
        {
            Assert.IsNotNull(this.context.Get<IEnumerable<CloseOrder>>());
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<CloseOrder>>(), typeof(ObservableHashSet<CloseOrder>));
        }

        [TestMethod]
        public void TradingData_contains_PositionSettings()
        {
            Assert.IsNotNull(this.context.Get<IEnumerable<PositionSettings>>());
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<PositionSettings>>(), typeof(ObservableHashSet<PositionSettings>));
        }

        [TestMethod]
        public void TraidingDataContext_contains_collection_of_Symbols_test()
        {
            Assert.IsNotNull(this.context.Get<HashSetOfNamedMutable<Symbol>>());
            Assert.IsNotNull(this.context.Get<IEnumerable<Symbol>>());
        }

        [TestMethod]
        public void TradingData_contains_Trend()
        {
            Assert.IsNotNull(this.context.Get<IEnumerable<Trend>>());
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<Trend>>(), typeof(ObservableHashSet<Trend>));
        }

        [TestMethod]
        public void TradingData_contains_Ticks()
        {
            Assert.IsNotNull(this.context.Get<ObservableCollection<Tick>>());
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<Tick>>(), typeof(ObservableCollection<Tick>));
        }

        [TestMethod]
        public void TradingData_contains_Bars()
        {
            Assert.IsNotNull(this.context.Get<ObservableCollection<Bar>>());
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<Bar>>(), typeof(ObservableCollection<Bar>));
        }

        [TestMethod]
        public void TradingData_contains_OrderMoveRequest_test()
        {
            Assert.IsNotNull(this.context.Get<IEnumerable<OrderMoveRequest>>());
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<OrderMoveRequest>>(), typeof(ObservableCollection<OrderMoveRequest>));
        }

        [TestMethod]
        public void TradingData_contains_strategyVolumeChangeStep_hashset()
        {
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<StrategyVolumeChangeStep>>(), typeof(HashSet<StrategyVolumeChangeStep>));
        }

        [TestMethod]
        public void TradingData_contains_SpreadValue_collection_test()
        {
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<SpreadValue>>(), typeof(ObservableCollection<SpreadValue>));
        }

        [TestMethod]
        public void TradingData_contains_ArbitrageSettings_hashset_test()
        {
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<ArbitrageSettings>>(), typeof(ObservableHashSet<ArbitrageSettings>));
        }

        [TestMethod]
        public void TradingData_contains_BidAsk_collection_test()
        {
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<BidAsk>>(), typeof(ObservableCollection<BidAsk>));
        }

        [TestMethod]
        public void TradingData_contains_MoveOrder_collection_test()
        {
            Assert.IsInstanceOfType(this.context.Get<IEnumerable<MoveOrder>>(), typeof(ObservableHashSet<MoveOrder>));
        }

    }
}
