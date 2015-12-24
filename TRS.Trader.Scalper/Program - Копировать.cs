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

namespace TRL.Trader.Scalper
{
    class Program
    {
        private static MarketDataProvider marketDataProvider = new MarketDataProvider();
        private static RawTradingDataProvider rawTradingDataProvider = new RawTradingDataProvider();
        private static SymbolsDataProvider symbolsDataProvider = new SymbolsDataProvider();

        private static IOrderManager orderManager = new BacktestOrderManager(TradingData.Instance, DefaultLogger.Instance);
        
        private static TraderBase traderBase =
            new TraderBase(orderManager);

        //private static Strategy strategy = new Strategy(1, "Sample strategy", "ST46520-RF-01", "RIH4", 1);
        private static Strategy strategy = new Strategy(1, "Sample strategy", "ST46520-RF-01", "SPFB.RTS-3.14", 1);

        static void Main(string[] args)
        {
            TradingData.Instance.Get<ICollection<Strategy>>().Add(strategy);

            //BarSettings barSettings = new BarSettings(strategy, "RIH4", 3600, 3);
            BarSettings barSettings = new BarSettings(strategy, "SPFB.RTS-3.14", 3600, 3);
            TradingData.Instance.Get<ICollection<BarSettings>>().Add(barSettings);

            BreakOutOnBar breakOnBar =
                new BreakOutOnBar(strategy,
                    TradingData.Instance,
                    SignalQueue.Instance,
                    DefaultLogger.Instance);

            StopLossOnBar stopLossOnBar =
                new StopLossOnBar(strategy,
                    100,
                    TradingData.Instance,
                    SignalQueue.Instance,
                    DefaultLogger.Instance);

            TakeProfitOnBar takeProfitOnBar =
                new TakeProfitOnBar(strategy,
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
                            TradingData.Instance.GetProfitAndLossPoints(strategy)));
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
