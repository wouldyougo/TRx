using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using TRL.Common;
using TRL.Indicators;
using TRL.Common.TimeHelpers;

namespace TRL.Indicators.Test
{
    [TestClass]
    public class AdrTests
    {
        [TestMethod]
        public void Indicators_Adr_equals_to_one()
        {
            List<Bar> bars = new List<Bar>();
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 130, 135, 129, 133, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 131, 136, 130, 134, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 132, 137, 131, 135, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 133, 138, 132, 136, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 134, 139, 133, 137, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 133, 131, 128, 130, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 132, 132, 127, 129, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 131, 130, 126, 128, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 130, 129, 125, 127, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 129, 128, 124, 126, 10));

            double adrValue = Adr.Make(bars, 10);

            Assert.AreEqual(1, adrValue);
        }

        [TestMethod]
        public void Indicators_Adr_make_value_for_ten_bars()
        {
            List<Bar> bars = new List<Bar>();
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 129, 134, 128, 132, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 130, 135, 129, 133, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 131, 136, 130, 134, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 132, 137, 131, 135, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 133, 138, 132, 136, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 134, 139, 133, 137, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 133, 131, 128, 130, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 132, 132, 127, 129, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 131, 130, 126, 128, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 130, 129, 125, 127, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 129, 128, 124, 126, 10));

            double adrValue = Adr.Make(bars, 10);

            Assert.AreEqual(1, adrValue);
        }

        [TestMethod]
        public void Indicators_Adr_equals_three()
        {
            List<Bar> bars = new List<Bar>();
            
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 129, 134, 128, 132, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 130, 135, 129, 133, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 131, 136, 130, 134, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 132, 137, 131, 135, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 133, 138, 132, 136, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 134, 139, 133, 137, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 133, 131, 128, 130, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 132, 132, 127, 129, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 131, 133, 129, 132, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 132, 134, 130, 133, 10));
            bars.Add(new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 129, 130, 129, 130, 10));

            double adrValue = Adr.Make(bars, 8);

            Assert.AreEqual(3, adrValue);
        }

    }
}
