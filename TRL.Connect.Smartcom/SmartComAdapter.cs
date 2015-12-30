using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Connect.Smartcom.Events;
using TRL.Connect.Smartcom.Data;
using SmartCOM3Lib;
using TRL.Connector;
using TRL.Message;
using TRL.Connect.Smartcom;
using TRL.Configuration;
using TRL.Common.Events;
using TRL.Common.Data;
using System.Runtime.InteropServices;
using System.Threading;
using TRL.Logging;
using TRL.Common;
using TRL.Common.TimeHelpers;

namespace TRL.Connect.Smartcom
{
    public class SmartComAdapter : Subject, IService
    {
        private IGenericSingleton<StServer> stServerSingleton;
        private StServer stServer;
        private SmartComHandlersDatabase handlers;
        private IBinder binder;
        private ISubscriber subscriber;
        private ILogger logger;
        private IConnector connector;
        private IDataContext defaultAdapterHandlers;
        private Timer dataFlowMonitorTimer, connectionMonitorTimer;
        private int seconds, monitorTimeoutSeconds;

        public SmartComAdapter()
            : this(new SmartComConnector(), 
            SmartComHandlers.Instance, 
            DefaultBinder.Instance, 
            DefaultSubscriber.Instance, 
            new StServerSingleton(), 
            DefaultLogger.Instance) { }

        public SmartComAdapter(IConnector connector, 
            SmartComHandlersDatabase handlers, 
            IBinder binder, 
            ISubscriber subscriber, 
            IGenericSingleton<StServer> stServerSingleton, 
            ILogger logger,
            int monitorTimeoutSeconds = 20)
        {
            this.stServerSingleton = stServerSingleton;
            this.stServer = this.stServerSingleton.Instance;
            this.handlers = handlers;
            this.binder = binder;
            this.subscriber = subscriber;
            this.logger = logger;
            this.connector = connector;
            this.monitorTimeoutSeconds = monitorTimeoutSeconds;
            this.defaultAdapterHandlers = new AdapterHandlers();
            this.seconds = AppSettings.GetValue<int>("SecondsBetweenConnectionAwaitingAttempts");

            this.handlers.Add<_IStClient_ConnectedEventHandler>(SmartComTraderConnected);
            this.handlers.Add<_IStClient_DisconnectedEventHandler>(SmartComTraderDisconnected);

            this.dataFlowMonitorTimer = new Timer(MonitorDataFlow, null, this.monitorTimeoutSeconds * 1000, 1000 * this.seconds);
            this.connectionMonitorTimer = new Timer(MonitorConnection, null, this.monitorTimeoutSeconds * 1000, 1000 * this.seconds);
        }

        private void MonitorConnection(object state)
        {
            if (this.isRunning && !this.connector.IsConnected)
                this.connector.Connect();
        }

        private void MonitorDataFlow(object state)
        {
            if (this.isRunning && this.connector.IsConnected)
                this.stServer.GetPrortfolioList();
        }

        public void Start()
        {
            this.isRunning = true;

            BindHandlers();

            this.connector.Connect();
        }

        private void BindHandlers()
        {
            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, выполняется связывание обработчиков SmartCom", 
                BrokerDateTime.Make(DateTime.Now), 
                this.GetType().Name));
            this.binder.Bind();
        }


        public void Stop()
        {
            this.isRunning = false;

            Unsubscribe();

            this.connector.Disconnect();

            UnbindHandlers();
        }

        private void Reconnect()
        {
            this.connector.Connect();
        }

        private void Unsubscribe()
        {
            if (this.subscriber.SubscriptionsCounter == 0)
                return;

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, отменяем подписки на получение данных", 
                BrokerDateTime.Make(DateTime.Now), 
                this.GetType().Name));

            this.subscriber.Unsubscribe();
        }

        private void UnbindHandlers()
        {
            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, выполняется отключение обработчиков SmartCom", 
                BrokerDateTime.Make(DateTime.Now), 
                this.GetType().Name));
            this.binder.Unbind();
        }

        public void Restart()
        {
            this.Stop();

            this.Start();
        }

        private bool isRunning;
        public bool IsRunning
        {
            get { return this.isRunning; }
        }

        private void SmartComTraderConnected()
        {
            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, соединение установлено", 
                BrokerDateTime.Make(DateTime.Now), 
                this.GetType().Name));
            this.subscriber.Subscribe();
        }

        private void SmartComTraderDisconnected(string reason)
        {
            if (reason.ToLower().Contains("disconnected by user"))
                return;

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, соединение неожиданно разорвано {2}", 
                BrokerDateTime.Make(DateTime.Now), 
                this.GetType().Name, 
                reason));
        }
        /// <summary>
        /// stServer.IsConnected()
        /// </summary>
        public bool IsConnected
        {
            get { return this.connector.IsConnected; }
        }
    }
}
