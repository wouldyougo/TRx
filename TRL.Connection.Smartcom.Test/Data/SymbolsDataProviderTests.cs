using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Connect.Smartcom.Test.Mocks;
using SmartCOM3Lib;
using TRL.Connect.Smartcom.Data;
using TRL.Connect.Smartcom.Events;
using TRL.Logging;

namespace TRL.Connect.Smartcom.Test.Data
{
    [TestClass]
    public class SymbolsDataProviderTests
    {
        private SymbolDataContext symbolsDataContext;
        private IGenericSingleton<StServer> stServerSingleton;
        private SmartComHandlersDatabase handlers;
        private SmartComBinder binder;
        private SymbolsDataProvider provider;
        private StServerClassMock stServer;

        [TestInitialize]
        public void Setup()
        {
            this.symbolsDataContext = new SymbolDataContext();
            this.stServerSingleton = new StServerMockSingleton();
            this.handlers = new SmartComHandlersDatabase();
            this.binder = new SmartComBinder(this.stServerSingleton.Instance, this.handlers, new NullLogger());

            this.provider = new SymbolsDataProvider(this.handlers, this.symbolsDataContext, new NullLogger());
            this.binder.Bind();

            this.stServer = (StServerClassMock)this.stServerSingleton.Instance;
        }

        [TestCleanup]
        public void Teardown()
        {
            this.stServerSingleton.Destroy();
        }


        [TestMethod]
        public void SymbolsDataProvider_new_SymbolSettings_Arrived()
        {
            this.binder.Bind();
            Assert.AreEqual(0, this.symbolsDataContext.Get<IEnumerable<SymbolSettings>>().Count());

            this.stServer.EmulateSymbolSettingsArrival(1,
                3000,
                "RTS-12.13_FT",
                "RIZ3",
                "Фьючерсный контракт на Индекс РТС",
                "Фьючерс",
                5,
                1,
                6.37572,
                10,
                "isin",
                "exchg",
                new DateTime(2013, 12, 16),
                35,
                149020);

            Assert.AreEqual(1, this.symbolsDataContext.Get<IEnumerable<SymbolSettings>>().Count());

            this.binder.Unbind();
        }

        [TestMethod]
        public void SymbolsDataProvider_SymbolSettings_update_Arrived()
        {
            this.binder.Bind();
            Assert.AreEqual(0, this.symbolsDataContext.Get<IEnumerable<SymbolSettings>>().Count());

            this.stServer.EmulateSymbolSettingsArrival(1,
                3000,
                "RTS-12.13_FT",
                "RIZ3",
                "Фьючерсный контракт на Индекс РТС",
                "Фьючерс",
                5,
                1,
                6.37572,
                10,
                "isin",
                "exchg",
                new DateTime(2013, 12, 16),
                35,
                149020);

            Assert.AreEqual(1, this.symbolsDataContext.Get<IEnumerable<SymbolSettings>>().Count());

            this.stServer.EmulateSymbolSettingsArrival(1,
                3000,
                "RTS-12.13_FT",
                "RIZ3",
                "Фьючерсный контракт на Индекс РТС",
                "Фьючерс",
                5,
                1,
                6.37578,
                10,
                "isin",
                "exchg",
                new DateTime(2013, 12, 16),
                34,
                149020);

            Assert.AreEqual(1, this.symbolsDataContext.Get<IEnumerable<SymbolSettings>>().Count());

            SymbolSettings item = this.symbolsDataContext.Get<IEnumerable<SymbolSettings>>().First();
            Assert.AreEqual("RTS-12.13_FT", item.Name);
            Assert.AreEqual(6.37578, item.MinStepPrice);

            this.binder.Unbind();
        }

        [TestMethod]
        public void SymbolsDataProvider_new_SymbolSummary_Arrived()
        {
            this.binder.Bind();
            Assert.AreEqual(0, this.symbolsDataContext.Get<IEnumerable<SymbolSummary>>().Count());

            this.stServer.EmulateUpdateQuoteArrival(
                "RTS-12.13_FT",
                new DateTime(2013, 12, 16),
                145000,
                146000,
                144000,
                144500,
                145500,
                300,
                400,
                145510,
                145520,
                100,
                200,
                500,
                600,
                700,
                800,
                900,
                150000,
                140000,
                1,
                1000,
                143000);

            Assert.AreEqual(1, this.symbolsDataContext.Get<IEnumerable<SymbolSummary>>().Count());

            this.binder.Unbind();
        }

        [TestMethod]
        public void SymbolsDataProvider_SymbolSummary_update_Arrived()
        {
            this.binder.Bind();
            Assert.AreEqual(0, this.symbolsDataContext.Get<IEnumerable<SymbolSummary>>().Count());

            this.stServer.EmulateUpdateQuoteArrival(
                "RTS-12.13_FT",
                new DateTime(2013, 12, 16),
                145000,
                146000,
                144000,
                144500,
                145500,
                300,
                400,
                145510,
                145520,
                100,
                200,
                500,
                600,
                700,
                800,
                900,
                150000,
                140000,
                1,
                1000,
                143000);

            Assert.AreEqual(1, this.symbolsDataContext.Get<IEnumerable<SymbolSummary>>().Count());

            this.stServer.EmulateUpdateQuoteArrival(
                "RTS-12.13_FT",
                new DateTime(2013, 12, 16),
                145000,
                146000,
                144000,
                144500,
                145590,
                300,
                400,
                145510,
                145520,
                100,
                200,
                500,
                600,
                700,
                800,
                900,
                150000,
                140000,
                1,
                1000,
                143000);

            Assert.AreEqual(1, this.symbolsDataContext.Get<IEnumerable<SymbolSummary>>().Count());

            SymbolSummary item = this.symbolsDataContext.Get<IEnumerable<SymbolSummary>>().First();
            Assert.AreEqual("RTS-12.13_FT", item.Name);
            Assert.AreEqual(145590, item.LastDealPrice);
            this.binder.Unbind();
        }

    }
}
