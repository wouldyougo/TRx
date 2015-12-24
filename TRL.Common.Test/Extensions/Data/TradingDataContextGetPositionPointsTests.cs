using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.Extensions.Data;
using TRL.Emulation;

namespace TRL.Common.Extensions.Data.Test
{
    [TestClass]
    public class TradingDataContextGetPositionPointsTests
    {
        private IDataContext tradingData;
        private StrategyHeader strategyHeader;

        [TestInitialize]
        public void Setup()
        {
            this.tradingData = new TradingDataContext();
            this.strategyHeader = new StrategyHeader(1, "Пробойная стратегия", "BP12345-RF-01", "RTS-3.14_FT", 10);
            this.tradingData.Get<ICollection<StrategyHeader>>().Add(this.strategyHeader);
        }

        [TestMethod]
        public void TradingDataContextExtensions_GetPositionPoints_for_single_trade_long_position_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 145000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            Assert.AreEqual(1450000, this.tradingData.GetPositionPoints(this.strategyHeader));
        }

        [TestMethod]
        public void TradingDataContextExtensions_GetPositionPoints_for_multiple_trade_long_position_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 145000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 145000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 145000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 145000, 4);

            Assert.AreEqual(1450000, this.tradingData.GetPositionPoints(this.strategyHeader));
        }

        [TestMethod]
        public void TradingDataContextExtensions_GetPositionPoints_for_multiple_trade_with_different_prices_long_position_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 145000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 145000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 146000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 147000, 4);

            Assert.AreEqual(1461000, this.tradingData.GetPositionPoints(this.strategyHeader));
        }

        [TestMethod]
        public void TradingDataContextExtensions_GetPositionPoints_for_single_trade_next_long_position_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 145000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            Signal closeSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 146000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(closeSignal);

            Signal buySignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 147000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(buySignal);

            Assert.AreEqual(1470000, this.tradingData.GetPositionPoints(this.strategyHeader));
        }

        [TestMethod]
        public void TradingDataContextExtensions_GetPositionPoints_for_multiple_trade_next_long_position_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 145000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 145000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 145000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 145000, 4);

            Signal closeSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 146000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(closeSignal);

            Signal buySignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 147000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(buySignal, 147000, 5);
            this.tradingData.AddSignalAndItsOrderAndTrade(buySignal, 147000, 4);
            this.tradingData.AddSignalAndItsOrderAndTrade(buySignal, 147000, 1);

            Assert.AreEqual(1470000, this.tradingData.GetPositionPoints(this.strategyHeader));
        }

        [TestMethod]
        public void TradingDataContextExtensions_GetPositionPoints_for_multiple_trade_with_different_prices_next_long_position_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 145000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 145000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 146000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 147000, 4);

            Signal closeSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 146000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(closeSignal);

            Signal buySignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 147000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 148000, 5);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 149000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 150000, 2);

            Assert.AreEqual(1487000, this.tradingData.GetPositionPoints(this.strategyHeader));
        }

        [TestMethod]
        public void TradingDataContextExtensions_GetPositionPoints_for_multiple_trade_with_different_prices_next_non_strategy_amount_long_position_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 145000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 145000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 146000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 147000, 4);

            Signal closeSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 146000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(closeSignal);

            Signal buySignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 147000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(buySignal, 148000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(buySignal, 149000, 2);
            this.tradingData.AddSignalAndItsOrderAndTrade(buySignal, 150000, 1);

            Assert.AreEqual(892000, this.tradingData.GetPositionPoints(this.strategyHeader));
        }

        [TestMethod]
        public void TradingDataContextExtensions_GetPositionPoints_for_multiple_trade_with_different_prices_non_strategy_amount_long_position_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 145000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 145000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 146000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 147000, 4);

            Signal closeSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 146000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(closeSignal, 146000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(closeSignal, 146000, 2);

            Signal buySignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 147000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(buySignal, 148000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(buySignal, 149000, 2);
            this.tradingData.AddSignalAndItsOrderAndTrade(buySignal, 150000, 1);

            Assert.AreEqual(1626000, this.tradingData.GetPositionPoints(this.strategyHeader));
        }

        [TestMethod]
        public void TradingDataContextExtensions_GetPositionPoints_for_single_trade_short_position_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 145000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            Assert.AreEqual(-1450000, this.tradingData.GetPositionPoints(this.strategyHeader));
        }

        [TestMethod]
        public void TradingDataContextExtensions_GetPositionPoints_for_multiple_trade_short_position_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 145000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 145000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 145000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 145000, 4);

            Assert.AreEqual(-1450000, this.tradingData.GetPositionPoints(this.strategyHeader));
        }

        [TestMethod]
        public void TradingDataContextExtensions_GetPositionPoints_for_multiple_trade_with_different_prices_short_position_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 145000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 145000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 146000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 147000, 4);

            Assert.AreEqual(-1461000, this.tradingData.GetPositionPoints(this.strategyHeader));
        }

        [TestMethod]
        public void TradingDataContextExtensions_GetPositionPoints_for_single_trade_next_short_position_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 145000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal);

            Signal closeSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 146000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(closeSignal);

            Signal buySignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 147000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(buySignal);

            Assert.AreEqual(-1470000, this.tradingData.GetPositionPoints(this.strategyHeader));
        }

        [TestMethod]
        public void TradingDataContextExtensions_GetPositionPoints_for_multiple_trade_next_sell_position_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 145000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 145000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 145000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 145000, 4);

            Signal closeSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 146000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(closeSignal);

            Signal buySignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 147000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(buySignal, 147000, 5);
            this.tradingData.AddSignalAndItsOrderAndTrade(buySignal, 147000, 4);
            this.tradingData.AddSignalAndItsOrderAndTrade(buySignal, 147000, 1);

            Assert.AreEqual(-1470000, this.tradingData.GetPositionPoints(this.strategyHeader));
        }

        [TestMethod]
        public void TradingDataContextExtensions_GetPositionPoints_for_multiple_trade_with_different_prices_next_short_position_test()
        {
            Signal openSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 145000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 145000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 146000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 147000, 4);

            Signal closeSignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Buy, OrderType.Market, 146000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(closeSignal);

            Signal buySignal = new Signal(this.strategyHeader, DateTime.Now, TradeAction.Sell, OrderType.Market, 147000, 0, 0);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 148000, 5);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 149000, 3);
            this.tradingData.AddSignalAndItsOrderAndTrade(openSignal, 150000, 2);

            Assert.AreEqual(-1487000, this.tradingData.GetPositionPoints(this.strategyHeader));
        }
    }
}
