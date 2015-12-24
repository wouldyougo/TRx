using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;
using System.Globalization;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class TickTests
    {

        [TestMethod]
        public void Tick_constructor_test()
        {
            DateTime tickDate = BrokerDateTime.Make(DateTime.Now);

            Tick tick = new Tick("RTS-9.13_FT", tickDate, TradeAction.Buy, 150000, 3);

            Assert.AreEqual("RTS-9.13_FT", tick.Symbol);
            Assert.AreEqual(tickDate, tick.DateTime);
            Assert.AreEqual(TradeAction.Buy, tick.TradeAction);
            Assert.AreEqual(150000, tick.Price);
            Assert.AreEqual(3, tick.Volume);
        }

        [TestMethod]
        public void Tick_ToString()
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            Tick tick = new Tick { Symbol = "RTS-3.13_FT", DateTime = new DateTime(2012, 12, 12), Price = 140000, Volume = 1, TradeAction = TradeAction.Buy };

            string result = String.Format("Symbol: {0}, DateTime: {1}, Price: {2}, Volume: {3}, TradeAction: {4}",
                tick.Symbol, tick.DateTime.ToString(ci), tick.Price.ToString("0.0000", ci), tick.Volume.ToString("0.0000", ci), tick.TradeAction);

            Assert.AreEqual(result, tick.ToString());
        }

        [TestMethod]
        public void Tick_ToImportString()
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            Tick tick = new Tick { Symbol = "RTS-3.13_FT", DateTime = new DateTime(2012, 12, 12), Price = 140000, Volume = 1, TradeAction = TradeAction.Buy };

            string result = String.Format("{0},{1},{2},{3},{4}",
                tick.Symbol, tick.DateTime.ToString(ci), tick.Price.ToString("0.0000", ci), tick.Volume.ToString("0.0000", ci), tick.TradeAction);

            Assert.AreEqual(result, tick.ToImportString());
        }

        [TestMethod]
        public void Tick_Parse()
        {
            //Tick tick = Tick.Parse("20121230, 100000, RTS-3.13_FT, 145000, 1, 0");
            Tick tick = Tick.Parse("RTS-3.13_FT, 0, 20121230, 100000, 145000, 1");

            Assert.AreEqual(new DateTime(2012, 12, 30, 10, 0, 0), tick.DateTime);
            Assert.AreEqual("RTS-3.13_FT", tick.Symbol);
            Assert.AreEqual(145000, tick.Price);
            Assert.AreEqual(1, tick.Volume);
            Assert.AreEqual(TradeAction.Buy, tick.TradeAction);
        }

        [TestMethod]
        public void Tick_Try_Parse_finam_string()
        {
            string src = "20130802,100001,9826.000000000,10";

            Tick tick = Tick.Parse(src);

            Assert.AreEqual(new DateTime(2013, 8, 2, 10, 0, 1), tick.DateTime);
            Assert.AreEqual(string.Empty, tick.Symbol);
            Assert.AreEqual(9826, tick.Price);
            Assert.AreEqual(10, tick.Volume);
            Assert.AreEqual(TradeAction.Buy, tick.TradeAction);

        }

        [TestMethod]
        public void Tick_parse_my_own_export_string_test()
        {
            string tickString = "RTS-6.14,01/01/2014 00:00:00,10.0000,1.0000,Sell";

            Tick tick = Tick.Parse(tickString);

            Assert.AreEqual(new DateTime(2014, 1, 1, 0, 0, 0), tick.DateTime);
            Assert.AreEqual("RTS-6.14", tick.Symbol);
            Assert.AreEqual(10, tick.Price);
            Assert.AreEqual(1, tick.Volume);
            Assert.AreEqual(TradeAction.Sell, tick.TradeAction);
        }
    }
}
