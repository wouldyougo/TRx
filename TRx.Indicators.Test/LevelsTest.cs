using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace TRx.Indicators.Test
{
    [TestClass]
    public class LevelsTest
    {
        public Levels levels;

        [TestInitialize]
        public void Setup()
        {
            Assert.IsNull(levels);
            levels = new Levels(3, 10);
            Assert.IsNotNull(levels);
        }

        [TestMethod]
        public void LevelsInitTest()
        {
            Assert.AreEqual(3, levels.УровеньСредний);
            Assert.AreEqual(4, levels.ПоловинаУровней);
            Assert.AreEqual(7, levels.КоличествоУровней);
            Assert.AreEqual(0, levels.ТипУровней);
            Assert.AreEqual(10, levels.ШагУровней);
            Assert.AreEqual(4, levels.LevelValueUp.Count);
            Assert.AreEqual(4, levels.LevelValueDn.Count);
            Assert.AreEqual(7, levels.LevelValue.Count);
            Assert.AreEqual(-levels.ШагУровней * 3, levels.LevelValue[0]);
            Assert.AreEqual(levels.ШагУровней * 0, levels.LevelValue[3]);
            Assert.AreEqual(levels.ШагУровней * 3, levels.LevelValue[6]);
        }
        public void LevelsCountTest()
        {
            Assert.AreEqual(4, levels.ПоловинаУровней);
            Assert.AreEqual(7, levels.КоличествоУровней);
        }
        [TestMethod]
        public void LevelsCrossTest()
        {
            double[] first = { 3.0, 4.0, 5.0, 1.0, 25.0 };
            double[] second = { 2.0, 2.0, 6.0, -11.0, -35.0 };
            IList<bool> ПересеченияСверху;
            IList<bool> ПересеченияСнизу;

            ПересеченияСверху = levels.ПересеченияУровенейСверху(first);
            ПересеченияСнизу = levels.ПересеченияУровенейСнизу(first);

            Assert.IsNotNull(ПересеченияСверху);
            Assert.IsNotNull(ПересеченияСнизу);

            Assert.AreSame(ПересеченияСверху, levels.ПересеченияСверху);
            Assert.AreSame(ПересеченияСнизу, levels.ПересеченияСнизу);

            Assert.AreEqual(ПересеченияСверху.Count, levels.КоличествоУровней);
            Assert.AreEqual(ПересеченияСнизу.Count, levels.КоличествоУровней);

            Assert.AreEqual(2, ПересеченияСнизу.Where(i => i == true).Count());
            Assert.AreEqual(0, ПересеченияСверху.Where(i => i == true).Count());

            ПересеченияСверху = levels.ПересеченияУровенейСверху(second);
            ПересеченияСнизу = levels.ПересеченияУровенейСнизу(second);

            Assert.AreEqual(0, ПересеченияСнизу.Where(i => i == true).Count());
            Assert.AreEqual(2, ПересеченияСверху.Where(i => i == true).Count());
        }

        [TestMethod]
        public void LevelsTouchTest()
        {
            double[] first = { 3.0, 4.0, 5.0, 0.0, 20.0 };
            double[] second = { 2.0, 2.0, 6.0, 0.0, -30.0 };
            IList<bool> ПересеченияСверху;
            IList<bool> ПересеченияСнизу;

            ПересеченияСверху = levels.ПересеченияУровенейСверху(first);
            ПересеченияСнизу = levels.ПересеченияУровенейСнизу(first);

            Assert.IsNotNull(ПересеченияСверху);
            Assert.IsNotNull(ПересеченияСнизу);

            Assert.AreSame(ПересеченияСверху, levels.ПересеченияСверху);
            Assert.AreSame(ПересеченияСнизу, levels.ПересеченияСнизу);

            Assert.AreEqual(ПересеченияСверху.Count, levels.КоличествоУровней);
            Assert.AreEqual(ПересеченияСнизу.Count, levels.КоличествоУровней);

            Assert.AreEqual(2, ПересеченияСнизу.Where(i => i == true).Count());
            Assert.AreEqual(0, ПересеченияСверху.Where(i => i == true).Count());

            ПересеченияСверху = levels.ПересеченияУровенейСверху(second);
            ПересеченияСнизу = levels.ПересеченияУровенейСнизу(second);

            Assert.AreEqual(0, ПересеченияСнизу.Where(i => i == true).Count());
            Assert.AreEqual(3, ПересеченияСверху.Where(i => i == true).Count());
        }
    }
}
