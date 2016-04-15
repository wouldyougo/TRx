using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Test
{
    [TestClass]
    public class StructFactoryTests
    {
        [TestMethod]
        public void Common_ConvertStringToNullableInt32()
        {
            string source = "100";

            Assert.AreEqual(100, StructFactory.MakeNullable<int>(source));
        }

        [TestMethod]
        public void Common_ConvertStringToInt32()
        {
            string source = "100";

            Assert.AreEqual(100, StructFactory.Make<int>(source));
        }

        [TestMethod]
        public void Common_ConvertStringToDecimal()
        {
            //string source = "100,00";
            string source = "100.00";

            Assert.AreEqual(100m, StructFactory.Make<Decimal>(source));
        }

        [TestMethod]
        public void Common_ConvertStringToNullableDecimal()
        {
            //string source = "100,00";
            string source = "100.00";

            Assert.AreEqual(100m, StructFactory.MakeNullable<Decimal>(source));
        }

        [TestMethod]
        public void Common_ConvertStringToFloat()
        {
            //string source = "100,00";
            string source = "100.00";

            Assert.AreEqual(100F, StructFactory.Make<float>(source));
        }

        [TestMethod]
        public void Common_ConvertStringToNullableFloat()
        {
            //string source = "100,00";
            string source = "100.00";

            Assert.AreEqual(100F, StructFactory.MakeNullable<float>(source));
        }

        [TestMethod]
        public void Common_ConvertStringToGuid()
        {
            string source = "88888888888888888888888888888888";

            Assert.AreEqual(Guid.Parse(source), StructFactory.Make<Guid>(source));
        }

        [TestMethod]
        public void Common_ConvertStringToNullableGuid()
        {
            string source = "88888888888888888888888888888888";

            Assert.AreEqual(Guid.Parse(source), StructFactory.MakeNullable<Guid>(source));
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Common_Make_Int_From_EmptyString_Raise_An_Argument_Exception()
        {
            StructFactory.Make<int>(" ");
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Common_Make_NullableGuid_From_EmptyString_Raise_An_Argument_Exception()
        {
            StructFactory.MakeNullable<Guid>("");
        }
    }
}
