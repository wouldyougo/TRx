using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Models;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class OrderSettingsEqualityComparerTests
    {
        [TestMethod]
        public void OrderSettingsEqualityComparer_test_settings_are_equals()
        {
            StrategyHeader strategyHeader = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            OrderSettings os1 = new OrderSettings(strategyHeader, 60);
            OrderSettings os2 = new OrderSettings(strategyHeader, 120);

            OrderSettingsComparer comparer = new OrderSettingsComparer();

            Assert.IsTrue(comparer.Equals(os1, os2));
        }

        [TestMethod]
        public void OrderSettingsEqualityComparer_test_settings_are_not_equals()
        {
            StrategyHeader strategy1 = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            StrategyHeader strategy2 = new StrategyHeader(2, "Strategy 2", "BP12345-RF-01", "RTS-9.13_FT", 10);
            OrderSettings os1 = new OrderSettings(strategy1, 60);
            OrderSettings os2 = new OrderSettings(strategy2, 60);

            OrderSettingsComparer comparer = new OrderSettingsComparer();

            Assert.IsFalse(comparer.Equals(os1, os2));
        }

        [TestMethod]
        public void OrderSettingsEqualityComparer_test_cant_add_two_equals_settings()
        {
            StrategyHeader strategy1 = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            OrderSettings os1 = new OrderSettings(strategy1, 60);
            OrderSettings os2 = new OrderSettings(strategy1, 300);

            HashSet<OrderSettings> collection = new HashSet<OrderSettings>(new OrderSettingsComparer());

            collection.Add(os1);
            Assert.AreEqual(1, collection.Count);

            collection.Add(os2);
            Assert.AreEqual(1, collection.Count);
        }

        [TestMethod]
        public void OrderSettingsEqualityComparer_test_can_add_two_equals_settings()
        {
            StrategyHeader strategy1 = new StrategyHeader(1, "Strategy 1", "BP12345-RF-01", "RTS-9.13_FT", 10);
            StrategyHeader strategy2 = new StrategyHeader(2, "Strategy 2", "BP12345-RF-01", "RTS-9.13_FT", 10);
            OrderSettings os1 = new OrderSettings(strategy1, 60);
            OrderSettings os2 = new OrderSettings(strategy2, 300);

            HashSet<OrderSettings> collection = new HashSet<OrderSettings>(new OrderSettingsComparer());

            collection.Add(os1);
            Assert.AreEqual(1, collection.Count);

            collection.Add(os2);
            Assert.AreEqual(2, collection.Count);
        }
    }
}
