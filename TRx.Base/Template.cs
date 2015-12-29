using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Reflection;
//using System.Diagnostics;
using System.Threading.Tasks;
//using Microsoft.AspNet.SignalR;
//using Microsoft.Owin.Hosting;

//using SmartCOM3Lib;

using TRL.Logging;
using TRL.Common;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Models;
using TRL.Common.Handlers;
using TRL.Common.Collections;
//using TRL.Common.TimeHelpers;
//using TRL.Connect.Smartcom.Commands;
using TRL.Connect.Smartcom.Data;
//using TRL.Transaction;
//using TRL.Connect.Smartcom;
//using TRL.Connect.Smartcom.Handlers;
//using TRL.Handlers.Spreads;
//using TRL.Handlers.StopLoss;
//using TRL.Handlers.TakeProfit;
//using TRx.TestBack;
using TRx.Helpers;
//using TRx.Handlers;
using TRL.Common.Statistics;

//<add key="Symbol" value="Si-9.15_FT" />

namespace TRx.Base
{
    /// <summary>
    /// шаблон консольного приложения (базовый)
    /// </summary>
    public class Template: ISetupStrategy
    {
        #region //
        #endregion //

        #region // базовые сущности
        public MarketDataProvider marketDataProvider { get; set; }

        public RawTradingDataProvider rawTradingDataProvider { get; set; }

        public SymbolsDataProvider symbolsDataProvider { get; set; }

        public IOrderManager orderManager { get; set; }

        //private SmartComAdapter adapter =
        //    new SmartComAdapter();
        public TraderBase traderBase { get; set; }

        public StrategyHeader strategyHeader { get; set; }

        //public BarSettings barSettings { get; set; }

        public string[] assemblies = { "TRS.Base.dll" };
        //public string[] assemblies = { "Interop.SmartCOM3Lib.dll", "TRL.Common.dll", "TRL.Connect.Smartcom.dll" };
        #endregion //
        //private SendItemOnBar barItemSender;
        //private SendItemOnTrade tradeItemSender;
        //private SendItemOnOrder senderItemOrder;
        /*
        /// <summary>
        /// отправляем Bar клиентам 
        /// </summary>
        public static SendItemOnBar sendItemBar //{ get; set; }
            = new SendItemOnBar(barSettings, TradingData.Instance);
        /// <summary>
        /// отправляем Trade клиентам 
        /// </summary>
        public static SendItemOnTrade sendItemTrade //{ get; set; }
            = new SendItemOnTrade(TradingData.Instance, DefaultLogger.Instance);
        /// <summary>
        /// отправляем Order клиентам 
        // </summary>
        public static SendItemOnOrder sendItemOrder //{ get; set; }
            = new SendItemOnOrder(TradingData.Instance.Get<ObservableQueue<Order>>());
        */


        /// <summary>
        /// TradeHubStarter
        /// </summary>
        public static TradeHubStarter tradeHubStarter// { get; set; } 
            = new TradeHubStarter();

        public string[] args { get; set; }

        #region // базовые методы
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main(string[] args)
        {
            Template tmp = new Template(args);
            tmp.Do();
        }
        public Template(string[] args)

        {
            this.args = args;            
        }
        /// <summary>
        /// ISetupStrategy
        /// </summary>
        virtual public void Initialize()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// ISetupStrategy
        /// </summary>
        /// <param name="args"></param>
        virtual public void SetupStrategy(string[] args)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// tradeHubStarter.StartServer()
        /// </summary>
        public static void MethodSignalRServerStart()
        {
            //TradeHubStarter tradeHubStarter = new TradeHubStarter();
            if (AppSettings.GetValue<bool>("SignalHub"))
            {
                Task.Run(() => tradeHubStarter.StartServer());
                Console.WriteLine(String.Format("Starting SignalHub server..."));
            }
            else
            {
                Console.WriteLine(String.Format("SignalHub is off"));
            }
        }
        /// <summary>
        /// radeHubStarter.SignalR.Dispose()
        /// </summary>
        public static void MethodSignalRServerDispose()
        {
            if (tradeHubStarter.SignalR != null)
            {
                tradeHubStarter.SignalR.Dispose();
            }
        }

        /// <summary>
        /// StartSignalHub + ConsoleWhile()
        /// </summary>
        virtual public void Do()
        {
            TradeConsole.ConsoleSetSize();
            //TradeConsole.LogAssemblyInfo(assemblies);
            //////Setup();

            MethodHelp();

            Template.MethodSignalRServerStart();
            MethodWaitStart();
            ////ImportTicksTransaction(args);
            MethodPreStart(args);
            MethodWhile();
            Template.MethodSignalRServerDispose();
        }

        /// <summary>
        /// ConsoleWaitStart
        /// </summary>
        public void MethodWaitStart()
        {
            if (AppSettings.GetValue<bool>("ConsoleWaitStart"))
            {
                Console.WriteLine(String.Format("Press 's' to start program..."));
                //while (Console.ReadKey().KeyChar != 's')
                while (true)
                {
                    string command = Console.ReadLine();
                    if (command == "x")
                    {
                        //MethodAdapterStop();
                        MethodWaitExit();
                        //return;
                        System.Environment.Exit(0);
                    }
                    if (command == "s")
                    {
                        Console.WriteLine(String.Format("Program started..."));
                        break;
                    }
                    else
                    {
                        Console.WriteLine(String.Format("Press 's' to start program..."));
                    }
                }
            }
        }

        /// <summary>
        /// обработка консольного ввода команд пользователя
        /// </summary>
        public void MethodWhile()
        {
            MethodAdapterStart();

            while (true)
            {
                try
                {
                    string command = Console.ReadLine();
                    if (command == "x")
                    {
                        MethodAdapterStop();
                        ConsoleHandlerX();
                        break;
                    }
                    if (command == "s")
                    {   //статистика
                        ConsoleHandlerS();
                    }
                    if (command == "p")
                    {   //профит
                        ConsoleHandlerP();
                    }
                    if (command == "t")
                    {   //трейды
                        ConsoleHandlerT();
                    }
                    if (command == "b")
                    {   //бары
                        ConsoleHandlerB();
                    }
                    if (command == "c")
                    {   //очистить
                        ConsoleHandlerС();
                    }
                    if (command == "h")
                    {   //help
                        ConsoleHandlerH();
                    }
                    if (command == "time")
                    {   //отображать дату и время
                        ConsoleHandlerTime();
                    }
                }
                catch (System.Runtime.InteropServices.COMException e)
                {
                    DefaultLogger.Instance.Log(e.Message);
                    MethodAdapterRestart();
                }
            }
        }


        virtual public void MethodPreStart(string[] args)
        {
            Console.WriteLine("Fake.PreStart()");
        }

        /// <summary>
        /// Fake adapter.Start()
        /// </summary>
        virtual public void MethodAdapterStart()
        {
            //adapter.Start();
            Console.WriteLine("Fake.Adapter.Start()");
        }
        /// <summary>
        /// Fake adapter.Stop()
        /// </summary>
        virtual public void MethodAdapterStop()
        {
            //adapter.Stop();
            Console.WriteLine("Fake.Adapter.Stop()");
        }
        /// <summary>
        /// Fake adapter.Restart()
        /// </summary>
        virtual public void MethodAdapterRestart()
        {
            //adapter.Restart();
            Console.WriteLine("Fake.Adapter.Restart()");
        }

        virtual public void IsConnected()
        {
            DefaultLogger.Instance.Log("IsConnected.");
            Console.WriteLine("IsConnected.");
            //DefaultLogger.Instance.Log("Requesting history bars.");
            //Console.WriteLine(String.Format("Requesting history bars Symbol: {0} Interval: {1} Period: {2}", barSettings.Symbol, barSettings.Interval, barSettings.Period));
            //ITransaction getBars = new GetBarsCommand(barSettings.Symbol, barSettings.Interval, barSettings.Period);
            //getBars.Execute();
        }
        virtual public void IsDisconnected(string reason)
        {
            DefaultLogger.Instance.Log("IsDisconnected.");
            Console.WriteLine("IsDisconnected.");
            //DefaultLogger.Instance.Log("Cleaning Bar collection.");
            //Console.WriteLine("Cleaning Bar collection.");
            //TradingData.Instance.Get<ICollection<Bar>>().Clear();
        }
        #endregion //

        #region // базовые консольные команды
        /// <summary>
        /// Clear
        /// </summary>
        virtual public void ConsoleHandlerС()
        {
            Console.Clear();
            Console.WriteLine("Base.ConsoleHandlerС()");
            Console.WriteLine("ClearChart()");
            TradeHubStarter.clearChart();
        }

        /// <summary>
        /// Бары
        /// </summary>
        virtual public void ConsoleHandlerB()
        {
            Console.Clear();
            Console.WriteLine("Base.ConsoleHandlerB()");
            foreach (Bar item in TradingData.Instance.Get<IEnumerable<Bar>>().OrderBy(i => i.DateTime))
            //foreach (Bar item in TradingData.Instance.Get<IEnumerable<Bar>>())
            {
                TradeConsole.ConsoleWriteLineBar(item);
                //TradeHubStarter.sendBarString(item);
                TradeHubStarter.sendBar(item);
            }
        }

        /// <summary>
        /// Трейды
        /// </summary>
        virtual public void ConsoleHandlerT()
        {
            Console.Clear();
            Console.WriteLine("Base.ConsoleHandlerT()");
            foreach (Trade item in TradingData.Instance.Get<IEnumerable<Trade>>())
                Console.WriteLine(item.ToString());
        }

        /// <summary>
        /// Профит
        /// </summary>
        virtual public void ConsoleHandlerP()
        {
            Console.Clear();
            Console.WriteLine("Base.ConsoleHandlerP()");
            Console.WriteLine(String.Format("Реализованный профит и лосс составляет {0} пунктов",
                TradingData.Instance.GetProfitAndLossPoints(strategyHeader)));
        }

        /// <summary>
        /// Статистика
        /// </summary>
        virtual public void ConsoleHandlerS()
        {
            Console.Clear();
            Console.WriteLine("Base.ConsoleHandlerS()");
            TradeConsole.GetBuySellTrades(strategyHeader);
            var dealList = TradeConsole.GetDeals(strategyHeader);
            TradeConsole.ConsoleWriteDealList(dealList);
            //TradeConsole.ExportData<Deal>(dealList.Deals);
        }
        /// <summary>
        /// список доступных команд
        /// </summary>
        virtual public void MethodHelp()
        {
            Console.WriteLine("Base.Help()");
            Console.WriteLine("time - TradeHubStarter.ConsoleWriteTime");
            Console.WriteLine("p - Реализованный профит и лосс");
            Console.WriteLine("b - TradingData.Instance.Get<IEnumerable<Bar>>()");
            Console.WriteLine("t - TradingData.Instance.Get<IEnumerable<Trade>>()");
            Console.WriteLine("c - TradeHubStarter.clearChart()");
            Console.WriteLine("s - Statistics");
            Console.WriteLine("h - Help");
            Console.WriteLine("x - Stop");
        }

        /// <summary>
        /// Справка
        /// </summary>
        virtual public void ConsoleHandlerH()
        {
            Console.Clear();
            Console.WriteLine("Base.ConsoleHandlerH()");
            MethodHelp();
        }

        /// <summary>
        /// отображать дату и время
        /// </summary>
        virtual public void ConsoleHandlerTime()
        {
            //throw new NotImplementedException();
            TradeHubStarter.ConsoleWriteTime = !TradeHubStarter.ConsoleWriteTime;
            Console.WriteLine(String.Format("ConsoleWriteTime {0}",
                                            TradeHubStarter.ConsoleWriteTime));
        }

        /// <summary>
        /// Выход
        /// </summary>
        virtual public void ConsoleHandlerX()
        {
            Console.WriteLine("Base.ConsoleHandlerX()");
            Export.ExportData<Order>(AppSettings.GetValue<bool>("ExportOrdersOnExit"));
            Export.ExportData<Trade>(AppSettings.GetValue<bool>("ExportTradesOnExit"));
            Export.ExportData<Bar>(AppSettings.GetValue<bool>("ExportBarsOnExit"));
            ///переделать
            var dealList = TradeConsole.GetDeals(strategyHeader);
            if (dealList != null)
            {
                Export.ExportData<Deal>(dealList.Deals);
            }
            MethodWaitExit();
        }
        public static void MethodWaitExit()
        {
            Console.WriteLine(String.Format("Press 'x' to exit..."));
            while (Console.ReadKey().KeyChar != 'x')
            {
                Console.WriteLine(String.Format("Press 'x' to exit..."));
            }
        }
        #endregion //

        #region //пример
        #endregion //
    }
}