using TRL.Common;
using TRL.Common.Collections;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Models;
using TRL.Connect.Smartcom;
using TRL.Connect.Smartcom.Data;
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

namespace TRx.Trader.BackTest
{
    class Program
    {
        private static MarketDataProvider marketDataProvider = new MarketDataProvider();
        private static RawTradingDataProvider rawTradingDataProvider = new RawTradingDataProvider();
        private static SymbolsDataProvider symbolsDataProvider = new SymbolsDataProvider();

        private static IOrderManager orderManager = new TRL.Emulation.BacktestOrderManager(TradingData.Instance, DefaultLogger.Instance);

        private static TraderBase traderBase =
            new TraderBase(orderManager);

        //private static Strategy strategyHeader = new Strategy(1, "Sample strategyHeader", "ST46520-RF-01", "RIH4", 1);
        private static StrategyHeader strategyHeader = new StrategyHeader(1, "Sample strategyHeader", "ST46520-RF-01", "SPFB.RTS-3.14", 1);
        //private static BarSettings barSettings = new BarSettings(strategyHeader, "SPFB.RTS-3.14", 3600, 3);
        private static BarSettings barSettings = new BarSettings(
            strategyHeader,
            strategyHeader.Symbol,
            AppSettings.GetValue<int>("Interval"),
            AppSettings.GetValue<int>("Period"));

        static void Main(string[] args)
        {
            try
            {
                Console.BufferHeight = 300;
                Console.BufferWidth = 230;

                Console.WindowHeight = 90;
                Console.WindowWidth = 110;

                Console.WindowTop = 0;
                Console.WindowLeft = 0;
            }
            catch
            {
                Console.WriteLine(String.Format("Error Console.Window Size"));
            }

            Console.WriteLine(String.Format("Console.BufferHeight {0}", Console.BufferHeight));
            Console.WriteLine(String.Format("Console.BufferWidth {0}", Console.BufferWidth));
            Console.WriteLine(String.Format("Console.WindowHeight {0}", Console.WindowHeight));
            Console.WriteLine(String.Format("Console.WindowWidth {0}", Console.WindowWidth));
            Console.WriteLine(String.Format("Console.WindowTop {0}", Console.WindowTop));
            Console.WriteLine(String.Format("Console.WindowLeft {0}", Console.WindowLeft));


            TradingData.Instance.Get<ICollection<StrategyHeader>>().Add(strategyHeader);

            //BarSettings barSettings = new BarSettings(strategyHeader, "RIH4", 3600, 3);
            //BarSettings barSettings = new BarSettings(strategyHeader, "SPFB.RTS-3.14", 3600, 3);
            TradingData.Instance.Get<ICollection<BarSettings>>().Add(barSettings);

            BreakOutOnBar breakOnBar =
                new BreakOutOnBar(strategyHeader,
                    TradingData.Instance,
                    SignalQueue.Instance,
                    DefaultLogger.Instance);

            StopLossOnBar stopLossOnBar =
                new StopLossOnBar(strategyHeader,
                    100,
                    TradingData.Instance,
                    SignalQueue.Instance,
                    DefaultLogger.Instance);

            TakeProfitOnBar takeProfitOnBar =
                new TakeProfitOnBar(strategyHeader,
                    100,
                    TradingData.Instance,
                    SignalQueue.Instance,
                    DefaultLogger.Instance);


            ITransaction importBars =
                new ImportBarsTransaction(TradingData.Instance.Get<ObservableCollection<Bar>>(),
                    "bars.txt");

            importBars.Execute();

            //список доступных команд
            ConsoleWriteCommands();

            while (true)
            {
                try
                {
                    string command = Console.ReadLine();

                    if (command == "x")
                    {
                        //adapter.Stop();

                        //ExportData<Order>(AppSettings.GetValue<bool>("ExportOrdersOnExit"));
                        ExportData<Trade>(AppSettings.GetValue<bool>("ExportTradesOnExit"));
                        ExportData<Bar>(AppSettings.GetValue<bool>("ExportBarsOnExit"));

                        break;
                    }
                    if (command == "h")
                    {
                        Console.Clear();
                        ConsoleWriteCommands();
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
        private static void ConsoleWriteCommands()
        {
            Console.WriteLine("p - Реализованный профит и лосс");
            Console.WriteLine("b - TradingData.Instance.Get<IEnumerable<Bar>>()");
            Console.WriteLine("t - TradingData.Instance.Get<IEnumerable<Trade>>()");
            Console.WriteLine("h - Help");
            Console.WriteLine("x - Stop");
        }

        private static void ExportData<T>(bool confirmExport)
        {
            if (!confirmExport)
                return;

            string prefix = typeof(T).Name;

            ILogger logger = new TextFileLogger(prefix, 10000000, true);

            if (prefix == "Bar")
            {
                logger.Log("<TICKER>,<PER>,<DATE>,<TIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOL>");

                foreach (Bar item in TradingData.Instance.Get<IEnumerable<Bar>>().OrderBy(i => i.DateTime))
                {
                    logger.Log(item.ToStringFinam());
                }
            }
            else
            {
                foreach (T item in TradingData.Instance.Get<IEnumerable<T>>())
                    logger.Log(item.ToString());
            }
        }
    }
}
