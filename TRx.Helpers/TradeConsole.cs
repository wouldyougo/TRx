using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
//using System.Reflection;
//using System.Threading.Tasks;
//using Microsoft.AspNet.SignalR;
//using Microsoft.Owin.Hosting;
//using SmartCOM3Lib;

using TRL.Logging;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Models;
using TRL.Common.Collections;
using TRL.Common.Statistics;
using TRL.Transaction;
//using TRL.Connect.Smartcom.Commands;
//using TRL.Common;
//using TRL.Common.Extensions.Data;
//using TRL.Common.Handlers;
//using TRL.Common.TimeHelpers;
//using TRL.Connect.Smartcom;
//using TRL.Connect.Smartcom.Commands;
//using TRL.Connect.Smartcom.Data;
//using TRL.Handlers.Inputs;
//using TRL.Connect.Smartcom.Handlers;

namespace TRx.Helpers
{
    public class TradeConsole
    {
        /// <summary>
        /// устаревший метод
        /// </summary>
        public static void WaitStart()
        {
            Console.WriteLine(String.Format("Press 's' to start program..."));
            while (Console.ReadKey().KeyChar != 's')
            {
                Console.WriteLine(String.Format("Press 's' to start program..."));
            }
        }

        /// <summary>
        /// устаревший метод
        /// </summary>
        public static void WaitExit()
        {
            Console.WriteLine(String.Format("Press 'x' to exit..."));
            while (Console.ReadKey().KeyChar != 'x')
            {
                Console.WriteLine(String.Format("Press 'x' to exit..."));
            }
        }

        public static void ConsoleWriteLineBar(Bar item)
        {
            Console.WriteLine(item.ToString());
        }

        public static void ConsoleWriteLineOrder(Order item)
        {
            Console.WriteLine(item.ToString());
        }

        public static void ConsoleWriteLineSignal(Signal item)
        {
            Console.WriteLine(item.ToString());
        }

        public static void ConsoleWriteLineTrade(Trade item)
        {
            Console.WriteLine(item.ToString());
        }
        public static void ConsoleWriteLineDouble(double item)
        {
            Console.WriteLine(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сигнал Double", DateTime.Now, item.ToString("0.0000")));
        }
        public static void ConsoleWriteLineValueDouble(ValueDouble item)
        {
            Console.WriteLine(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сигнал {2}", item.DateTime, item.Value.ToString("0.0000"), item.Name));
        }
        public static void ConsoleWriteLineValueBool(ValueBool item)
        {
            Console.WriteLine(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сигнал {2}", item.DateTime, item.Value, item.Name));
        }
        /// <summary>
        /// устаревший метод
        /// </summary>
        public static void ConsoleWriteCommands()
        {
            /// список команд
            Console.WriteLine("p - Реализованный профит и лосс");
            Console.WriteLine("b - TradingData.Instance.Get<IEnumerable<Bar>>()");
            Console.WriteLine("t - TradingData.Instance.Get<IEnumerable<Trade>>()");
            Console.WriteLine("d - TradeHubStarter.ConsoleWriteTime");
            Console.WriteLine("c - TradeHubStarter.clearChart()");
            Console.WriteLine("s - Statistics");
            Console.WriteLine("h - Help");
            Console.WriteLine("x - Stop");
        }

        //ITransaction importBars =
        //    new ImportBarsTransaction(TradingData.Instance.Get<ObservableCollection<Bar>>(),
        //        "bars.txt");
        //importBars.Execute();
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

        public static void ConsoleSetSize()
        {
            try
            {
                Console.WindowTop = 0;
                Console.WindowLeft = 0;

                Console.WindowHeight = Console.LargestWindowHeight / 2;
                Console.WindowWidth = Console.LargestWindowWidth / 2;

                Console.BufferHeight = 9000;
                Console.BufferWidth = Console.LargestWindowWidth;

            }
            catch (Exception e)
            {
                Console.WriteLine("Console Exception: {0}", e);
            }
            finally
            {
                Console.WriteLine("Console.BufferHeight: {0}", Console.BufferHeight);
                Console.WriteLine("Console.BufferWidth: {0}", Console.BufferWidth);
                Console.WriteLine("Console.WindowHeight: {0}", Console.WindowHeight);
                Console.WriteLine("Console.WindowWidth: {0}", Console.WindowWidth);
                Console.WriteLine("Console.WindowTop: {0}", Console.WindowTop);
                Console.WriteLine("Console.WindowLeft: {0}", Console.WindowLeft);
            }
        }

        public static void GetBuySellTrades(StrategyHeader strategyHeader)
        {
            IEnumerable<Trade> BuyTrades = TradingData.Instance.GetBuyTrades(strategyHeader);
            IEnumerable<Trade> SellTrades = TradingData.Instance.GetSellTrades(strategyHeader);

            List<Trade> BuyTrades1 = new List<Trade>();
            foreach (Trade item in BuyTrades.OrderBy(i => i.DateTime))
            {
                //Console.WriteLine(item.ToString());
                for (int i = 0; i < item.Amount; i++)
                {
                    Trade trade = new Trade(item);
                    trade.Amount = 1;
                    BuyTrades1.Add(trade);
                    //Console.WriteLine(trade.ToString());
                }
            }
            List<Trade> SellTrades1 = new List<Trade>();
            foreach (Trade item in SellTrades.OrderBy(i => i.DateTime))
            {
                //Console.WriteLine(item.ToString());
                for (int i = 0; i < Math.Abs(item.Amount); i++)
                {
                    Trade trade = new Trade(item);
                    trade.Amount = -1;
                    SellTrades1.Add(trade);
                    //Console.WriteLine(trade.ToString());
                }
            }

            Console.WriteLine(String.Format("BuyTrades.Sum: {0}", BuyTrades.Sum(i => i.Amount).ToString()));
            Console.WriteLine(String.Format("BuyTrades1.Sum: {0}", BuyTrades1.Sum(i => i.Amount).ToString()));
            Console.WriteLine(String.Format("BuyTrades.Count: {0}", BuyTrades.Count().ToString()));
            Console.WriteLine(String.Format("BuyTrades1.Count: {0}", BuyTrades1.Count().ToString()));

            Console.WriteLine(String.Format("SellTrades.Sum: {0}", SellTrades.Sum(i => i.Amount).ToString()));
            Console.WriteLine(String.Format("SellTrades1.Sum: {0}", SellTrades1.Sum(i => i.Amount).ToString()));
            Console.WriteLine(String.Format("SellTrades.Count: {0}", SellTrades.Count().ToString()));
            Console.WriteLine(String.Format("SellTrades1.Count: {0}", SellTrades1.Count().ToString()));
        }
        public static DealList GetDeals(StrategyHeader strategyHeader)
        {
            if (strategyHeader == null) {
                return null;
            }
            IEnumerable<Trade> Trades = TradingData.Instance.GetTrades(strategyHeader);
            DealList dealList = new DealList(strategyHeader);
            Console.WriteLine("GetDeals");
            foreach (Trade item in Trades.OrderBy(i => i.DateTime))
            {
                dealList.OnItemAdded(item);
            }
            Console.WriteLine("Количество сделок:   {0}", dealList.Count);
            Console.WriteLine("Количество сделок P: {0}", dealList.CountP);
            Console.WriteLine("Количество сделок L: {0}", dealList.CountL);
            Console.WriteLine("Процент сделок P:    {0}", dealList.CountPercentP.ToString("0.0000"));
            Console.WriteLine("Процент сделок L:    {0}", dealList.CountPercentL.ToString("0.0000"));
            Console.WriteLine("Сумма PnL:   {0}", dealList.Sum);
            Console.WriteLine("Сумма P:     {0}", dealList.SumP);
            Console.WriteLine("Сумма L:     {0}", dealList.SumL);
            Console.WriteLine("Средняя Сумма PnL:   {0}", dealList.AverageSum.ToString("0.0000"));
            Console.WriteLine("Средняя Сумма P:     {0}", dealList.AverageSumP.ToString("0.0000"));
            Console.WriteLine("Средняя Сумма L:     {0}", dealList.AverageSumL.ToString("0.0000"));

            Console.WriteLine("Средняя Серия P:     {0}", dealList.AverageSeriesP.ToString("0.0000"));
            Console.WriteLine("Средняя Серия L:     {0}", dealList.AverageSeriesL.ToString("0.0000"));
            Console.WriteLine("Max Серия P:         {0}", dealList.MaxSeriesP.ToString("0.0000"));
            Console.WriteLine("Max Серия L:         {0}", dealList.MaxSeriesL.ToString("0.0000"));

            return dealList;
            //Console.WriteLine(String.Format("BuyTrades.Sum: {0}", BuyTrades.Sum(i => i.Amount).ToString()));
            //Console.WriteLine(String.Format("BuyTrades1.Sum: {0}", BuyTrades1.Sum(i => i.Amount).ToString()));
            //Console.WriteLine(String.Format("BuyTrades.Count: {0}", BuyTrades.Count().ToString()));
            //Console.WriteLine(String.Format("BuyTrades1.Count: {0}", BuyTrades1.Count().ToString()));

            //Console.WriteLine(String.Format("SellTrades.Sum: {0}", SellTrades.Sum(i => i.Amount).ToString()));
            //Console.WriteLine(String.Format("SellTrades1.Sum: {0}", SellTrades1.Sum(i => i.Amount).ToString()));
            //Console.WriteLine(String.Format("SellTrades.Count: {0}", SellTrades.Count().ToString()));
            //Console.WriteLine(String.Format("SellTrades1.Count: {0}", SellTrades1.Count().ToString()));
        }

        public static void ConsoleWriteDealList(DealList dealList)
        {
            if (dealList == null)
            {
                return;
            }
            foreach (Deal item in dealList.Deals)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }
}
