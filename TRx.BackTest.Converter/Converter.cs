using TRL.Common;
using TRL.Common.Collections;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Models;
//using TRL.Connect.Smartcom;
//using TRL.Connect.Smartcom.Data;
//using TRL.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRL.Common.TimeHelpers;
using TRL.Transaction;
using TRL.Logging;
using TRL.Common.Handlers;
using TRS.Helpers;
using TRS.BackTest;
/// <summary>
/// Утилита для конвертации тиковых данных в Range-бары
/// Работает в windows x86 (теперь и x64) под .net framework 
/// Читает тиковые файлы в формате финам
/// </summary>
namespace TRS.BackTest
{
    class Converter : TRS.BackTest.Template
    {
        ///// <summary>
        ///// в базовом бактесте не нужен
        ///// </summary>
        //private static SmartComAdapter adapter { get; set; }

        //private static TRS.Strategy.Sample1 strategySample1 { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main(string[] args)
        {
            Template tmp = new Converter();
            tmp.Do(args);
        }

        #region // переопределение базовых методов
        /// <summary>
        /// пример переопределения
        /// BackTest Sample1 Initialize()
        /// </summary>
        override public void Initialize()
        {
            Console.WriteLine("BackTest.Converter.Initialize()");
            //marketDataProvider = new MarketDataProvider();
            //rawTradingDataProvider = new RawTradingDataProvider(DefaultLogger.Instance);
            //symbolsDataProvider = new SymbolsDataProvider();
            orderManager = new TRL.Emulation.BacktestOrderManager(TradingData.Instance, DefaultLogger.Instance);
            traderBase = new TraderBase(orderManager);
        }

        /// <summary>
        /// пример переопределения
        /// Program.Sample1.SetupStrategy()
        /// </summary>
        /// <param name="args"></param>
        override public void SetupStrategy(string[] args)
        {
            Console.WriteLine("BackTest.Converter.SetupStrategy()");
            // инициализация обработчиков стратегии
            //strategySample1 = new Strategy.Sample1(args);
            //strategyHeader = strategySample1.strategyHeader;
            //AppSettings.GetStringValue("Symbol")
            string symbol = System.Configuration.ConfigurationManager.AppSettings["Symbol"];
            //Console.WriteLine(String.Format("Sybol: {0}", symbol));
            if (symbol == "") symbol = null;

            StrategyHeader strategyHeader = new StrategyHeader(1, "Sample strategyHeader", null, symbol, 1);
            BarSettings barSettings = new BarSettings(
                strategyHeader,
                strategyHeader.Symbol,
                //null,
                AppSettings.GetValue<int>("Interval"),
                AppSettings.GetValue<int>("Period"));

            TradingData.Instance.Get<ICollection<StrategyHeader>>().Add(strategyHeader);

            //BarSettings barSettings = new BarSettings(strategyHeader, "RIH4", 3600, 3);
            //BarSettings barSettings = new BarSettings(strategyHeader, "SPFB.RTS-3.14", 3600, 3);
            //TradingData.Instance.Get<ICollection<BarSettings>>().Add(barSettings);

            MakeRangeBarsOnTick updateBarsHandler =
                new MakeRangeBarsOnTick(barSettings,
                    new TimeTracker(),
                    TradingData.Instance,
                    DefaultLogger.Instance);
        }
        #endregion //

        #region // переопределение консольных команд
        /// <summary>
        /// переопределение консольные команды
        /// Бары
        /// </summary>
        override public void MethodHelp()
        {
            Console.Clear();
            Console.WriteLine("override ConsoleHandlerH()");
            base.MethodHelp();
            Console.WriteLine("Use data files in args:");
            Console.WriteLine("Converter.exe in.txt");
            Console.WriteLine("or use filename by default: {0}", fileNameDefault);
            Console.WriteLine("Interval - config file");
            Console.WriteLine("Symbol - config file");
        }
        #endregion //
    }
}