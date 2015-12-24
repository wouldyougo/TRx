using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using TRL.Common.Models;
using TRL.Common.Collections;
using TRL.Logging;
using TRL.Common.TimeHelpers;

namespace TRL.Common.Handlers
{
    public class TraderBase
    {
        protected SignalQueueProcessor signalProcessor;
        protected OrderQueueProcessor orderProcessor;
        protected UpdatePositionOnTrade updatePositionHandler;
        protected RejectOrderOnOrderRejection rejectOrderHandler;
        protected CancelOrderOnCancellationRequest cancelOrderRequestHandler;
        protected CancelOrderOnCancellationConfirmation cancelOrderHandler;
        protected UpdateOrderOnOrderDeliveryConfirmation orderDeliveryConfirmationHandler;
        protected CancelOrderOnTrade cancelOrderOnTrade;
        protected PlaceCancelOrderRequestOnTick placeCancelOrderRequestOnTick;

        protected CancelStopOrderOnTrade cancelStopOnTrade;

        public TraderBase(IOrderManager orderManager)
            : this(TradingData.Instance,
            SignalQueue.Instance,
            OrderQueue.Instance,
            orderManager,
            new FortsTradingSchedule(),
            DefaultLogger.Instance)
        {
        }

        public TraderBase(IDataContext tradingData, 
            ObservableQueue<Signal> signalQueue, 
            ObservableQueue<Order> orderQueue, 
            IOrderManager orderManager,
            ITradingSchedule schedule,
            ILogger logger)
        {
            this.signalProcessor = 
                new SignalQueueProcessor(signalQueue, orderQueue, tradingData, schedule, logger);

            this.orderProcessor = 
                new OrderQueueProcessor(orderManager, tradingData, orderQueue, logger);

            this.updatePositionHandler = 
                new UpdatePositionOnTrade(tradingData, logger);

            this.rejectOrderHandler = 
                new RejectOrderOnOrderRejection(tradingData, logger);

            this.cancelOrderRequestHandler = 
                new CancelOrderOnCancellationRequest(orderManager, tradingData, logger);

            this.cancelOrderHandler = 
                new CancelOrderOnCancellationConfirmation(tradingData, logger);

            this.orderDeliveryConfirmationHandler = 
                new UpdateOrderOnOrderDeliveryConfirmation(tradingData, logger);

            this.cancelOrderOnTrade = 
                new CancelOrderOnTrade(tradingData, logger);

            this.placeCancelOrderRequestOnTick = 
                new PlaceCancelOrderRequestOnTick(tradingData, logger);

            this.cancelStopOnTrade = 
                new CancelStopOrderOnTrade(tradingData, logger);
        }

    }
}
