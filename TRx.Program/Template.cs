using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
//using Microsoft.AspNet.SignalR;
//using Microsoft.Owin.Hosting;

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
using TRL.Transaction;
//using TRL.Connect.Smartcom.Handlers;
//using TRL.Handlers.Spreads;
//using TRL.Handlers.StopLoss;
//using TRL.Handlers.TakeProfit;
//using TRL.Handlers.Inputs;
using TRx.Handlers;
using TRx.Helpers;
using TRL.Common.Statistics;
using TRx.Base;

namespace TRx.Program
{
    /// <summary>
    /// шаблон консольного приложения 
    /// </summary>
    public class Template : TRx.Base.Template
    {
        //private static MakeRangeBarsOnTick updateBarsHandler { get; set; }
        //private static IndicatorOnBar2Ma indicatorsOnBar { get; set; }

        /// <summary>
        /// отправляем Trade клиентам 
        /// </summary>
        public SendItemOnTrade sendItemTrade { get; set; }
            //= new SendItemOnTrade(TradingData.Instance, DefaultLogger.Instance);
        /// <summary>
        /// отправляем Order клиентам 
        // </summary>
        public SendItemOnOrder sendItemOrder { get; set; }
        //= new SendItemOnOrder(TradingData.Instance.Get<ObservableQueue<Order>>());

        public SmartComAdapter adapter { get; set; }

        //public static string[] assemblies = { "Interop.SmartCOM3Lib.dll", "TRL.Common.dll", "TRL.Connect.Smartcom.dll" };

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main(string[] args)
        {
            Template tmp = new Template(args);
            tmp.Do();
        }
        public Template(string[] args) : base(args)
        {
            Console.WriteLine("Program.Template()");
            Initialize();
            SetupStrategy(args);
        }

        #region // переопределение базовых методов
        /// <summary>
        /// Program Template Do()
        /// </summary>
        /// <param name="args"></param>
        override public void Do()
        {
            Console.WriteLine("Program.Template.Do()");
            //TradeConsole.LogAssemblyInfo(assemblies);
            //Initialize();
            //SetupStrategy(args);
            base.Do();
                //Template.MethodServerStart();
                //////ImportTicksTransaction(args);
                //MethodWhile();
                //Template.MethodServerDispose();
        }
        /// <summary>
        /// Program Template Initialize()
        /// </summary>
        override public void Initialize()
        {
            Console.WriteLine("Program.Template.Initialize()");
            marketDataProvider = new MarketDataProvider();
            rawTradingDataProvider = new RawTradingDataProvider(DefaultLogger.Instance);
            symbolsDataProvider = new SymbolsDataProvider();
            traderBase = new TraderBase(new SmartComOrderManager());
            adapter = new SmartComAdapter();

            SmartComHandlers.Instance.Disconnected += IsDisconnected;
            //SmartComHandlers.Instance.Add<_IStClient_DisconnectedEventHandler>(IsDisconnected);
            SmartComHandlers.Instance.Add<_IStClient_ConnectedEventHandler>(IsConnected);

            sendItemTrade = new SendItemOnTrade(TradingData.Instance, DefaultLogger.Instance);
            //ObservableQueue<Order> orderQueue = TradingData.Instance.Get<ObservableQueue<Order>>();
            //ObservableHashSet<Order> orderQueue = TradingData.Instance.Get<ObservableHashSet<Order>>();

            //sendItemOrder = new SendItemOnOrder(TradingData.Instance.Get<ObservableQueue<Order>>());
            //Отправляем данные клиентам
            {
                //sendItemBar.AddItemHandler(TradeConsole.ConsoleWriteLineBar);
                sendItemTrade.AddItemHandler(TradeConsole.ConsoleWriteLineTrade);
                //senderItemOrder.AddedItemHandler(TradeHubStarter.sendOrder);

                if (AppSettings.GetValue<bool>("SignalHub"))
                {
                    //отправляем через signalR
                    //sendItemBar.AddItemHandler(TradeHubStarter.sendBar);
                    sendItemTrade.AddItemHandler(TradeHubStarter.sendTrade);
                }
            }
        }
        /// <summary>
        /// пример
        /// </summary>
        /// <param name="args"></param>
        override public void SetupStrategy(string[] args)
        {
            // инициализация обработчиков стратегии
            Console.WriteLine("Fake Program.Template.SetupStrategy()");
        }
        #endregion //

        #region // переопределение базовых методов
        /// <summary>
        /// переопределение базового метода
        /// adapter.Start();
        /// </summary>
        override public void MethodAdapterStart()
        {
            Console.WriteLine("SmartCom Adapter.Start()");
            adapter.Start();
        }
        /// <summary>
        /// переопределение базового метода
        /// adapter.Stop();
        /// </summary>
        override public void MethodAdapterStop()
        {
            Console.WriteLine("SmartCom Adapter.Stop()");
            adapter.Stop();
        }
        /// <summary>
        /// переопределение базового метода
        /// adapter.Restart();
        /// </summary>
        override public void MethodAdapterRestart()
        {
            Console.WriteLine("SmartCom Adapter.Restart()");
            adapter.Restart();
        }

        override public void IsDisconnected(string reason)
        {
            Console.WriteLine("Template IsDisconnected");
            //DefaultLogger.Instance.Log("Cleaning Bar collection.");
            //Console.WriteLine("Cleaning Bar collection.");
            //TradingData.Instance.Get<ICollection<Bar>>().Clear();
        }

        #endregion //

        #region // переопределение консольных команд
        #endregion //

        public void getBars(string reason)
        {
            //ITransaction getBars = new GetBarsCommand(barSettings.Symbol, barSettings.Interval, 3);
            //getBars.Execute();
        }
    }
}