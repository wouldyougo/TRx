using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
//using Microsoft.AspNet.SignalR;
//using Microsoft.Owin.Hosting;

//using SmartCOM3Lib;

using TRL.Logging;
using TRL.Common.Data;
using TRL.Common.Handlers;
using TRL.Connect.Smartcom.Data;
using TRx.Helpers;
using TRx.Base;

//using TRL.Common;
//using TRL.Common.Extensions.Data;
//using TRL.Common.Models;
//using TRL.Common.Collections;
//using TRL.Common.TimeHelpers;
//using TRL.Connect.Smartcom.Commands;
//using TRL.Transaction;
//using TRL.Connect.Smartcom;
//using TRL.Connect.Smartcom.Handlers;
//using TRL.Handlers.Spreads;
//using TRL.Handlers.StopLoss;
//using TRL.Handlers.TakeProfit;
//using TRx.TestBack;
//using TRx.Handlers;
//using TRL.Common.Statistics;
//using TRx.Base;
//using TRx.BackTest;

//<add key="Symbol" value="Si-9.15_FT" />

//namespace TRx.Base //TRx.TestBack
namespace TRx.BackTest
{
    class BackTestSample3: TRx.BackTest.Template
    {
        public BackTestSample3(string[] args) : base(args)
        {
        }

        ///// <summary>
        ///// в базовом бактесте не нужен
        ///// </summary>
        //private static SmartComAdapter adapter { get; set; }

        private static TRx.Strategy.Sample3 strategySample3 { get; set; }
        //private static ISetupStrategy strategySample3 { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main(string[] args)
        {
            Template tmp = new BackTestSample3(args);
            tmp.Do();
        }

        #region // переопределение базовых методов
        ///// <summary>
        ///// пример
        ///// </summary>
        ///// <param name="args"></param>
        //override public void Do(string[] args)
        //{
        //    //TradeConsole.ConsoleSetSize();
        //    //TradeConsole.LogAssemblyInfo(assemblies);
        //    //список доступных команд
        //    TradeConsole.ConsoleWriteCommands();
        //    Initialize();
        //    SetupStrategy(args);
        //    base.Do(args);
        //}

        /// <summary>
        /// пример переопределения
        /// BackTest Sample3 Initialize()
        /// </summary>
        override public void Initialize()
        {
            Console.WriteLine("Оverride BackTest.Sample3.Initialize()");
            marketDataProvider = new MarketDataProvider();
            rawTradingDataProvider = new RawTradingDataProvider(DefaultLogger.Instance);
            symbolsDataProvider = new SymbolsDataProvider();
            orderManager = new TRL.Emulation.BacktestOrderManager(TradingData.Instance, DefaultLogger.Instance);
            //private static SmartComAdapter adapter =
            //    new SmartComAdapter();
            traderBase = new TraderBase(orderManager);
            //new TraderBase(new SmartComOrderManager());
            //strategySettings = new StrategySettings(1,
            //    "ReversMaOnBar strategy",
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
        /// пример переопределения
        /// Program.Sample1.SetupStrategy()
        /// </summary>
        /// <param name="args"></param>
        override public void SetupStrategy(string[] args)
        {
            Console.WriteLine("Оverride BackTest.Sample3.SetupStrategy()");
            // инициализация обработчиков стратегии
            strategySample3 = new Strategy.Sample3(args);
            strategyHeader = strategySample3.strategyHeader;
        }
        #endregion
        #region // переопределение консольных команд
        /// <summary>
        /// переопределение консольные команды
        /// Бары
        /// </summary>
        override public void ConsoleHandlerB()
        {
            Console.WriteLine("Оverride BackTest.Sample3.ConsoleHandlerB()");
            // здесь вызвать базовый метод
            base.ConsoleHandlerB();
            {
                /*
                Console.Clear();
                Console.WriteLine("override ConsoleHandlerB()");
                foreach (Bar item in TradingData.Instance.Get<IEnumerable<Bar>>().OrderBy(i => i.DateTime))
                //foreach (Bar item in TradingData.Instance.Get<IEnumerable<Bar>>())
                {
                    TradeConsole.ConsoleWriteLineBar(item);
                    //TradeHubStarter.sendBarString(item);
                    TradeHubStarter.sendBar(item);
                }
                */
            }
            // здесь вызвать метод стратегии
            strategySample3.ConsoleHandlerB();
        }
        #endregion //  
    }
}