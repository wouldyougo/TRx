using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.Collections;
using TRL.Common.TimeHelpers;
using TRL.Common;
using TRL.Logging;
using TRL.Common.Handlers;

namespace TRL.Emulation
{
    public abstract class TraderBaseInitializer
    {
        protected TradingDataContext tradingData;
        protected ObservableQueue<Signal> signalQueue;
        protected ObservableQueue<Order> orderQueue;
        protected IOrderManager orderManager;
        protected TraderBase traderBase;

        public TraderBaseInitializer()
        {
            this.tradingData = SampleTradingDataContextFactory.Make();
            this.signalQueue = new ObservableQueue<Signal>();
            this.orderQueue = new ObservableQueue<Order>();
            this.orderManager = new MockOrderManager();

            this.traderBase =
                new TraderBase(this.tradingData,
                    this.signalQueue,
                    this.orderQueue,
                    this.orderManager,
                    new AlwaysTimeToTradeSchedule(),
                    new NullLogger());
        }

        public void EmulateTradeFor(Signal signal)
        {
            EmulateTradeFor(signal, MakePrice(signal), signal.Strategy.Amount);
        }

        private double MakePrice(Signal signal)
        {
            if (signal.OrderType == OrderType.Limit)
                return signal.Limit;

            if (signal.OrderType == OrderType.Stop)
                return signal.Stop;

            return signal.Price;
        }

        public void EmulateTradeFor(Signal signal, double price)
        {
                EmulateTradeFor(signal, price, signal.Strategy.Amount);
        }

        public void EmulateTradeFor(Signal signal, double price, double amount)
        {
            EnqueueSignal(signal);

            this.tradingData.Get<ObservableHashSet<Trade>>().Add(MakeTrade(signal, price, amount));
        }

        private void EnqueueSignal(Signal signal)
        {
            if (!SignalExists(signal))
                this.signalQueue.Enqueue(signal);
        }

        private Trade MakeTrade(Signal signal, double price, double amount)
        {
            Order order = FindOrderFor(signal);

            ConfirmOrderDelivery(order);

            return new Trade(order, order.Portfolio, order.Symbol, price, GetSign(order.TradeAction) * amount, BrokerDateTime.Make(DateTime.Now));
        }

        private void ConfirmOrderDelivery(Order order)
        {
            OrderDeliveryConfirmation dc = new OrderDeliveryConfirmation(order, BrokerDateTime.Make(DateTime.Now));

            this.tradingData.Get<ObservableHashSet<OrderDeliveryConfirmation>>().Add(dc);
        }

        private Order FindOrderFor(Signal signal)
        {
            try
            {
                return this.tradingData.Get<IEnumerable<Order>>().Single(o => o.SignalId == signal.Id);
            }
            catch
            {
                return null;
            }
        }

        private double GetSign(TradeAction action)
        {
            return action == TradeAction.Buy ? 1 : -1;
        }

        private bool SignalExists(Signal signal)
        {
            return this.tradingData.Get<IEnumerable<Signal>>().Any(s => s.Id == signal.Id);
        }

    }
}
