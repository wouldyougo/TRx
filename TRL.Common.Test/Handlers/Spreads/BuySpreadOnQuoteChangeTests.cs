using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Collections;
using TRL.Common.Models;
using TRL.Common.Data;
using TRL.Common.Test.Mocks;
using TRL.Common.TimeHelpers;
using TRL.Common.Handlers;
using TRL.Handlers.Spreads;
using TRL.Emulation;
using TRL.Logging;

namespace TRL.Common.Handlers.Test.Spreads
{
    [TestClass]
    public class BuySpreadOnQuoteChangeTests:TraderBaseInitializer
    {
        private OrderBookContext qProvider;
        private SpreadSettings spreadSettings;
        private List<StrategyHeader> leftLeg, rightLeg;

        [TestInitialize]
        public void Setup()
        {

            this.qProvider = new OrderBookContext();
            this.spreadSettings = new SpreadSettings(1.35, 1.48, 1.18);
            
            this.leftLeg = new List<StrategyHeader>();
            this.leftLeg.Add(this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 1));

            this.rightLeg = new List<StrategyHeader>();
            this.rightLeg.Add(this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 2));
            this.rightLeg.Add(this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 3));

            BuySpreadOnQuoteChange handler =
                new BuySpreadOnQuoteChange(this.qProvider,
                    this.leftLeg,
                    this.rightLeg,
                    this.spreadSettings,
                    this.tradingData,
                    this.signalQueue,
                    new NullLogger());
        }

        [TestMethod]
        public void BuySpreadOnQuote_ignore_updates_when_any_position_from_basket_of_strategies_exists()
        {
            StrategyHeader strtgy = this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == 1);

            Signal sgnl = new Signal(strtgy, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, 143000, 0, 0);
            this.tradingData.Get<ICollection<Signal>>().Add(sgnl);

            Order ordr = new Order(sgnl);
            this.tradingData.Get<ICollection<Order>>().Add(ordr);

            OrderDeliveryConfirmation cnfrmtn = new OrderDeliveryConfirmation(ordr, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<OrderDeliveryConfirmation>>().Add(cnfrmtn);
            Assert.IsTrue(ordr.IsDelivered);

            Trade trd = new Trade(ordr, ordr.Portfolio, ordr.Symbol, 1430000, ordr.Amount, BrokerDateTime.Make(DateTime.Now));
            this.tradingData.Get<ObservableHashSet<Trade>>().Add(trd);
            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Position>>().Count());

            this.qProvider.Update(0, "RTS-12.13_FT", 128990, 20, 129000, 100);
            this.qProvider.Update(0, "Si-12.13_FT", 33000, 50, 33001, 40);
            this.qProvider.Update(0, "Eu-12.13_FT", 44000, 30, 44001, 0);

            Assert.AreEqual(1, this.tradingData.Get<IEnumerable<Signal>>().Count());
        }

        [TestMethod]
        public void BuySpreadOnQuote_ignore_updates_when_no_some_of_symbol_quotes_in_storage()
        {
            this.qProvider.Update(0, "RTS-12.13_FT", 143000, 100, 0, 0);
            this.qProvider.Update(0, "Si-12.13_FT", 31000, 50, 0, 0);
            this.qProvider.Update(0, "Eu-12.13_FT", 41000, 30, 0, 0);

            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());
        }

        [TestMethod]
        public void BuySpreadOnQuote_makes_buy_at_market_signals()
        {
            Assert.AreEqual(0, this.tradingData.Get<IEnumerable<Signal>>().Count());

            this.qProvider.Update(0, "RTS-12.13_FT", 0, 0, 129000, 100);
            this.qProvider.Update(0, "Si-12.13_FT", 33000, 50, 0, 0);
            this.qProvider.Update(0, "Eu-12.13_FT", 44000, 30, 0, 0);

            Assert.AreEqual(3, this.tradingData.Get<IEnumerable<Signal>>().Count());
            Assert.AreEqual(3, this.tradingData.Get<IEnumerable<Order>>().Count());
        }
    }
}
