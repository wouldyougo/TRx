using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common.Collections;
using TRL.Common.Test.Mocks;
using TRL.Common.Data;

namespace TRL.Common.Test.Data
{
    public class BarsDataFactory : RawBaseNamedDataContext
    {
        public NamedObservableCollection<Bar> RtsBars { get; private set; }
        public NamedObservableCollection<Bar> SiBars { get; private set; }

        public BarsDataFactory()
        {
            this.RtsBars = new NamedObservableCollection<Bar>("RTS-12.13_FT");
            this.SiBars = new NamedObservableCollection<Bar>("Si-12.13_FT");
        }
    }

    [TestClass]
    public class NamedDataContextTests
    {
        [TestMethod]
        public void NamedDataContext_test()
        {
            INamedDataContext factory = new BarsDataFactory();

            IEnumerable<Bar> rts = factory.Get<NamedObservableCollection<Bar>>("RTS-12.13_FT");
            Assert.IsNotNull(rts);

            NamedObservableCollection<Bar> si = factory.Get<NamedObservableCollection<Bar>>("Si-12.13_FT");
            Assert.IsNotNull(si);

            NamedObservableCollection<Bar> gold = factory.Get<NamedObservableCollection<Bar>>("GOLD-12.13_FT");
            Assert.IsNull(gold);
        }
    }
}
