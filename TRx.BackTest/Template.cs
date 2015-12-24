using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Reflection;
//using System.Diagnostics;
//using Microsoft.AspNet.SignalR;
//using Microsoft.Owin.Hosting;

using SmartCOM3Lib;

using TRL.Logging;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.Handlers;
using TRL.Common.Collections;
using TRL.Connect.Smartcom.Data;
using TRL.Transaction;
using TRx.Helpers;
using TRx.Handlers;

//using TRL.Common;
//using TRL.Common.Extensions.Data;
//using TRL.Common.TimeHelpers;
//using TRL.Connect.Smartcom.Commands;
//using TRL.Connect.Smartcom;
//using TRL.Connect.Smartcom.Handlers;
//using TRL.Handlers.Spreads;
//using TRL.Handlers.StopLoss;
//using TRL.Handlers.TakeProfit;
//using TRx.TestBack;
//using TRL.Common.Statistics;
//using TRx.Base;
//<add key="Symbol" value="Si-9.15_FT" />

//namespace TRx.Base //TRx.TestBack
namespace TRx.BackTest
{
    /// <summary>
    /// шаблон консольного приложения BackTest
    /// </summary>
    public class Template: TRx.Base.Template
    {
        ///// <summary>
        ///// в базовом бактесте не нужен
        ///// может быть нужен в тесте на реальном подключении
        ///// </summary>
        //private static SmartComAdapter adapter { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main(string[] args)
        {
            Template tmp = new Template(args);
            tmp.Do();
        }
        public Template(string[] args) : base(args)
        {
            Console.WriteLine("BackTest.Template()");
            Initialize();
            SetupStrategy(args);
        }

        #region // переопределение базовых методов
        /// <summary>
        /// BackTest Template Do()
        /// </summary>
        /// <param name="args"></param>
        override public void Do()
        {
            Console.WriteLine("BackTest.Template.Do()");
            //TradeConsole.LogAssemblyInfo(assemblies);
            //Initialize();
            //SetupStrategy(args);
            base.Do();
            //Template.MethodServerStart();
            //////ImportTicksTransaction(args);
            //MethodWhile();
            //Template.MethodServerDispose();
        }
        /// <summary>
        /// BackTest Template Initialize()
        /// </summary>
        override public void Initialize()
        {
            Console.WriteLine("BackTest.Template.Initialize()");
            marketDataProvider = new MarketDataProvider();
            rawTradingDataProvider = new RawTradingDataProvider(DefaultLogger.Instance);
            symbolsDataProvider = new SymbolsDataProvider();
            orderManager = new TRL.Emulation.BacktestOrderManager(TradingData.Instance, DefaultLogger.Instance);
            //private static SmartComAdapter adapter =
            //    new SmartComAdapter();
            traderBase = new TraderBase(orderManager);
            //new TraderBase(new SmartComOrderManager());
            //strategySettings = new StrategySettings(1,
            //    "ReversOnBar strategy",
            //    AppSettings.GetStringValue("Portfolio"),
            //    AppSettings.GetStringValue("Symbol"),
            //    AppSettings.GetValue<double>("Amount"));
            //barSettings = new BarSettings(
            //    strategySettings,
            //    strategySettings.Symbol,
            //    AppSettings.GetValue<int>("Interval"),
            //    AppSettings.GetValue<int>("Period"));
            //assemblies = { "Interop.SmartCOM3Lib.dll", "TRL.Common.dll", "TRL.Connect.Smartcom.dll" };
        }

        /// <summary>
        /// пример
        /// инициализация обработчиков стратегии
        /// </summary>
        /// <param name="args"></param>
        override public void SetupStrategy(string[] args)
        {
            // инициализация обработчиков стратегии
            Console.WriteLine("BackTest.Template.SetupStrategy");
        }
        /// <summary>
        /// пример
        /// действия после старта
        /// </summary>
        /// <param name="args"></param>
        override public void MethodPreStart(string[] args)
        {
            // действия после старта
            Console.WriteLine("BackTest.Template.MethodPreStart()");
            ImportTicksTransaction(args);
        }

        #endregion //

        public static string fileNameDefault = "ticks.txt";

        public static void ImportTicksTransaction(string[] args)
        {
            Console.WriteLine("");
            Console.WriteLine(String.Format("TimeStart: {0}", System.DateTime.Now));
            System.DateTime TimeStart = System.DateTime.Now;
            if (args.Count() > 0)
            {
                foreach (var item in args)
                {
                    Console.WriteLine(String.Format("Read: {0}", item.ToString()));
                    ITransaction tickImportTransaction =
                        new ImportTicksTransaction(TradingData.Instance.Get<ObservableCollection<Tick>>(),
                            item);
                    tickImportTransaction.Execute();
                }
            }
            else
            {
                Console.WriteLine(String.Format("Read: {0}", fileNameDefault.ToString()));
                ITransaction tickImportTransaction =
                    new ImportTicksTransaction(TradingData.Instance.Get<ObservableCollection<Tick>>(),
                        fileNameDefault);
                tickImportTransaction.Execute();
            }
            Console.WriteLine(String.Format("TimeFinish: {0}", System.DateTime.Now));
            System.DateTime TimeFinish = System.DateTime.Now;
            var TimeTotal = TimeFinish - TimeStart;
            Console.WriteLine(String.Format("TimeTotal: {0}", TimeTotal));
        }
    }
}