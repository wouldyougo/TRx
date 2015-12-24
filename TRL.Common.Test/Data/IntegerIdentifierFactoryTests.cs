using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;

namespace TRL.Common.Test.Data
{
    [TestClass]
    public class SerialIntegerFactoryTests
    {
        [TestMethod]
        public void SerialIntegerFactory_Makes_Value_Greater_Than_Zero()
        {
            int cookie = SerialIntegerFactory.Make();

            Assert.IsTrue(cookie > 0);
        }

        [TestMethod]
        public void SerialInteger_Values_Are_Not_Equals()
        {
            int first = SerialIntegerFactory.Make();
            int second = SerialIntegerFactory.Make();

            Assert.AreNotEqual(first, second);
            Assert.IsTrue(first + 1 == second);
        }

    }
}
