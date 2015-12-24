using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace TRL.Common.Test
{
    [TestClass]
    public class ProjectRootFolderNameFactoryTests
    {
        [TestMethod]
        public void Common_RootFolderNameFactory_File_Exists()
        {
            string fileName = String.Concat(ProjectRootFolderNameFactory.Make(), "\\App_Data\\Import\\SPFB.RTS_120901_121001_Hour.txt");

            Assert.IsTrue(File.Exists(fileName));
        }
    }
}
