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
using TRL.Connect.Smartcom;
using TRL.Connect.Smartcom.Data;

using TRL.Logging;
using TRL.Common;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.Handlers;
using TRx.Helpers;
using TRx.Base;
//using TRL.Common.Extensions.Data;
//using TRL.Common.Collections;
//using TRL.Common.TimeHelpers;
//using TRL.Connect.Smartcom.Commands;
//using TRL.Transaction;
//using TRL.Connect.Smartcom.Handlers;
//using TRL.Handlers.Spreads;
//using TRL.Handlers.StopLoss;
//using TRL.Handlers.TakeProfit;
//using TRL.Handlers.Inputs;
//using TRx.Handlers;
//using TRL.Common.Statistics;
//using TRx.Strategy;

namespace TRx.Program
{
    class ProgramSample1 : TRx.Program.Template
    {
        public ProgramSample1(string[] args) : base(args)
        {
        }

        //private static MakeRangeBarsOnTick updateBarsHandler { get; set; }
        //private static IndicatorOnBar2Ma indicatorsOnBar { get; set; }
        //private static SmartComAdapter adapter { get; set; }
        //private static string[] assemblies = { "Interop.SmartCOM3Lib.dll", "TRL.Common.dll", "TRL.Connect.Smartcom.dll" };

        private static TRx.Strategy.Sample1 strategySample1  { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main(string[] args)
        {
            Template tmp = new ProgramSample1(args);
            tmp.Do();
        }
        
        #region // переопределение базовых методов
        ///// <summary>
        ///// пример
        ///// </summary>
        ///// <param name="args"></param>
        //override public void Do(string[] args)
        //{
        //    //TradeConsole.ConsoleSetSize();
        //    //TradeConsole.LogAssemblyInfo(assemblies);
        //    //список доступных команд
        //    TradeConsole.ConsoleWriteCommands();
        //    Initialize();
        //    SetupStrategy(args);
        //    base.Do(args);
        //}

        /// <summary>
        /// пример переопределения
        /// Program.Sample1.Initialize()
        /// </summary>
        override public void Initialize()
        {
            Console.WriteLine("Program.Sample1.Initialize()");
            marketDataProvider = new MarketDataProvider();
            rawTradingDataProvider = new RawTradingDataProvider(DefaultLogger.Instance);
            symbolsDataProvider = new SymbolsDataProvider();
            traderBase = new TraderBase(new SmartComOrderManager());
            adapter = new SmartComAdapter();

            SmartComHandlers.Instance.Add<_IStClient_DisconnectedEventHandler>(IsDisconnected);
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
        /// пример strategySample1
        /// </summary>
        /// <param name="args"></param>
        override public void SetupStrategy(string[] args)
        {
            Console.WriteLine("Оverride Program.Sample1.SetupStrategy()");
            // инициализация обработчиков стратегии
            strategySample1 = new Strategy.Sample1(args);
            strategyHeader = strategySample1.strategyHeader;
        }
        #endregion //

        #region // переопределение консольных команд
        /// <summary>
        /// переопределение консольные команды
        /// Бары
        /// </summary>
        override public void ConsoleHandlerB()
        {
            Console.WriteLine("Оverride Program.Sample1.ConsoleHandlerB()");
            // здесь вызвать базовый метод
            base.ConsoleHandlerB();
            {
                /*
                Console.Clear();
                Console.WriteLine("override ConsoleHandlerB()");
                foreach (Bar item in TradingData.Instance.Get<IEnumerable<Bar>>().OrderBy(i => i.DateTime))
                //foreach (Bar item in TradingData.Instance.Get<IEnumerable<Bar>>())
                {
                    TradeConsole.ConsoleWriteLineBar(item);
                    //TradeHubStarter.sendBarString(item);
                    TradeHubStarter.sendBar(item);
                }
                */
            }

            // здесь вызвать метод стратегии
            strategySample1.ConsoleHandlerB();
            {
                // здесь вызвать метод стратегии
                //foreach (var item in indicatorsOnBar.MaFastValue)
                //{
                //    TradeHubStarter.sendValueDouble1(item);
                //}
                //foreach (var item in indicatorsOnBar.MaSlowValue)
                //{
                //    TradeHubStarter.sendValueDouble2(item);
                //}
                //foreach (var item in indicatorsOnBar.CrossX)
                //{
                //    TradeHubStarter.sendValueBool(item);
                //}
            }
        }

        #endregion //
    }
}