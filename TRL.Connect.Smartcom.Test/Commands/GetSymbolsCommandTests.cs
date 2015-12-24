using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Connect.Smartcom.Test.Mocks;
using TRL.Connect.Smartcom.Data;
using TRL.Connect.Smartcom.Commands;
using TRL.Transaction;
using TRL.Logging;

namespace TRL.Connect.Smartcom.Test.Data
{
    [TestClass]
    public class GetSymbolsCommandTests
    {
        private StServerMockSingleton singleton;
        private StServerClassMock stServer;

        [TestInitialize]
        public void Setup()
        {
            this.singleton = new StServerMockSingleton();
            this.stServer = (StServerClassMock)this.singleton.Instance;
        }

        [TestMethod]
        public void Execute_GetSymbolsCommand()
        {
            ITransaction transaction = new GetSymbolsCommand(this.singleton, new NullLogger());
            transaction.Execute();
            Assert.IsTrue(this.stServer.GetSymbolsExecuted);
        }
    }
}
