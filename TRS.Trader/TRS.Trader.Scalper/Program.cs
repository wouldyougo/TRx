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
using TRL.Common;
using TRL.Common.Collections;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Models;
using TRL.Connect.Smartcom;
using TRL.Connect.Smartcom.Data;
using TRL.Handlers.Cancel;

namespace TRx.Trader.Scalper
{
    /*
    class Program
    {
        private static MarketDataProvider marketDataProvider = new MarketDataProvider();
        private static RawTradingDataProvider rawTradingDataProvider = new RawTradingDataProvider();
        private static SymbolsDataProvider symbolsDataProvider = new SymbolsDataProvider();

        private static IOrderManager orderManager = new BacktestOrderManager(TradingData.Instance, DefaultLogger.Instance);
        
        private static TraderBase traderBase =
            new TraderBase(orderManager);

        //private static Strategy strategyHeader = new Strategy(1, "Sample strategyHeader", "ST46520-RF-01", "RIH4", 1);
        private static Strategy strategyHeader = new Strategy(1, "Sample strategyHeader", "ST46520-RF-01", "SPFB.RTS-3.14", 1);

        static void Main(string[] args)
        {
            TradingData.Instance.Get<ICollection<Strategy>>().Add(strategyHeader);

            //BarSettings barSettings = new BarSettings(strategyHeader, "RIH4", 3600, 3);
            BarSettings barSettings = new BarSettings(strategyHeader, "SPFB.RTS-3.14", 3600, 3);
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
            while (true)
            {
                try
                {
                    string command = Console.ReadLine();

                    if (command == "stop")
                    {

                        break;
                    }

                    if (command == "pnl")
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
*/
    class Program
    {
        private static MarketDataProvider marketDataProvider = new MarketDataProvider();
        private static RawTradingDataProvider rawTradingDataProvider = new RawTradingDataProvider();
        private static SymbolsDataProvider symbolsDataProvider = new SymbolsDataProvider();

        //private static OrderManager orderManager = new BacktestOrderManager(TradingData.Instance, DefaultLogger.Instance);

        private static TraderBase traderBase =
            new TraderBase(new SmartComOrderManager());
            //new TraderBase(new SmartComOrderManager(),
            //new TraderBase(orderManager,
            //    AppSettings.GetValue<bool>("MeasureStopFromSignalPrice"),
            //    AppSettings.GetValue<bool>("MeasureProfitFromSignalPrice"));

        private static SmartComAdapter adapter = new SmartComAdapter();
        private static StrategyHeader strategyHeader = new StrategyHeader(1, "Sample strategyHeader", "ST46520-RF-01", "RTS-3.14_FT", 1);
        //private static Strategy strategyHeader = new Strategy(1, "Sample strategyHeader", "ST46520-RF-01", "RIH4", 1);

        static void Main(string[] args)
        {
            AddStrategySettings();
/*
            TradingData.Instance.Get<ICollection<Strategy>>().Add(strategyHeader);

            BarSettings barSettings = new BarSettings(strategyHeader, "RIH4", 3600, 3);
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
*/
            AddStrategyHandlers();
/*            TakeProfitOnBar takeProfitOnBar =
                new TakeProfitOnBar(strategyHeader,
                    100,
                    TradingData.Instance,
                    SignalQueue.Instance,
                    DefaultLogger.Instance);
*/
            AddStrategySubscriptions();

            adapter.Start();
/*            Transaction importBars =
                new ImportBarsTransaction(TradingData.Instance.Get<ObservableCollection<Bar>>(),
                    "bars.txt");

            importBars.Execute();
*/			
            while (true)
            {
                try
                {
                    string command = Console.ReadLine();

                    if (command == "stop")
                    {
                        adapter.Stop();
                        break;
                    }

                    if (command == "pnl")
                    {
                        Console.WriteLine(String.Format("Реализованный профит и лосс составляет {0} пунктов",
                            TradingData.Instance.GetProfitAndLossPoints(strategyHeader)));
                    }
                }
                catch (System.Runtime.InteropServices.COMException e)
                {
                    DefaultLogger.Instance.Log(e.Message);
                    //adapter.Restart();
                }
            }
        }

        private static void ScalperIsConnected()
        {
            Console.WriteLine("Соединение установлено");
        }

        private static void ScalperDisconnected(string reason)
        {
            DefaultLogger.Instance.Log(reason);
        }

        private static void AddStrategySubscriptions()
        {
            DefaultSubscriber.Instance.Portfolios.Add(strategyHeader.Portfolio);
            DefaultSubscriber.Instance.BidsAndAsks.Add(strategyHeader.Symbol);
            DefaultSubscriber.Instance.Ticks.Add(strategyHeader.Symbol);
        }

        private static void AddStrategyHandlers()
        {
            OpenPositionOnOrderBookChange openPositionHandler =
                new OpenPositionOnOrderBookChange(strategyHeader,
                    OrderBook.Instance,
                    SignalQueue.Instance,
                    TradingData.Instance,
                    DefaultLogger.Instance);

            MakeOrderCancellationRequestOnQuote cancalOpenOrder =
                new MakeOrderCancellationRequestOnQuote(strategyHeader,
                    AppSettings.GetValue<double>("PriceShift"));
        }

        private static void AddStrategySettings()
        {
            TradingData.Instance.Make<StrategyHeader>().Add(strategyHeader);

            StopPointsSettings spSettings = new StopPointsSettings(strategyHeader, 50, false);
            TradingData.Instance.Make<StopPointsSettings>().Add(spSettings);

            ProfitPointsSettings ppSettings = new ProfitPointsSettings(strategyHeader, 10, false);
            TradingData.Instance.Make<ProfitPointsSettings>().Add(ppSettings);

            StopLossOrderSettings sloSettings = new StopLossOrderSettings(strategyHeader, 3600);
            TradingData.Instance.Make<StopLossOrderSettings>().Add(sloSettings);

            TakeProfitOrderSettings tpoSettings = new TakeProfitOrderSettings(strategyHeader, 3600);
            TradingData.Instance.Make<TakeProfitOrderSettings>().Add(tpoSettings);

            OrderSettings oSettings = new OrderSettings(strategyHeader, 3600);
            TradingData.Instance.Make<OrderSettings>().Add(oSettings);
        }
    }
}