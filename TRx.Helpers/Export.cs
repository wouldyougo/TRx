using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
//using System.Reflection;
//using System.Threading.Tasks;
//using Microsoft.AspNet.SignalR;
//using Microsoft.Owin.Hosting;
//using SmartCOM3Lib;

using TRL.Logging;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Models;
using TRL.Common.Collections;
using TRL.Common.Statistics;
using TRL.Transaction;
//using TRL.Connect.Smartcom.Commands;
//using TRL.Common;
//using TRL.Common.Extensions.Data;
//using TRL.Common.Handlers;
//using TRL.Common.TimeHelpers;
//using TRL.Connect.Smartcom;
//using TRL.Connect.Smartcom.Commands;
//using TRL.Connect.Smartcom.Data;
//using TRL.Handlers.Inputs;
//using TRL.Connect.Smartcom.Handlers;

namespace TRx.Helpers
{
    public class Export
    {
        public static void LogAssemblyInfo(string[] assemblies)
        {
                foreach (string assembly in assemblies)
                {
                    try
                    {
                    DefaultLogger.Instance.Log(
                        String.Format("assembly: {0} version: {1}",
                        assembly,
                        FileVersionInfo.GetVersionInfo(assembly).ProductVersion));
                    }
                    catch {
                        Console.WriteLine(String.Format("Exeption LogAssemblyInfo {0}", assembly));
                    }
                }
        }

        public static void ExportData<T>(bool confirmExport)
        {
            if (!confirmExport)
                return;

            string prefix = typeof(T).Name;

            ILogger logger = new TextFileLogger(prefix, 10000000, true);

            if (prefix == "Bar")
            {
                //logger.Log("<TICKER>,<PER>,<DATE>,<TIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOL>");
                logger.Log("<TICKER>,<PER>,<DATE>,<TIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOL>,<DATEID>");

                foreach (Bar item in TradingData.Instance.Get<IEnumerable<Bar>>().OrderBy(i => i.DateTime))
                {
                    //logger.Log(item.ToStringFinam());
                    logger.Log(item.ToStringDateID());
                }
            }
            else
            {
                ///TODO 1. ExportData<T> Добавить BarDateId
                foreach (T item in TradingData.Instance.Get<IEnumerable<T>>())
                    logger.Log(item.ToString());
            }
        }

        public static void ExportData<T>(IEnumerable<T> data)
        {
            //if (!confirmExport)
            //    return;
            string prefix = typeof(T).Name;
            ILogger logger = new TextFileLogger(prefix, 10000000, true);

            //if (prefix == "Bar")
            //{
            //    //logger.Log("<TICKER>,<PER>,<DATE>,<TIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOL>");

            //    foreach (Bar item in TradingData.Instance.Get<IEnumerable<Bar>>().OrderBy(i => i.DateTime))
            //    {
            //        logger.Log(item.ToFinamString());
            //    }
            //}
                foreach (T item in data)
                    logger.Log(item.ToString());
        }

        public static void ExportDeal<Deal>(IEnumerable<Deal> data)
        {
            //if (!confirmExport)
            //    return;
            string prefix = typeof(Deal).Name;
            ILogger logger = new TextFileLogger(prefix, 10000000, true);

            //if (prefix == "Bar")
            //{
            //    //logger.Log("<TICKER>,<PER>,<DATE>,<TIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOL>");

            //    foreach (Bar item in TradingData.Instance.Get<IEnumerable<Bar>>().OrderBy(i => i.DateTime))
            //    {
            //        logger.Log(item.ToFinamString());
            //    }
            //}
            foreach (Deal item in data)
                logger.Log(item.ToString());
        }
    }
}
