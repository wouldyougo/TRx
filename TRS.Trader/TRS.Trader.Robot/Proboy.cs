using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
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

namespace TRx.Trader.Robot
{
    class Proboy
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

        private static string[] assemblies = { "Interop.SmartCOM3Lib.dll", "TRL.Common.dll", "TRL.Connect.Smartcom.dll", "TRS.Trader.Robot.exe" };

        static void Main(string[] args)
        {
            LogAssemblyInfo();

            AddStrategySettings();

            StrategiesPlaceStopLossByPointsOnTradeHandlers stopLossOnTradeHandlers =
                new StrategiesPlaceStopLossByPointsOnTradeHandlers(TradingData.Instance, 
                    SignalQueue.Instance, 
                    DefaultLogger.Instance, 
                    AppSettings.GetValue<bool>("MeasureStopFromSignalPrice"));

            StrategiesStopLossByPointsOnTickHandlers stopLossOnTickHandlers =
                new StrategiesStopLossByPointsOnTickHandlers(TradingData.Instance,
                    SignalQueue.Instance,
                    DefaultLogger.Instance,
                    AppSettings.GetValue<bool>("MeasureStopFromSignalPrice"));

            StrategiesPlaceTakeProfitByPointsOnTradeHandlers takeProfitOnTradeHandlers =
                new StrategiesPlaceTakeProfitByPointsOnTradeHandlers(TradingData.Instance,
                    SignalQueue.Instance,
                    DefaultLogger.Instance,
                    AppSettings.GetValue<bool>("MeasureProfitFromSignalPrice"));

            StrategiesTakeProfitByPointsOnTickHandlers takeProfitOnTickHandlers =
                new StrategiesTakeProfitByPointsOnTickHandlers(TradingData.Instance,
                    SignalQueue.Instance,
                    DefaultLogger.Instance,
                    AppSettings.GetValue<bool>("MeasureProfitFromSignalPrice"));
               
            SmartComHandlers.Instance.Add<_IStClient_DisconnectedEventHandler>(ScalperIsDisconnected);
            SmartComHandlers.Instance.Add<_IStClient_ConnectedEventHandler>(ScalperIsConnected);

            BreakOutOnTick openHandler =
                new BreakOutOnTick(strategyHeader,
                    TradingData.Instance,
                    SignalQueue.Instance,
                    DefaultLogger.Instance);

            UpdateBarsOnTick updateBarsHandler =
                new UpdateBarsOnTick(barSettings,
                    new TimeTracker(),
                    TradingData.Instance,
                    DefaultLogger.Instance);

            AddStrategySubscriptions();

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

                        foreach (Bar item in TradingData.Instance.Get<IEnumerable<Bar>>())
                            Console.WriteLine(item.ToString());
                    }

                }
                catch (System.Runtime.InteropServices.COMException e)
                {
                    DefaultLogger.Instance.Log(e.Message);

                    adapter.Restart();
                }
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

        private static void ScalperIsDisconnected(string reason)
        {
            DefaultLogger.Instance.Log("Cleaning Bar collection.");
            TradingData.Instance.Get<ICollection<Bar>>().Clear();
        }

        private static void ScalperIsConnected()
        {
            DefaultLogger.Instance.Log("Requesting history bars.");
            ITransaction getBars = new GetBarsCommand(barSettings.Symbol, barSettings.Interval, barSettings.Period);
            getBars.Execute();
        }

        private static void LogAssemblyInfo()
        {

            foreach (string assembly in assemblies) 
            { 
                DefaultLogger.Instance.Log(
                    String.Format("assembly: {0} version: {1}", 
                    assembly, 
                    FileVersionInfo.GetVersionInfo(assembly).ProductVersion));
            }
        }

        private static void ExportData<T>(bool confirmExport)
        {
            if (!confirmExport)
                return;

            string prefix = typeof(T).Name;

            ILogger logger = new TextFileLogger(prefix, 10000000);

            foreach (T item in TradingData.Instance.Get<IEnumerable<T>>())
                logger.Log(item.ToString());
        }
    }
}
