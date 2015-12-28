using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Connect.Smartcom.Data;
using TRL.Common.Data;
using SmartCOM3Lib;
using TRL.Connect.Smartcom.Events;
using TRL.Connect.Smartcom.Test.Mocks;
using TRL.Connect.Smartcom.Models;
using TRL.Logging;

namespace TRL.Connect.Smartcom.Test.Data
{
    [TestClass]
    public class RawTradingDataProviderTests
    {
        private RawTradingDataContext rawTradingDataContext;
        private IGenericSingleton<StServer> stServerSingleton;
        private SmartComHandlersDatabase handlers;
        private SmartComBinder binder;
        private RawTradingDataProvider provider;
        private StServerClassMock stServer;

        [TestInitialize]
        public void Setup()
        {
            this.rawTradingDataContext = new RawTradingDataContext();
            this.stServerSingleton = new StServerMockSingleton();
            this.handlers = new SmartComHandlersDatabase();
            this.binder = new SmartComBinder(this.stServerSingleton.Instance, this.handlers, new NullLogger());

            this.provider = new RawTradingDataProvider(this.handlers, this.rawTradingDataContext, new NullLogger());
            this.binder.Bind();

            this.stServer = (StServerClassMock)this.stServerSingleton.Instance;
        }

        [TestCleanup]
        public void Teardown()
        {
            this.stServerSingleton.Destroy();
        }


        [TestMethod]
        public void DataProvider_OrderFailed_Arrival()
        {
            Assert.AreEqual(0, this.rawTradingDataContext.GetData<OrderFailed>().Count);

            this.stServer.EmulateOrderFailedArrival(10, "023098", "Reject");
            Assert.AreEqual(1, this.rawTradingDataContext.GetData<OrderFailed>().Count);

            this.stServer.EmulateOrderFailedArrival(10, "023098", "Reject");
            Assert.AreEqual(2, this.rawTradingDataContext.GetData<OrderFailed>().Count);
            
            this.stServer.EmulateOrderFailedArrival(10, "023098", "Reject");
            Assert.AreEqual(3, this.rawTradingDataContext.GetData<OrderFailed>().Count);
        }

        [TestMethod]
        public void DataProvider_OrderSucceeded_Arrival()
        {
            Assert.AreEqual(0, this.rawTradingDataContext.GetData<OrderSucceeded>().Count);

            this.stServer.EmulateOrderSucceededArrival(10, "023098");
            Assert.AreEqual(1, this.rawTradingDataContext.GetData<OrderSucceeded>().Count);

            this.stServer.EmulateOrderSucceededArrival(10, "023098");
            Assert.AreEqual(2, this.rawTradingDataContext.GetData<OrderSucceeded>().Count);

            this.stServer.EmulateOrderSucceededArrival(10, "023098");
            Assert.AreEqual(3, this.rawTradingDataContext.GetData<OrderSucceeded>().Count);
        }

        [TestMethod]
        public void DataProvider_UpdateOrder_Arrival()
        {
            Assert.AreEqual(0, this.rawTradingDataContext.GetData<UpdateOrder>().Count);

            this.stServer.EmulateUpdateOrderArrival("PRTFL", "RTS-6.13", StOrder_State.StOrder_State_Filled, StOrder_Action.StOrder_Action_Buy, StOrder_Type.StOrder_Type_Market, StOrder_Validity.StOrder_Validity_Day, 150000, 10, 0, 1, DateTime.Now, "23", "2435", 1, 23);
            Assert.AreEqual(1, this.rawTradingDataContext.GetData<UpdateOrder>().Count);

            this.stServer.EmulateUpdateOrderArrival("PRTFL", "RTS-6.13", StOrder_State.StOrder_State_Open, StOrder_Action.StOrder_Action_Buy, StOrder_Type.StOrder_Type_Market, StOrder_Validity.StOrder_Validity_Day, 150000, 10, 0, 1, DateTime.Now, "23", "2435", 1, 23);
            Assert.AreEqual(2, this.rawTradingDataContext.GetData<UpdateOrder>().Count);

            this.stServer.EmulateUpdateOrderArrival("PRTFL", "RTS-6.13", StOrder_State.StOrder_State_ContragentReject, StOrder_Action.StOrder_Action_Buy, StOrder_Type.StOrder_Type_Market, StOrder_Validity.StOrder_Validity_Day, 150000, 10, 0, 1, DateTime.Now, "23", "2435", 1, 23);
            Assert.AreEqual(3, this.rawTradingDataContext.GetData<UpdateOrder>().Count);
        }

        [TestMethod]
        public void DataProvider_SetPortfolio_Arrival()
        {
            Assert.AreEqual(0, this.rawTradingDataContext.GetData<SetPortfolio>().Count);

            this.stServer.EmulateSetPortfolioArrival("PRTFL", 55000, 1, 0, 65000);
            Assert.AreEqual(1, this.rawTradingDataContext.GetData<SetPortfolio>().Count);

            this.stServer.EmulateSetPortfolioArrival("PRTFL", 55000, 1, 0, 65500);
            Assert.AreEqual(2, this.rawTradingDataContext.GetData<SetPortfolio>().Count);

            this.stServer.EmulateSetPortfolioArrival("PRTFL", 55000, 1, 0, 65800);
            Assert.AreEqual(3, this.rawTradingDataContext.GetData<SetPortfolio>().Count);
        }

        [TestMethod]
        public void DataProvider_Trade_Arrival()
        {
            Assert.AreEqual(0, this.rawTradingDataContext.GetData<TradeInfo>().Count);

            this.stServer.EmulateTradeArrival("PRTFL", "RTS-6.13_FT", "234234", 55000, 1, DateTime.Now, "345345");
            Assert.AreEqual(1, this.rawTradingDataContext.GetData<TradeInfo>().Count);

            this.stServer.EmulateTradeArrival("PRTFL", "RTS-6.13_FT", "234234", 55000, 1, DateTime.Now, "345345");
            Assert.AreEqual(2, this.rawTradingDataContext.GetData<TradeInfo>().Count);

            this.stServer.EmulateTradeArrival("PRTFL", "RTS-6.13_FT", "234234", 55000, 1, DateTime.Now, "345345");
            Assert.AreEqual(3, this.rawTradingDataContext.GetData<TradeInfo>().Count);
        }
        
        [TestMethod]
        public void DataProvider_RawSymbol_Arrival()
        {
            Assert.AreEqual(0, this.rawTradingDataContext.GetData<RawSymbol>().Count);

            this.stServer.EmulateSymbolArrival(1, 10, "Symbol", "short name", "Long name", "type", 1, 1, 1, 1, "SEC", "EXCH", DateTime.Now, 1, 3);
            Assert.AreEqual(1, this.rawTradingDataContext.GetData<RawSymbol>().Count);

            this.stServer.EmulateSymbolArrival(1, 10, "Symbol", "short name", "Long name", "type", 1, 1, 1, 1, "SEC", "EXCH", DateTime.Now, 1, 3);
            Assert.AreEqual(2, this.rawTradingDataContext.GetData<RawSymbol>().Count);

            this.stServer.EmulateSymbolArrival(1, 10, "Symbol", "short name", "Long name", "type", 1, 1, 1, 1, "SEC", "EXCH", DateTime.Now, 1, 3);
            Assert.AreEqual(3, this.rawTradingDataContext.GetData<RawSymbol>().Count);
        }
    }
}
