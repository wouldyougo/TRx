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
//using TRL.Connect.Smartcom;
//using TRL.Connect.Smartcom.Commands;
using TRL.Connect.Smartcom.Data;
using TRL.Transaction;
//using TRL.Connect.Smartcom.Handlers;
//using TRL.Handlers.Spreads;
//using TRL.Handlers.StopLoss;
//using TRL.Handlers.TakeProfit;
//using TRL.Handlers.Inputs;
using TRx.Handlers;
using TRx.Helpers;
using TRx.Base;
using TRL.Connect.Smartcom.Commands;

//using TRL.Common.Statistics;

namespace TRx.Strategy
{
    /// <summary>
    /// пример стратегии с пересечением скользящих 
    /// с тейк профитом
    /// исправить сигнал на открытие позиции только при пересечении
    /// </summary>
    public class Sample3 : ISetupStrategy
    {
        #region // базовые сущности
        public  StrategyHeader strategyHeader { get; set; }
        public  BarSettings barSettings { get; set; }

        /// <summary>
        /// отправляем Bar клиентам 
        /// </summary>
        public  SendItemOnBar sendItemBar { get; set; }
        ///    = new SendItemOnBar(barSettings, TradingData.Instance);
        #endregion //

        private  MakeRangeBarsOnTick updateBarsHandler { get; set; }
        private  IndicatorOnBar2Ma indicatorsOnBar { get; set; }
        private ReversOnBar reversHandler { get; set; }

        private TakeProfitOnBar takeProfitOnBar { get; set; }

        private StopLossOnBar stopLossOnBar { get; set; }

        public Sample3(string[] args)
        {
            Initialize();
            SetupStrategy(args);
        }

        #region // методы стратегии
        public void Initialize()
        {
            Console.WriteLine("Strategy.Sample3.Initialize()");

            strategyHeader = new StrategyHeader(1, "Strategy Sample3",
                AppSettings.GetStringValue("Portfolio"),
                AppSettings.GetStringValue("Symbol"),
                AppSettings.GetValue<double>("Amount"));
            barSettings = new BarSettings(strategyHeader,
                        strategyHeader.Symbol,
                        AppSettings.GetValue<int>("Interval"),
                        AppSettings.GetValue<int>("Period"));
            
            //Отправляем данные клиентам
            {
                /// отправляем Bar клиентам 
                sendItemBar = new SendItemOnBar(barSettings, TradingData.Instance);
                sendItemBar.AddItemHandler(TradeConsole.ConsoleWriteLineBar);
                if (AppSettings.GetValue<bool>("SignalHub"))
                {
                    //отправляем через signalR
                    sendItemBar.AddItemHandler(TradeHubStarter.sendBar);
                }
            }

            //private static ProfitPointsSettings ppSettings =
            //    new ProfitPointsSettings(strategyHeader, AppSettings.GetValue<double>("ProfitPoints"), false);

            //private static TakeProfitOrderSettings poSettings =
            //    new TakeProfitOrderSettings(strategyHeader, 86400);

            //private static StopPointsSettings spSettings =
            //    new StopPointsSettings(strategyHeader, AppSettings.GetValue<double>("StopPoints"), false);

            //private static StopLossOrderSettings soSettings =
            //    new StopLossOrderSettings(strategyHeader, 86400);

            //SmartComHandlers.Instance.Add<_IStClient_DisconnectedEventHandler>(IsDisconnected);
            //SmartComHandlers.Instance.Add<_IStClient_ConnectedEventHandler>(IsConnected);


            AddStrategySettings();
            //TradingData.Instance.Get<ICollection<StrategyHeader>>().Add(strategyHeader);
            //TradingData.Instance.Get<ICollection<BarSettings>>().Add(barSettings);

            AddStrategySubscriptions();
            //DefaultSubscriber.Instance.Portfolios.Add(strategyHeader.Portfolio);
            //DefaultSubscriber.Instance.BidsAndAsks.Add(strategyHeader.Symbol);
            //DefaultSubscriber.Instance.Ticks.Add(strategyHeader.Symbol);
        }
        //private static string[] assemblies = { "Interop.SmartCOM3Lib.dll", "TRL.Common.dll", "TRL.Connect.Smartcom.dll" };

        /// <summary>
        /// пример
        /// </summary>
        /// <param name="args"></param>
        public void SetupStrategy(string[] args)
        {

            //TradingData.Instance.Get<ICollection<Strategy>>().Add(strategyHeader);
            //TradingData.Instance.Get<ICollection<BarSettings>>().Add(barSettings);
            //TradingData.Instance.Get<ICollection<ProfitPointsSettings>>().Add(ppSettings);
            //TradingData.Instance.Get<ICollection<TakeProfitOrderSettings>>().Add(poSettings);
            //TradingData.Instance.Get<ICollection<StopPointsSettings>>().Add(spSettings);
            //TradingData.Instance.Get<ICollection<StopLossOrderSettings>>().Add(soSettings);

            //SMASettings smaSettings = new SMASettings(strategyHeader, 7, 14);
            int maf = AppSettings.GetValue<int>("MaFast");
            int mas = AppSettings.GetValue<int>("MaSlow");
            SMASettings smaSettings = new SMASettings(strategyHeader, maf, mas);
            TradingData.Instance.Get<ICollection<SMASettings>>().Add(smaSettings);

            //MakeRangeBarsOnTick updateBarsHandler;
            updateBarsHandler = new MakeRangeBarsOnTick(barSettings,
                    new TimeTracker(),
                    TradingData.Instance,
                    DefaultLogger.Instance);

            //IndicatorOnBar2Ma indicatorsOnBar;
            indicatorsOnBar = new IndicatorOnBar2Ma(strategyHeader,
                    TradingData.Instance,
                    SignalQueue.Instance,
                    DefaultLogger.Instance);

            indicatorsOnBar.AddMa1Handler(TradeConsole.ConsoleWriteLineValueDouble);
            indicatorsOnBar.AddMa2Handler(TradeConsole.ConsoleWriteLineValueDouble);
            indicatorsOnBar.AddCrossUpHandler(TradeConsole.ConsoleWriteLineValueBool);
            indicatorsOnBar.AddCrossDnHandler(TradeConsole.ConsoleWriteLineValueBool);

            //Отправляем данные клиентам
            {
                //SetupHubHandlers();
                if (AppSettings.GetValue<bool>("SignalHub"))
                {
                    //отправляем через signalR
                    indicatorsOnBar.AddMa1Handler(TradeHubStarter.sendValueDouble1);
                    indicatorsOnBar.AddMa2Handler(TradeHubStarter.sendValueDouble2);
                    indicatorsOnBar.AddCrossUpHandler(TradeHubStarter.sendValueBool);
                    indicatorsOnBar.AddCrossDnHandler(TradeHubStarter.sendValueBool);

                    //reversHandler.AddMa1Handler(TradeHubStarter.sendIndicator1);
                    //reversHandler.AddMa2Handler(TradeHubStarter.sendIndicator2);
                }
                else
                {
                    Console.WriteLine(String.Format("SignalHub is off"));
                }
            }

            //ReversOnBar reversHandler =
            reversHandler = new ReversOnBar(strategyHeader,
                    TradingData.Instance,
                    SignalQueue.Instance,
                    DefaultLogger.Instance)
                {
                    IndicatorsOnBar = indicatorsOnBar
                };

            double pp = AppSettings.GetValue<double>("ProfitPoints");
            //TakeProfitOnBar takeProfitOnBar =
            takeProfitOnBar =
            new TakeProfitOnBar(strategyHeader,
                    200,
                    TradingData.Instance,
                    SignalQueue.Instance,
                    DefaultLogger.Instance);

            //StopLossOnBar stopLossOnBar =
            stopLossOnBar =
            new StopLossOnBar(strategyHeader,
                    100,
                    TradingData.Instance,
                    SignalQueue.Instance,
                    DefaultLogger.Instance);

        }
        #endregion //

        #region // консольные команды
        /// <summary>
        /// необъодимо вызывать метод из переопределения консольной команды
        /// Индикаторы
        /// </summary>
        public void ConsoleHandlerB()
        {
            //foreach (double item in indicatorsOnBar.MaFast)
            //{
            //    TradeHubStarter.sendDouble1(item);
            //}
            //foreach (double item in indicatorsOnBar.MaSlow)
            //{
            //    TradeHubStarter.sendDouble2(item);
            //}
            foreach (var item in indicatorsOnBar.MaFastValue)
            {
                TradeHubStarter.sendValueDouble1(item);
            }
            foreach (var item in indicatorsOnBar.MaSlowValue)
            {
                TradeHubStarter.sendValueDouble2(item);
            }
            foreach (var item in indicatorsOnBar.CrossX)
            {
                TradeHubStarter.sendValueBool(item);
            }
        }
        #endregion //

        public void AddStrategySubscriptions()
        {
            DefaultSubscriber.Instance.Portfolios.Add(strategyHeader.Portfolio);
            DefaultSubscriber.Instance.BidsAndAsks.Add(strategyHeader.Symbol);
            DefaultSubscriber.Instance.Ticks.Add(strategyHeader.Symbol);
        }
        public void AddStrategySettings()
        {
            TradingData.Instance.Get<ICollection<StrategyHeader>>().Add(strategyHeader);
            TradingData.Instance.Get<ICollection<BarSettings>>().Add(barSettings);
            //TradingData.Instance.Get<ICollection<ProfitPointsSettings>>().Add(ppSettings);
            //TradingData.Instance.Get<ICollection<TakeProfitOrderSettings>>().Add(poSettings);
            //TradingData.Instance.Get<ICollection<StopPointsSettings>>().Add(spSettings);
            //TradingData.Instance.Get<ICollection<StopLossOrderSettings>>().Add(soSettings);
        }
        public void IsConnected()
        {
            //DefaultLogger.Instance.Log("IsConnected.");
            //Console.WriteLine("IsConnected.");
            //ITransaction getBars = new GetBarsCommand(barSettings.Symbol, barSettings.Interval, 3);
            ITransaction getBars = new GetBarsCommand(barSettings.Symbol, barSettings.Interval, 12);
            getBars.Execute();
        }
    }
}