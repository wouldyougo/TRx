using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartCOM3Lib;
using TRL.Connect.Smartcom.Data;
using TRL.Common.Events;
using TRL.Common.Extensions;
using System.Reflection;
using TRL.Common.Data;
using TRL.Logging;
using TRL.Common;
using TRL.Common.TimeHelpers;

namespace TRL.Connect.Smartcom.Events
{
    public class SmartComBinder:TRL.Common.Events.IBinder
    {
        private SmartComHandlersDatabase handlers;

        private ILogger logger;
        private StServer stServer;
        private int bindedHandlersCounter;
        public int BindedHandlersCounter
        {
            get
            {
                return this.bindedHandlersCounter;
            }
        }

        public SmartComBinder()
            : this(new StServerSingleton().Instance, SmartComHandlers.Instance, DefaultLogger.Instance)
        {
        }

        public SmartComBinder(StServer stServer, SmartComHandlersDatabase handlers, ILogger logger)
        {
            this.logger = logger;
            this.stServer = stServer;
            this.handlers = handlers;
        }

        private void WriteLogMessage<T>(string action) where T : class
        {
            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, {2} обработчика {3} успешно выполнено", 
                BrokerDateTime.Make(DateTime.Now),
                this.GetType().Name, 
                action, 
                typeof(T).Name));
        }

        public void UnbindAllHandlersOfType<T>() where T : class
        {
            foreach (T item in this.handlers.GetData<T>())
            {
                this.stServer.Unbind<T>(item);
                this.bindedHandlersCounter--;
                WriteLogMessage<T>("отключение");
            }
        }

        public void BindAllHandlersOfType<T>() where T : class
        {
            foreach (T item in this.handlers.GetData<T>())
            {
                this.stServer.Bind<T>(item);
                this.bindedHandlersCounter++;
                WriteLogMessage<T>("подключение");
            }
        }

        public void Unbind()
        {
            if (this.bindedHandlersCounter == 0)
                return;

            foreach (Type t in SmartComEventsTypes.Collection)
            {
                MethodInfo mi = this.GetType().GetMethod("UnbindAllHandlersOfType").MakeGenericMethod(t);
                mi.Invoke(this, null);
            }
        }

        public void Bind()
        {
            if (this.bindedHandlersCounter == this.handlers.HandlerCounter)
                return;

            foreach (Type t in SmartComEventsTypes.Collection)
            {
                MethodInfo mi = this.GetType().GetMethod("BindAllHandlersOfType").MakeGenericMethod(t);
                mi.Invoke(this, null);
            }
        }

    }
}
