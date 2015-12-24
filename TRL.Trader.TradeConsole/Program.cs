using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;

using SmartCOM3Lib;

using TRL.Logging;
using TRL.Common;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Models;
using TRL.Common.Handlers;
using TRL.Common.Collections;
using TRL.Common.TimeHelpers;
using TRL.Connect.Smartcom;
using TRL.Connect.Smartcom.Commands;
using TRL.Connect.Smartcom.Data;
using TRL.Connect.Smartcom.Handlers;
using TRL.Handlers.Spreads;
using TRL.Handlers.StopLoss;
using TRL.Handlers.TakeProfit;
using TRL.Handlers.Inputs;
using TRL.Transaction;
using TRL.Trader;

namespace TRL.Trader.TradeConsole
{
    class Program
    {

        private static MarketDataProvider marketDataProvider = 
            new MarketDataProvider();

        private static RawTradingDataProvider rawTradingDataProvider = 
            new RawTradingDataProvider(DefaultLogger.Instance);

        private static SymbolsDataProvider symbolsDataProvider = 
            new SymbolsDataProvider();

        private static TraderBase traderBase =
            new TraderBase(new SmartComOrderManager());

        private static SmartComAdapter adapter = 
            new SmartComAdapter();
        
        private static Strategy strategy = 
            new Strategy(1, 
                "Proboy take profit and stop loss points strategy",
                AppSettings.GetStringValue("Portfolio"),
                AppSettings.GetStringValue("Symbol"),
                AppSettings.GetValue<double>("Amount"));
        
        private static BarSettings barSettings = new BarSettings(strategy,
            strategy.Symbol,
            AppSettings.GetValue<int>("Interval"),
            AppSettings.GetValue<int>("Period"));

        private static ProfitPointsSettings ppSettings =
            new ProfitPointsSettings(strategy, AppSettings.GetValue<double>("ProfitPoints"), false);

        private static TakeProfitOrderSettings poSettings =
            new TakeProfitOrderSettings(strategy, 86400);

        private static StopPointsSettings spSettings =
            new StopPointsSettings(strategy, AppSettings.GetValue<double>("StopPoints"), false);

        private static StopLossOrderSettings soSettings =
            new StopLossOrderSettings(strategy, 86400);

        private static string[] assemblies = { "Interop.SmartCOM3Lib.dll", "TRL.Common.dll", "TRL.Connect.Smartcom.dll", "TRL.Trader.TradeConsole.exe" };

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

            LogAssemblyInfo();

            //AddStrategySettings();
            {
                TradingData.Instance.Get<ICollection<Strategy>>().Add(strategy);
                TradingData.Instance.Get<ICollection<BarSettings>>().Add(barSettings);
                TradingData.Instance.Get<ICollection<ProfitPointsSettings>>().Add(ppSettings);
                TradingData.Instance.Get<ICollection<TakeProfitOrderSettings>>().Add(poSettings);
                TradingData.Instance.Get<ICollection<StopPointsSettings>>().Add(spSettings);
                TradingData.Instance.Get<ICollection<StopLossOrderSettings>>().Add(soSettings);

                //SMASettings smaSettings = new SMASettings(strategy, 7, 14);
                int maf = AppSettings.GetValue<int>("MaFast");
                int mas = AppSettings.GetValue<int>("MaSlow");
                SMASettings smaSettings = new SMASettings(strategy, maf, mas);
                TradingData.Instance.Get<ICollection<SMASettings>>().Add(smaSettings);
            }

            ////stopLoss
            //StrategiesPlaceStopLossByPointsOnTradeHandlers stopLossOnTradeHandlers =
            //    new StrategiesPlaceStopLossByPointsOnTradeHandlers(TradingData.Instance,
            //        SignalQueue.Instance,
            //        DefaultLogger.Instance,
            //        AppSettings.GetValue<bool>("MeasureStopFromSignalPrice"));
            ////stopLoss
            //StrategiesStopLossByPointsOnTickHandlers stopLossOnTickHandlers =
            //    new StrategiesStopLossByPointsOnTickHandlers(TradingData.Instance,
            //        SignalQueue.Instance,
            //        DefaultLogger.Instance,
            //        AppSettings.GetValue<bool>("MeasureStopFromSignalPrice"));
            ////takeProfit
            //StrategiesPlaceTakeProfitByPointsOnTradeHandlers takeProfitOnTradeHandlers =
            //    new StrategiesPlaceTakeProfitByPointsOnTradeHandlers(TradingData.Instance,
            //        SignalQueue.Instance,
            //        DefaultLogger.Instance,
            //        AppSettings.GetValue<bool>("MeasureProfitFromSignalPrice"));
            ////takeProfit
            //StrategiesTakeProfitByPointsOnTickHandlers takeProfitOnTickHandlers =
            //    new StrategiesTakeProfitByPointsOnTickHandlers(TradingData.Instance,
            //        SignalQueue.Instance,
            //        DefaultLogger.Instance,
            //        AppSettings.GetValue<bool>("MeasureProfitFromSignalPrice"));

            SmartComHandlers.Instance.Add<_IStClient_DisconnectedEventHandler>(ScalperIsDisconnected);
            SmartComHandlers.Instance.Add<_IStClient_ConnectedEventHandler>(ScalperIsConnected);

            //BreakOutOnTick openHandler =
            //    new BreakOutOnTick(strategy,
            //        TradingData.Instance,
            //        SignalQueue.Instance,
            //        DefaultLogger.Instance);

            //ReversOnTick openHandler =
            //    new ReversOnTick(strategy,
            //        TradingData.Instance,
            //        SignalQueue.Instance,
            //        DefaultLogger.Instance);


            //UpdateBarsOnTick updateBarsHandler =
            //    new UpdateBarsOnTick(barSettings,
            //        new TimeTracker(),
            //        TradingData.Instance,
            //        DefaultLogger.Instance);

            MakeRangeBarsOnTick updateBarsHandler =
                new MakeRangeBarsOnTick(barSettings,
                    new TimeTracker(),
                    TradingData.Instance,
                    DefaultLogger.Instance);

            //отправляем бар через signalR
            SendItemOnBar barItemSender =
                new SendItemOnBar(barSettings,
                                  TradingData.Instance);
            barItemSender.AddItemHandler(TradeHubStarter.sendBar);
            barItemSender.AddItemHandler(ConsoleWriteLineBar);

            ReversOnBar reversHandler =
                new ReversOnBar(strategy,
                    TradingData.Instance,
                    SignalQueue.Instance,
                    DefaultLogger.Instance);
            reversHandler.AddMa1Handler(TradeHubStarter.sendIndicator1);
            reversHandler.AddMa2Handler(TradeHubStarter.sendIndicator2);


            //отправляем бар через signalR
            SendItemOnTrade tradeItemSender =
                new SendItemOnTrade(TradingData.Instance, DefaultLogger.Instance);
            tradeItemSender.AddItemHandler(TradeHubStarter.sendTrade);

            //отправляем ордер через signalR
            //SendItemOnOrder senderItemOrder =
            //    new SendItemOnOrder(TradingData.Instance.Get<ObservableQueue<Order>>());
            //senderItemOrder.AddedItemHandler(TradeHubStarter.sendOrder);

            //список доступных команд
            ConsoleWriteCommands();

            //AddStrategySubscriptions();
            {
                DefaultSubscriber.Instance.Portfolios.Add(strategy.Portfolio);
                DefaultSubscriber.Instance.BidsAndAsks.Add(strategy.Symbol);
                DefaultSubscriber.Instance.Ticks.Add(strategy.Symbol);            
            }

            TradeHubStarter tradeHubStarter = new TradeHubStarter();
            //StartServer();
            Task.Run(() => tradeHubStarter.StartServer());
            Console.WriteLine(String.Format("Starting server..."));

            if(AppSettings.GetValue<bool>("ConsoleReadKey"))
            {
                Console.WriteLine(String.Format("Press 's' to start..."));
                while (Console.ReadKey().KeyChar != 's')
                {
                    Console.WriteLine(String.Format("Press 's' to start..."));
                }
            }

            adapter.Start();

            while (true)
            {
                try
                {
                    string command = Console.ReadLine();

                    if (command == "x")
                    {
                        adapter.Stop();

                        ExportData<Order>(AppSettings.GetValue<bool>("ExportOrdersOnExit"));
                        ExportData<Trade>(AppSettings.GetValue<bool>("ExportTradesOnExit"));
                        ExportData<Bar>(AppSettings.GetValue<bool>("ExportBarsOnExit"));

                        break;
                    }

                    if (command == "p")
                    {
                        Console.Clear();

                        Console.WriteLine(String.Format("Реализованный профит и лосс составляет {0} пунктов",
                            TradingData.Instance.GetProfitAndLossPoints(strategy)));
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
                            ConsoleWriteLineBar(item);
                            //TradeHubStarter.sendBarString(item);
                            TradeHubStarter.sendBar(item);
                        }
                    }
                    if (command == "c")
                    {
                        //Console.Clear();
                        Console.WriteLine("clearChart");
                        TradeHubStarter.clearChart();
                    }
                    if (command == "h")
                    {
                        Console.Clear();
                        ConsoleWriteCommands();
                    }

                    if (command == "d")
                    {
                        TradeHubStarter.ConsoleWriteTime = !TradeHubStarter.ConsoleWriteTime;
                        Console.WriteLine(String.Format("ConsoleWriteTime {0}",
                                                        TradeHubStarter.ConsoleWriteTime));
                    }
                }
                catch (System.Runtime.InteropServices.COMException e)
                {
                    DefaultLogger.Instance.Log(e.Message);

                    adapter.Restart();
                }
            }
            if (tradeHubStarter.SignalR != null)
            {
                tradeHubStarter.SignalR.Dispose();
            }
        }

        private static void ConsoleWriteLineBar(Bar item)
        {
            Console.WriteLine(item.ToString());
        }

        private static void ConsoleWriteCommands()
        {
            Console.WriteLine("p - Реализованный профит и лосс");
            Console.WriteLine("b - TradingData.Instance.Get<IEnumerable<Bar>>()");
            Console.WriteLine("t - TradingData.Instance.Get<IEnumerable<Trade>>()");
            Console.WriteLine("d - TradeHubStarter.ConsoleWriteTime");
            Console.WriteLine("c - TradeHubStarter.clearChart()");
            Console.WriteLine("h - Help");
            Console.WriteLine("x - Stop");
        }

        private static void AddStrategySettings()
        {
            TradingData.Instance.Get<ICollection<Strategy>>().Add(strategy);
            TradingData.Instance.Get<ICollection<BarSettings>>().Add(barSettings);
            TradingData.Instance.Get<ICollection<ProfitPointsSettings>>().Add(ppSettings);
            TradingData.Instance.Get<ICollection<TakeProfitOrderSettings>>().Add(poSettings);
            TradingData.Instance.Get<ICollection<StopPointsSettings>>().Add(spSettings);
            TradingData.Instance.Get<ICollection<StopLossOrderSettings>>().Add(soSettings);
        }

        private static void AddStrategySubscriptions()
        {
            DefaultSubscriber.Instance.Portfolios.Add(strategy.Portfolio);
            DefaultSubscriber.Instance.BidsAndAsks.Add(strategy.Symbol);
            DefaultSubscriber.Instance.Ticks.Add(strategy.Symbol);
        }

        private static void ScalperIsDisconnected(string reason)
        {
            DefaultLogger.Instance.Log("Cleaning Bar collection.");
            Console.WriteLine("Cleaning Bar collection.");
            TradingData.Instance.Get<ICollection<Bar>>().Clear();
        }

        private static void ScalperIsConnected()
        {
            DefaultLogger.Instance.Log("Requesting history bars.");
            Console.WriteLine(String.Format("Requesting history bars Symbol: {0} Interval: {1} Period: {2}", barSettings.Symbol, barSettings.Interval, barSettings.Period));
            ITransaction getBars = new GetBarsCommand(barSettings.Symbol, barSettings.Interval, barSettings.Period);
            getBars.Execute();
        }

        private static void LogAssemblyInfo()
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
                    logger.Log(item.ToFinamString());
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
