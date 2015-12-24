using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Connect.Smartcom.Test.Mocks;
using TRL.Connect.Smartcom.Data;
using SmartCOM3Lib;
using TRL.Common.Data;
using TRL.Logging;

namespace TRL.Connect.Smartcom.Test.Data
{
    [TestClass]
    public class SmartComSubscriberTests
    {
        private SmartComSubscriber subscriber;

        [TestInitialize]
        public void Setup()
        {
            this.subscriber = new SmartComSubscriber(new StServerClassMock(), new NullLogger());
        }

        [TestMethod]
        public void Subscriber_Increments_SubscribtionsCounter_After_Subscribe()
        {
            this.subscriber.Ticks.Add("RTS-6.13_FT");
            this.subscriber.Portfolios.Add("ST88888-RF-01");
            this.subscriber.BidsAndAsks.Add("Si-6.13_FT");

            Assert.AreEqual(0, this.subscriber.SubscriptionsCounter);

            this.subscriber.Subscribe();

            Assert.AreEqual(3, this.subscriber.SubscriptionsCounter);
        }

        [TestMethod]
        public void Subscriber_Decrements_SubscribtionsCounter_After_Unsubscribe()
        {
            this.subscriber.Ticks.Add("RTS-6.13_FT");
            this.subscriber.Portfolios.Add("ST88888-RF-01");
            this.subscriber.BidsAndAsks.Add("Si-6.13_FT");

            Assert.AreEqual(0, this.subscriber.SubscriptionsCounter);

            this.subscriber.Subscribe();

            Assert.AreEqual(3, this.subscriber.SubscriptionsCounter);

            this.subscriber.Unsubscribe();

            Assert.AreEqual(0, this.subscriber.SubscriptionsCounter);
        }

        [TestMethod]
        public void Subscriber_Do_Nothing_For_Empty_Subscriptions_Lists()
        {
            Assert.AreEqual(0, this.subscriber.SubscriptionsCounter);

            this.subscriber.Subscribe();

            Assert.AreEqual(0, this.subscriber.SubscriptionsCounter);
        }

        [TestMethod]
        public void Subscriber_Do_Nothing_For_Empty_Subscriptions_After_Update()
        {
            Assert.AreEqual(0, this.subscriber.SubscriptionsCounter);

            this.subscriber.Subscribe();

            Assert.AreEqual(0, this.subscriber.SubscriptionsCounter);
        }

        [TestMethod]
        public void DefaultSubscriber_IsSingleton()
        {
            SmartComSubscriber s = DefaultSubscriber.Instance;
            SmartComSubscriber s1 = DefaultSubscriber.Instance;

            Assert.AreSame(s, s1);
        }
    }
}
