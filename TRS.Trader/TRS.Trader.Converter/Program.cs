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
using TRx.Helpers;
/// <summary>
/// Утилита для конвертации тиковых данных в Range-бары
/// Работает в windows x86 (теперь и x64) под .net framework 
/// Читает тиковые файлы в формате финам
/// </summary>
namespace TRx.Trader.Converter
{
    class Program
    {
        //private static MarketDataProvider marketDataProvider = new MarketDataProvider();
        //private static RawTradingDataProvider rawTradingDataProvider = new RawTradingDataProvider();
        //private static SymbolsDataProvider symbolsDataProvider = new SymbolsDataProvider();

        private static IOrderManager orderManager = new TRL.Emulation.BacktestOrderManager(TradingData.Instance, DefaultLogger.Instance);
        
        private static TraderBase traderBase =
            new TraderBase(orderManager);

        //private static Strategy strategyHeader = new Strategy(1, "Sample strategyHeader", "ST46520-RF-01", "SPFB.Si-9.15", 1);
        //private static Strategy strategyHeader = new Strategy(1, "Sample strategyHeader", null, null, 1);

        static void Main(string[] args)
        {
            TradeConsole.ConsoleSetSize();

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

            TradeConsole.ImportTicksTransaction(args);

            //список доступных команд
            TradeConsole.ConsoleWriteCommands();

            while (true)
            {
                try
                {
                    string command = Console.ReadLine();

                    if (command == "x")
                    {
                        //adapter.Stop();
                        //TradeConsole.ExportData<Order>(AppSettings.GetValue<bool>("ExportOrdersOnExit"));
                        //TradeConsole.ExportData<Trade>(AppSettings.GetValue<bool>("ExportTradesOnExit"));
                        Export.ExportData<Bar>(AppSettings.GetValue<bool>("ExportBarsOnExit"));

                        break;
                    }
                    if (command == "h")
                    {
                        Console.Clear();
                        TradeConsole.ConsoleWriteCommands();
                        Console.WriteLine("Use data files in args:");
                        Console.WriteLine("Converter.exe in.txt");
                        Console.WriteLine("or use filename by default: {0}", TradeConsole.fileNameDefault);
                        Console.WriteLine("Interval - config file");
                        Console.WriteLine("Symbol - config file");
                    }

                    if (command == "t")
                    {
                        Console.Clear();

                        foreach (Trade item in TradingData.Instance.Get<IEnumerable<Trade>>())
                            Console.WriteLine(item.ToString());
                    }

                    if (command == "b")
                    {
                        Console.Clear();
                        foreach (Bar item in TradingData.Instance.Get<IEnumerable<Bar>>().OrderBy(i => i.DateTime))
                        //foreach (Bar item in TradingData.Instance.Get<IEnumerable<Bar>>())
                        {
                            Console.WriteLine(item.ToString());
                            //Console.WriteLine(item.ToImportString());
                        }
                    }

                    if (command == "p")
                    {
                        Console.WriteLine(String.Format("Реализованный профит и лосс составляет {0} пунктов",
                            TradingData.Instance.GetProfitAndLossPoints(strategyHeader)));
                    }
                }
                catch (System.Runtime.InteropServices.COMException e)
                {
                    DefaultLogger.Instance.Log(e.Message);

                }
            }
        }
    }
}
//пример формата даты
//DateTime DateTime = new DateTime(2015, 1, 8);
//System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.InvariantCulture;
//string result;
//result = String.Format("{0:yyyyMMdd,HHmmss}", DateTime.ToString(ci));
//Console.WriteLine(result);
//result = String.Format("{0:yyyyMMdd,HHmmss}", DateTime);
//Console.WriteLine(result);
//result = String.Format("{0}", DateTime.ToString("yyyyMMdd,HHmmss"));
//Console.WriteLine(result);
//result = String.Format("{0}", DateTime);
//Console.WriteLine(result);

//BreakOutOnBar breakOnBar =
//    new BreakOutOnBar(strategyHeader,
//        TradingData.Instance,
//        SignalQueue.Instance,
//        DefaultLogger.Instance);

//StopLossOnBar stopLossOnBar =
//    new StopLossOnBar(strategyHeader,
//        100,
//        TradingData.Instance,
//        SignalQueue.Instance,
//        DefaultLogger.Instance);

//TakeProfitOnBar takeProfitOnBar =
//    new TakeProfitOnBar(strategyHeader,
//        100,
//        TradingData.Instance,
//        SignalQueue.Instance,
//        DefaultLogger.Instance);