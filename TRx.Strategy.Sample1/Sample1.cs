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

//using TRL.Common.Statistics;

namespace TRx.Strategy
{
    /// <summary>
    /// пример вычисления пересечения скользящих средних
    /// </summary>
    public class Sample1 : ISetupStrategy
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

        public Sample1(string[] args)
        {
            Initialize();
            SetupStrategy(args);
        }

        #region // методы стратегии
        public void Initialize()
        {
            Console.WriteLine("Strategy.Sample1.Initialize()");
            strategyHeader = new StrategyHeader(1, "Strategy Sample1",
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

                //AddStrategySettings();
            TradingData.Instance.Get<ICollection<StrategyHeader>>().Add(strategyHeader);
            TradingData.Instance.Get<ICollection<BarSettings>>().Add(barSettings);

            //AddStrategySubscriptions();
            DefaultSubscriber.Instance.Portfolios.Add(strategyHeader.Portfolio);
            DefaultSubscriber.Instance.BidsAndAsks.Add(strategyHeader.Symbol);
            DefaultSubscriber.Instance.Ticks.Add(strategyHeader.Symbol);
        }
        //private static string[] assemblies = { "Interop.SmartCOM3Lib.dll", "TRL.Common.dll", "TRL.Connect.Smartcom.dll" };

        /// <summary>
        /// пример
        /// </summary>
        /// <param name="args"></param>
        public void SetupStrategy(string[] args)
        {
            //AddStrategySettings();
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

            //ReversMaOnBar reversHandler =
            //    new ReversMaOnBar(strategyHeader,
            //        TradingData.Instance,
            //        SignalQueue.Instance,
            //        DefaultLogger.Instance);

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
            }
        }
        #endregion //

        #region // консольные команды
        /// <summary>
        /// необъодимо вызывать метов из переопределения консольной команды
        /// Бары
        /// </summary>
        public void ConsoleHandlerB()
        {
            Console.Clear();
            foreach (Bar item in TradingData.Instance.Get<IEnumerable<Bar>>().OrderBy(i => i.DateTime))
            //foreach (Bar item in TradingData.Instance.Get<IEnumerable<Bar>>())
            {
                TradeConsole.ConsoleWriteLineBar(item);
                //TradeHubStarter.sendBarString(item);
                TradeHubStarter.sendBar(item);
            }
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

        /// <summary>
        /// пример
        /// </summary>
        private void SetupRevers()
        {
            //AddStrategySettings();
            {
                TradingData.Instance.Get<ICollection<StrategyHeader>>().Add(strategyHeader);
                TradingData.Instance.Get<ICollection<BarSettings>>().Add(barSettings);

                //TradingData.Instance.Get<ICollection<ProfitPointsSettings>>().Add(ppSettings);
                //TradingData.Instance.Get<ICollection<TakeProfitOrderSettings>>().Add(poSettings);
                //TradingData.Instance.Get<ICollection<StopPointsSettings>>().Add(spSettings);
                //TradingData.Instance.Get<ICollection<StopLossOrderSettings>>().Add(soSettings);

                //SMASettings smaSettings = new SMASettings(strategyHeader, 7, 14);
                int mafast = AppSettings.GetValue<int>("MaFast");
                int maslow = AppSettings.GetValue<int>("MaSlow");
                SMASettings smaSettings = new SMASettings(strategyHeader, mafast, maslow);
                TradingData.Instance.Get<ICollection<SMASettings>>().Add(smaSettings);
            }

            //SmartComHandlers.Instance.Add<_IStClient_DisconnectedEventHandler>(IsDisconnected);
            //SmartComHandlers.Instance.Add<_IStClient_ConnectedEventHandler>(IsConnected);

            MakeRangeBarsOnTick updateBarsHandler =
                new MakeRangeBarsOnTick(barSettings,
                    new TimeTracker(),
                    TradingData.Instance,
                    DefaultLogger.Instance);

            SendItemOnBar barItemSender =
                new SendItemOnBar(barSettings,
                                  TradingData.Instance);
            //barItemSender.AddItemHandler(TradeConsole.ConsoleWriteLineBar);

            IndicatorOnBar2Ma indicatorsOnBar =
                new IndicatorOnBar2Ma(strategyHeader,
                    TradingData.Instance,
                    SignalQueue.Instance,
                    DefaultLogger.Instance);
            indicatorsOnBar.AddMa1Handler(TradeConsole.ConsoleWriteLineValueDouble);
            indicatorsOnBar.AddMa2Handler(TradeConsole.ConsoleWriteLineValueDouble);
            indicatorsOnBar.AddCrossUpHandler(TradeConsole.ConsoleWriteLineValueBool);
            indicatorsOnBar.AddCrossDnHandler(TradeConsole.ConsoleWriteLineValueBool);

            ReversMaOnBar reversHandler =
                new ReversMaOnBar(strategyHeader,
                    TradingData.Instance,
                    SignalQueue.Instance,
                    DefaultLogger.Instance)
                {
                    IndicatorsOnBar = indicatorsOnBar
                };

            SendItemOnTrade tradeItemSender =
                new SendItemOnTrade(TradingData.Instance, DefaultLogger.Instance);
            //tradeItemSender.AddItemHandler(TradeConsole.ConsoleWriteLineTrade);

            //SendItemOnOrder senderItemOrder =
            //    new SendItemOnOrder(TradingData.Instance.Get<ObservableQueue<Order>>());
            //senderItemOrder.AddedItemHandler(TradeHubStarter.sendOrder);

            //AddStrategySubscriptions();
            {
                DefaultSubscriber.Instance.Portfolios.Add(strategyHeader.Portfolio);
                DefaultSubscriber.Instance.BidsAndAsks.Add(strategyHeader.Symbol);
                DefaultSubscriber.Instance.Ticks.Add(strategyHeader.Symbol);
            }

            //Отправляем данные клиентам
            {
                barItemSender.AddItemHandler(TradeConsole.ConsoleWriteLineBar);
                tradeItemSender.AddItemHandler(TradeConsole.ConsoleWriteLineTrade);
                //senderItemOrder.AddedItemHandler(TradeHubStarter.sendOrder);

                //SetupHubHandlers();
                if (AppSettings.GetValue<bool>("SignalHub"))
                {
                    //отправляем через signalR
                    barItemSender.AddItemHandler(TradeHubStarter.sendBar);
                    tradeItemSender.AddItemHandler(TradeHubStarter.sendTrade);
                    indicatorsOnBar.AddMa1Handler(TradeHubStarter.sendValueDouble1);
                    indicatorsOnBar.AddMa2Handler(TradeHubStarter.sendValueDouble2);
                    indicatorsOnBar.AddCrossUpHandler(TradeHubStarter.sendValueBool);
                    indicatorsOnBar.AddCrossDnHandler(TradeHubStarter.sendValueBool);

                    //reversHandler.AddMa1Handler(TradeHubStarter.sendIndicator1);
                    //reversHandler.AddMa2Handler(TradeHubStarter.sendIndicator2);
                }
            }
        }

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

            //SMASettings smaSettings = new SMASettings(strategyHeader, 7, 14);
            int maf = AppSettings.GetValue<int>("MaFast");
            int mas = AppSettings.GetValue<int>("MaSlow");
            SMASettings smaSettings = new SMASettings(strategyHeader, maf, mas);
            TradingData.Instance.Get<ICollection<SMASettings>>().Add(smaSettings);
        }
    }
}