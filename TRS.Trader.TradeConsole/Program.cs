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
using TRL.Handlers.Inputs;
using TRL.Transaction;
//using TRL.Connect.Smartcom.Handlers;
//using TRL.Handlers.Spreads;
//using TRL.Handlers.StopLoss;
//using TRL.Handlers.TakeProfit;
using TRx.Handlers;
using TRx.Helpers;

namespace TRx.Trader.TradeConsole
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
        
        private static StrategyHeader strategyHeader = 
            new StrategyHeader(1, 
                "Proboy take profit and stop loss points strategyHeader",
                AppSettings.GetStringValue("Portfolio"),
                AppSettings.GetStringValue("Symbol"),
                AppSettings.GetValue<double>("Amount"));
        
        private static BarSettings barSettings = new BarSettings(strategyHeader,
            strategyHeader.Symbol,
            AppSettings.GetValue<int>("Interval"),
            AppSettings.GetValue<int>("Period"));

        private static ProfitPointsSettings ppSettings =
            new ProfitPointsSettings(strategyHeader, AppSettings.GetValue<double>("ProfitPoints"), false);

        private static TakeProfitOrderSettings poSettings =
            new TakeProfitOrderSettings(strategyHeader, 86400);

        private static StopPointsSettings spSettings =
            new StopPointsSettings(strategyHeader, AppSettings.GetValue<double>("StopPoints"), false);

        private static StopLossOrderSettings soSettings =
            new StopLossOrderSettings(strategyHeader, 86400);

        private static string[] assemblies = { "Interop.SmartCOM3Lib.dll", "TRL.Common.dll", "TRL.Connect.Smartcom.dll", "TRS.Trader.TradeConsole.exe" };

        static void Main(string[] args)
        {
            TRx.Helpers.TradeConsole.ConsoleSetSize();
            TRx.Helpers.Export.LogAssemblyInfo(assemblies);

            //AddStrategySettings();
            {
                TradingData.Instance.Get<ICollection<StrategyHeader>>().Add(strategyHeader);
                TradingData.Instance.Get<ICollection<BarSettings>>().Add(barSettings);
                TradingData.Instance.Get<ICollection<ProfitPointsSettings>>().Add(ppSettings);
                TradingData.Instance.Get<ICollection<TakeProfitOrderSettings>>().Add(poSettings);
                TradingData.Instance.Get<ICollection<StopPointsSettings>>().Add(spSettings);
                TradingData.Instance.Get<ICollection<StopLossOrderSettings>>().Add(soSettings);

                //SMASettings smaSettings = new SMASettings(strategyHeader, 7, 14);
                int maf = AppSettings.GetValue<int>("MaFast");
                int mas = AppSettings.GetValue<int>("MaSlow");
                SMASettings smaSettings = new SMASettings(strategyHeader, maf, mas);
                TradingData.Instance.Get<ICollection<SMASettings>>().Add(smaSettings);
            }

            SmartComHandlers.Instance.Add<_IStClient_DisconnectedEventHandler>(IsDisconnected);
            SmartComHandlers.Instance.Add<_IStClient_ConnectedEventHandler>(IsConnected);

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
            barItemSender.AddItemHandler(TRx.Helpers.TradeConsole.ConsoleWriteLineBar);

            ReversOnBar1 reversHandler =
                new ReversOnBar1(strategyHeader,
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

            //AddStrategySubscriptions();
            {
                DefaultSubscriber.Instance.Portfolios.Add(strategyHeader.Portfolio);
                DefaultSubscriber.Instance.BidsAndAsks.Add(strategyHeader.Symbol);
                DefaultSubscriber.Instance.Ticks.Add(strategyHeader.Symbol);            
            }

            //список доступных команд
            TRx.Helpers.TradeConsole.ConsoleWriteCommands();


            TradeHubStarter tradeHubStarter = new TradeHubStarter();
            if (AppSettings.GetValue<bool>("SignalHub"))
            {
                Task.Run(() => tradeHubStarter.StartServer());
                Console.WriteLine(String.Format("Starting server..."));
            }

            if (AppSettings.GetValue<bool>("ConsoleWaitStart"))
            {
                TRx.Helpers.TradeConsole.WaitStart();
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

                        TRx.Helpers.Export.ExportData<Order>(AppSettings.GetValue<bool>("ExportOrdersOnExit"));
                        TRx.Helpers.Export.ExportData<Trade>(AppSettings.GetValue<bool>("ExportTradesOnExit"));
                        TRx.Helpers.Export.ExportData<Bar>(AppSettings.GetValue<bool>("ExportBarsOnExit"));

                        break;
                    }

                    if (command == "p")
                    {
                        Console.Clear();

                        Console.WriteLine(String.Format("Реализованный профит и лосс составляет {0} пунктов",
                            TradingData.Instance.GetProfitAndLossPoints(strategyHeader)));
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
                            TRx.Helpers.TradeConsole.ConsoleWriteLineBar(item);
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
                        TRx.Helpers.TradeConsole.ConsoleWriteCommands();
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

        private static void AddStrategySettings()
        {
            TradingData.Instance.Get<ICollection<StrategyHeader>>().Add(strategyHeader);
            TradingData.Instance.Get<ICollection<BarSettings>>().Add(barSettings);
            TradingData.Instance.Get<ICollection<ProfitPointsSettings>>().Add(ppSettings);
            TradingData.Instance.Get<ICollection<TakeProfitOrderSettings>>().Add(poSettings);
            TradingData.Instance.Get<ICollection<StopPointsSettings>>().Add(spSettings);
            TradingData.Instance.Get<ICollection<StopLossOrderSettings>>().Add(soSettings);
        }

        private static void AddStrategySubscriptions()
        {
            DefaultSubscriber.Instance.Portfolios.Add(strategyHeader.Portfolio);
            DefaultSubscriber.Instance.BidsAndAsks.Add(strategyHeader.Symbol);
            DefaultSubscriber.Instance.Ticks.Add(strategyHeader.Symbol);
        }
        public static void IsConnected()
        {
            DefaultLogger.Instance.Log("Requesting history bars.");
            Console.WriteLine(String.Format("Requesting history bars Symbol: {0} Interval: {1} Period: {2}", barSettings.Symbol, barSettings.Interval, barSettings.Period));
            ITransaction getBars = new GetBarsCommand(barSettings.Symbol, barSettings.Interval, barSettings.Period);
            getBars.Execute();
        }
        public static void IsDisconnected(string reason)
        {
            DefaultLogger.Instance.Log("Cleaning Bar collection.");
            Console.WriteLine("Cleaning Bar collection.");
            TradingData.Instance.Get<ICollection<Bar>>().Clear();
        }
    }
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

//BreakOutOnTick openHandler =
//    new BreakOutOnTick(strategyHeader,
//        TradingData.Instance,
//        SignalQueue.Instance,
//        DefaultLogger.Instance);

//ReversOnTick openHandler =
//    new ReversOnTick(strategyHeader,
//        TradingData.Instance,
//        SignalQueue.Instance,
//        DefaultLogger.Instance);


//UpdateBarsOnTick updateBarsHandler =
//    new UpdateBarsOnTick(barSettings,
//        new TimeTracker(),
//        TradingData.Instance,
//        DefaultLogger.Instance);