using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TRL.Common.Test
{
    [TestClass]
    public class AppSettingsTests
    {

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void Configuration_AppSettingsConfiguration_Get_Non_Existent_Parameter()
        {
            AppSettings.GetValue<int>("NonExistantIntegerKeyName");
        }

        [TestMethod]
        public void Configuration_AppSettingsConfiguration_Get_Integer_Value()
        {
            Assert.AreEqual(358, AppSettings.GetValue<int>("SampleIntegerParameterKeyName"));
        }

        [TestMethod]
        public void Configuration_AppSettingsConfiguration_Get_Guid_Value()
        {
            Assert.AreEqual(new Guid("88888888888888888888888888888888"), AppSettings.GetValue<Guid>("SampleGuid"));
        }

        [TestMethod]
        public void Configuration_AppSettingsConfiguration_Get_String_Value()
        {
            Assert.AreEqual("Sample string", AppSettings.GetStringValue("SampleStringParameterKeyName"));
        }

        [TestMethod]
        public void Configuration_AppSettingsConfiguration_Get_Decimal_Value()
        {
            Assert.AreEqual(38.88m, AppSettings.GetValue<decimal>("SampleDecimalParameterKeyName"));
        }

        [TestMethod]
        public void Configuration_AppSettingsConfiguration_Get_Boolean_Value()
        {
            Assert.AreEqual(true, AppSettings.GetValue<bool>("SampleBooleanParameterKeyName"));
        }

        [TestMethod]
        public void Configuration_AppSettingsConfiguration_Get_Nonexistent_Boolean_Value()
        {
            Assert.AreEqual(false, AppSettings.GetValue<bool>("NonexistentBooleanParameterKeyName"));
        }
    }
}
