using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Common.Data;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Test
{
    [TestClass]
    public class TextFileStringListFactoryTests
    {
        [TestMethod]
        public void TextFileStringListFactory_test()
        {
            IGenericFactory<IEnumerable<string>> logReader = new TextFileStringListFactory(String.Concat(ProjectRootFolderNameFactory.Make(), "\\TestData\\logfile.txt"));

            IEnumerable<string> log = logReader.Make();

            Assert.AreEqual(10, log.Count());
        }
    }
}
