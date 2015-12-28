using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Connect.Smartcom.Test.Mocks;
using TRL.Configuration;
using TRL.Common.Data;
using TRL.Connect.Smartcom.Commands;
using TRL.Transaction;
using TRL.Logging;
using TRL.Common;

namespace TRL.Connect.Smartcom.Test.Commands
{
    [TestClass]
    public class GetBarsCommandTests
    {
        private StServerMockSingleton serverSingleton;
        private StServerClassMock stServer;

        [TestInitialize]
        public void Setup()
        {
            this.serverSingleton = new StServerMockSingleton();
            this.stServer = (StServerClassMock)this.serverSingleton.Instance;
        }

        [TestMethod]
        public void GetBarsCommand_Test()
        {
            Assert.AreEqual(0, this.stServer.BarsReceived);

            int barIntervalSeconds = AppSettings.GetValue<int>("BarIntervalSeconds");
            int barQuantity = AppSettings.GetValue<int>("BarQuantity");

            ITransaction transaction = new GetBarsCommand("RTS-12.13_FT", 60, 19, this.serverSingleton, new NullLogger());

            transaction.Execute();

            Assert.AreEqual(barQuantity, this.stServer.BarsReceived);

        }
    }
}
