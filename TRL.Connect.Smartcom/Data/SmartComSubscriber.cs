using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using SmartCOM3Lib;
using TRL.Configuration;
using TRL.Common.Events;
using TRL.Logging;
using TRL.Common;
using TRL.Common.TimeHelpers;

namespace TRL.Connect.Smartcom.Data
{
    public class SmartComSubscriber:ISubscriber
    {
        private int subscriptionsCounter;
        private StServer stServer;
        private ILogger logger;

        public HashSet<string> Ticks;
        public HashSet<string> Portfolios;
        public HashSet<string> BidsAndAsks;
        public HashSet<string> Quotes;

        public SmartComSubscriber() :
            this(new StServerSingleton().Instance, DefaultLogger.Instance) { }

        public SmartComSubscriber(StServer stServer, ILogger logger)
        {
            this.stServer = stServer;
            this.logger = logger;

            this.Ticks = new HashSet<string>();
            this.Portfolios = new HashSet<string>();
            this.BidsAndAsks = new HashSet<string>();
            this.Quotes = new HashSet<string>();
        }

        public void Subscribe()
        {
            DataProviderSettings settings = new DataProviderSettings();

            SubscribeForPortfolios(settings);

            SubscribeForTicks(settings);

            SubscribeForBidsAndAsks(settings);

            SubscribeForQuotes(settings);
        }

        private void SubscribeForQuotes(DataProviderSettings settings)
        {
            if (settings.ListenQuotes)
            {
                foreach (string item in this.Quotes)
                {
                    this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, выполняется подписка на получение Quote для {2}", 
                        BrokerDateTime.Make(DateTime.Now), 
                        this.GetType().Name, 
                        item));
                    this.stServer.ListenQuotes(item);
                    this.subscriptionsCounter++;
                }
            }
        }

        private void SubscribeForBidsAndAsks(DataProviderSettings settings)
        {
            if (settings.ListenBidAsk)
            {
                foreach (string item in this.BidsAndAsks)
                {
                    this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, выполняется подписка на получение BidAsk для {2}", 
                        BrokerDateTime.Make(DateTime.Now),
                        this.GetType().Name, 
                        item));
                    this.stServer.ListenBidAsks(item);
                    this.subscriptionsCounter++;
                }
            }
        }

        private void SubscribeForTicks(DataProviderSettings settings)
        {
            if (settings.ListenTicks)
            {
                foreach (string item in this.Ticks)
                {
                    this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, выполняется подписка на получение Tick для {2}", 
                        BrokerDateTime.Make(DateTime.Now),
                        this.GetType().Name, 
                        item));
                    this.stServer.ListenTicks(item);
                    this.subscriptionsCounter++;
                }
            }
        }

        private void SubscribeForPortfolios(DataProviderSettings settings)
        {
            if (settings.ListenPortfolio)
            {
                foreach (string item in this.Portfolios)
                {
                    this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, выполняется подписка на получение Portfolio для {2}", 
                        BrokerDateTime.Make(DateTime.Now),
                        this.GetType().Name, 
                        item));
                    this.stServer.ListenPortfolio(item);
                    this.subscriptionsCounter++;
                }
            }
        }

        public void Unsubscribe()
        {
            DataProviderSettings settings = new DataProviderSettings();

            UnsubscribeFromPortfolios(settings);

            UnsubscribeFromTicks(settings);

            UnsubscribeFromBidAsk(settings);

            UnsubscribeFromQuotes(settings);

        }

        private void UnsubscribeFromPortfolios(DataProviderSettings settings)
        {
            if (settings.ListenPortfolio)
            {
                foreach (string item in this.Portfolios)
                {
                    this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, отменяем подписку на получение Portfolios для {2}", 
                        BrokerDateTime.Make(DateTime.Now),
                        this.GetType().Name, 
                        item));
                    this.stServer.CancelPortfolio(item);
                    this.subscriptionsCounter--;
                }
            }
        }

        private void UnsubscribeFromTicks(DataProviderSettings settings)
        {
            if (settings.ListenTicks)
            {
                foreach (string item in this.Ticks)
                {
                    this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, отменяем подписку на получение Ticks для {2}",
                        BrokerDateTime.Make(DateTime.Now), 
                        this.GetType().Name, 
                        item));
                    this.stServer.CancelTicks(item);
                    this.subscriptionsCounter--;
                }
            }
        }

        private void UnsubscribeFromBidAsk(DataProviderSettings settings)
        {
            if (settings.ListenBidAsk)
            {
                foreach (string item in this.BidsAndAsks)
                {
                    this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, отменяем подписку на получение BidAsk для {2}", 
                        BrokerDateTime.Make(DateTime.Now), 
                        this.GetType().Name, 
                        item));
                    this.stServer.CancelBidAsks(item);
                    this.subscriptionsCounter--;
                }
            }
        }

        private void UnsubscribeFromQuotes(DataProviderSettings settings)
        {
            if (settings.ListenQuotes)
            {
                foreach (string item in this.Quotes)
                {
                    this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, отменяем подписку на получение Quote для {2}", 
                        BrokerDateTime.Make(DateTime.Now),
                        this.GetType().Name, 
                        item));
                    this.stServer.CancelQuotes(item);
                    this.subscriptionsCounter--;
                }
            }
        }

        public int SubscriptionsCounter
        {
            get { return this.subscriptionsCounter; }
        }

    }
}
