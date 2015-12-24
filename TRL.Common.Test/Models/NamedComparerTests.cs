using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TRL.Common.Models;

namespace TRL.Common.Test.Models
{
    [TestClass]
    public class NamedComparerTests
    {
        [TestMethod]
        public void NamedEqualityComparer_test()
        {
            IEqualityComparer<Model> iec = new NamedEqualityComparer<Model>();

            HashSet<Model> models = new HashSet<Model>(iec);

            Model first = new Model("First", 1);

            models.Add(first);

            Assert.AreEqual(1, models.Count);

            Model second = new Model("First", 3);

            models.Add(second);

            Assert.AreEqual(1, models.Count);
        }
    }
}
