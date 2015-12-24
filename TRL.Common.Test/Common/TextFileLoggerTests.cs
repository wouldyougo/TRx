using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using TRL.Logging;

namespace TRL.Common.Test
{
    [TestClass]
    public class TextFileLoggerTests
    {
        private TextFileLogger logger;

        [TestInitialize]
        public void Common_Setup()
        {
            this.logger = new TextFileLogger();
        }

        [TestCleanup]
        public void Common_Teardown()
        {
            this.logger.Dispose();

            File.Delete("default-0.log");
            File.Delete("log-0.log");
            File.Delete("log-1.log");
        }

        [TestMethod]
        public void Common_LogMessage()
        {
            string message = String.Concat(DateTime.Now.ToLocalTime(), ";Тестовое сообщение в журнал");
            this.logger.Log(message);

            Assert.IsTrue(File.Exists("default-0.log"));

            StreamReader streamReader = new StreamReader(new FileStream("default-0.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            Assert.AreEqual(message, streamReader.ReadLine());
                
            streamReader.Close();
            streamReader.Dispose();
        }

        [TestMethod]
        public void Common_Create_New_File_On_Size_Limit_Exceed()
        {
            TextFileLogger logger = new TextFileLogger("log", 10000);

            for (int i = 0; i < 200; i++)
                logger.Log(String.Format("{0}, Запись в журнале", DateTime.Now));

            Assert.IsTrue(File.Exists("log-0.log"));
            
            FileInfo fileInfo = new FileInfo("log-0.log");
            
            Assert.IsTrue(fileInfo.Length > 10000);

            Assert.IsTrue(File.Exists("log-1.log"));

            StreamReader streamReader = new StreamReader(new FileStream("log-1.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            Assert.IsTrue(streamReader.ReadLine().Contains("Запись в журнале"));

            streamReader.Close();
            streamReader.Dispose();

            logger.Dispose();
        }
    }
}
