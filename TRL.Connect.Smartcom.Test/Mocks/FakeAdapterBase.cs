using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Connect.Smartcom.Data;
using TRL.Connect.Smartcom.Events;
using TRL.Connect.Smartcom.Net;
using TRL.Logging;

namespace TRL.Connect.Smartcom.Test.Mocks
{
    public class FakeAdapterBase
    {
        private SmartComHandlersDatabase handlers;
        public SmartComHandlersDatabase Handlers
        {
            get
            {
                return this.handlers;
            }
        }

        private SmartComBinder binder;
        public SmartComBinder Binder
        {
            get
            {
                return this.binder;
            }
        }

        private StServerMockSingleton mockSingleton;
        public StServerMockSingleton StServerMockSingleton
        {
            get
            {
                return this.mockSingleton;
            }
        }

        private SmartComSubscriber subscriber;
        public SmartComSubscriber Subscriber
        {
            get
            {
                return this.subscriber;
            }
        }

        private SmartComConnector connector;
        public SmartComConnector Connector
        {
            get
            {
                return this.connector;
            }
        }

        public FakeAdapterBase()
        {
            this.handlers = new SmartComHandlersDatabase();
            this.mockSingleton = new StServerMockSingleton();
            this.subscriber = new SmartComSubscriber(this.mockSingleton.Instance, new NullLogger());
            this.binder = new SmartComBinder(this.mockSingleton.Instance, this.handlers, new NullLogger());
            this.connector = new SmartComConnector(this.mockSingleton.Instance, this.handlers, new NullLogger());
        }
    }
}
