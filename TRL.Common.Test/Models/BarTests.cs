using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;
//using TRL.Common.Extensions.Models;     

namespace TRL.Common.Models.tests
{
    [TestClass]
    public class BarTests
    {
        [TestMethod]
        public void Bar_constructor_test()
        {
            Bar bar = new Bar("RTS-6.13_FT", 3600, new DateTime(2013, 5, 10, 10, 0, 0), 150000, 153000, 148000, 152500, 55000);

            Assert.AreEqual("RTS-6.13_FT", bar.Symbol);
            Assert.AreEqual(3600, bar.Interval);
            Assert.AreEqual(bar.DateTime, new DateTime(2013, 5, 10, 10, 0, 0));
            Assert.AreEqual(150000, bar.Open);
            Assert.AreEqual(153000, bar.High);
            Assert.AreEqual(148000, bar.Low);
            Assert.AreEqual(152500, bar.Close);
            Assert.AreEqual(55000, bar.Volume);
        }

        [TestMethod]
        public void Bar_one_more_constructor_test()
        {
            Bar bar = new Bar(new DateTime(2013, 5, 10, 10, 0, 0), 150000, 153000, 148000, 152500, 55000);

            Assert.AreEqual(String.Empty, bar.Symbol);
            Assert.AreEqual(0, bar.Interval);
            Assert.AreEqual(bar.DateTime, new DateTime(2013, 5, 10, 10, 0, 0));
            Assert.AreEqual(150000, bar.Open);
            Assert.AreEqual(153000, bar.High);
            Assert.AreEqual(148000, bar.Low);
            Assert.AreEqual(152500, bar.Close);
            Assert.AreEqual(55000, bar.Volume);
        }

        [TestMethod]
        public void Bar_Parse_Without_Symbol()
        {
            string barString = "20111003,100000,129225.00000,129225.00000,125270.00000,126495.00000,195580";

            Bar bar = Bar.Parse(barString);

            Assert.AreEqual(bar.DateTime, new DateTime(2011, 10, 3, 10, 0, 0));
            Assert.AreEqual(129225, bar.Open);
            Assert.AreEqual(129225, bar.High);
            Assert.AreEqual(125270, bar.Low);
            Assert.AreEqual(126495, bar.Close);
            Assert.AreEqual(195580, bar.Volume);
        }

        [TestMethod]
        public void Bar_Parse_With_Symbol()
        {
            string barString = "RTS-12.12,20111003,100000,129225.00000,129225.00000,125270.00000,126495.00000,195580";

            Bar bar = Bar.Parse(barString);

            Assert.AreEqual("RTS-12.12", bar.Symbol);
            Assert.AreEqual(bar.DateTime, new DateTime(2011, 10, 3, 10, 0, 0));
            Assert.AreEqual(129225, bar.Open);
            Assert.AreEqual(129225, bar.High);
            Assert.AreEqual(125270, bar.Low);
            Assert.AreEqual(126495, bar.Close);
            Assert.AreEqual(195580, bar.Volume);
        }

        [TestMethod]
        public void Bar_parse_with_symbol_and_period()
        {
            string src = "RTS-6.13,15,20130610,101500,130040.0000000,130940.0000000,130000.0000000,130420.0000000,44666";

            Bar bar = Bar.Parse(src);
            Assert.AreEqual("RTS-6.13", bar.Symbol);
            Assert.AreEqual(900, bar.Interval);
            Assert.AreEqual(bar.DateTime, new DateTime(2013, 6, 10, 10, 15, 0));
            Assert.AreEqual(130040, bar.Open);
            Assert.AreEqual(130940, bar.High);
            Assert.AreEqual(130000, bar.Low);
            Assert.AreEqual(130420, bar.Close);
            Assert.AreEqual(44666, bar.Volume);
        }

        [TestMethod]
        public void Bar_ToString()
        {
            Bar bar = new Bar { Symbol = "RTS-3.13_FT", DateTime = new DateTime(2013, 1, 8), Open = 147150, High = 149300, Low = 146900, Close = 149120, Volume = 109480 };

            CultureInfo ci = CultureInfo.InvariantCulture;

            string result = String.Format("Symbol: {0}, Interval: {1}, DateTime: {2}, Open: {3}, High: {4}, Low: {5}, Close: {6}, Volume: {7}",
                bar.Symbol, bar.Interval, bar.DateTime.ToString(ci), bar.Open.ToString("0.0000", ci), bar.High.ToString("0.0000", ci), bar.Low.ToString("0.0000", ci), bar.Close.ToString("0.0000", ci), bar.Volume.ToString("0.0000", ci));

            Assert.AreEqual(result, bar.ToString());
        }

        [TestMethod]
        public void Bar_ToImportString()
        {
            Bar bar = new Bar { Symbol = "RTS-3.13_FT", DateTime = new DateTime(2013, 1, 8), Open = 147150.1, High = 149300.23, Low = 146900.356, Close = 149120.8327, Volume = 109480.0 };

            CultureInfo ci = CultureInfo.InvariantCulture;

            string result = String.Format("{0},{1},{2:yyyyMMdd,HHmmss},{3},{4},{5},{6},{7}",
                bar.Symbol, bar.Interval, bar.DateTime.ToString(ci), bar.Open.ToString("0.0000", ci), bar.High.ToString("0.0000", ci), bar.Low.ToString("0.0000", ci), bar.Close.ToString("0.0000", ci), bar.Volume.ToString("0.0000", ci));

            Assert.AreEqual(result, bar.ToImportString());
        }

        [TestMethod]
        public void Bar_IsWhite()
        {
            Bar bar = new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 150, 155, 149, 153, 10);

            Assert.IsTrue(bar.IsWhite);
            Assert.IsFalse(bar.IsBlack);
        }

        [TestMethod]
        public void Bar_IsBlack()
        {
            Bar bar = new Bar("RTS-9.13_FT", 60, BrokerDateTime.Make(DateTime.Now), 150, 151, 147, 148, 10);

            Assert.IsTrue(bar.IsBlack);
            Assert.IsFalse(bar.IsWhite);
        }
    }
}
