using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common;
//using TRL.Common.Extensions;
using TRL.Common.TimeHelpers;
using TRL.Common.Collections;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Handlers;
using TRL.Common.Models;
using TRL.Logging;

namespace TRL.Handlers.Cancel
{
    public class MakeOrderCancellationRequestOnQuote : QuotesHandler
    {
        private StrategyHeader strategyHeader;
        private double priceShift;
        private IQuoteProvider quotesProvider;
        private IDataContext tradingData;
        private ILogger logger;

        private double bestPrice;
        private OrderCancellationRequest request;

        public MakeOrderCancellationRequestOnQuote(StrategyHeader strategyHeader,
            double priceShiftPoints)
            : this(strategyHeader, priceShiftPoints, OrderBook.Instance, TradingData.Instance, new NullLogger()) { }

        public MakeOrderCancellationRequestOnQuote(StrategyHeader strategyHeader,
            double priceShiftPoints,
            IQuoteProvider quotesProvider,
            IDataContext tradingData,
            ILogger logger)
            : base(quotesProvider)
        {
            this.strategyHeader = strategyHeader;
            this.priceShift = priceShiftPoints;
            this.quotesProvider = quotesProvider;
            this.tradingData = tradingData;
            this.logger = logger;

            this.bestPrice = 0;
            this.request = null;
        }

        public override void OnQuotesUpdate(string symbol)
        {
            if (!this.strategyHeader.Symbol.Equals(symbol))
                return;

            if (this.tradingData.GetAmount(this.strategyHeader) != 0)
                return;

            Order lastUnfilledOrder = GetLastUnfilledOrder();

            if (lastUnfilledOrder == null)
                return;

            if (!lastUnfilledOrder.IsDelivered)
                return;

            if (!BestQuotePriceGoneOverTheLimit(lastUnfilledOrder))
                return;

            if (OrderCancellationRequstExists(lastUnfilledOrder))
                return;

            this.request = null;
            this.request = MakeOrderCancellationRequest(lastUnfilledOrder);             

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, запрос на снятие заявки {2}", DateTime.Now, this.GetType().Name, request.ToString()));

            this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Add(request);

        }

        private Order GetLastUnfilledOrder()
        {
            try
            {
                return this.tradingData.GetUnfilled(this.strategyHeader, OrderType.Limit).Last();
            }
            catch
            {
                return null;
            }
        }

        private bool BestQuotePriceGoneOverTheLimit(Order order)
        {
            if (order.TradeAction == TradeAction.Buy)
            {
                this.bestPrice = quotesProvider.GetBidPrice(order.Symbol, 0);

                return order.Price + priceShift <= bestPrice;
            }
            else
            {
                this.bestPrice = quotesProvider.GetOfferPrice(order.Symbol, 0);

                return order.Price - priceShift >= bestPrice;
            }
        }

        private bool OrderCancellationRequstExists(Order order)
        {
            return this.tradingData.Get<IEnumerable<OrderCancellationRequest>>().Any(o => o.OrderId == order.Id
                && o.DateTime.AddSeconds(60) > BrokerDateTime.Make(DateTime.Now));
        }

        private OrderCancellationRequest MakeOrderCancellationRequest(Order order)
        {
            return new OrderCancellationRequest(order,
                String.Format("Лучшая цена ушла от лимита заявки на {0} пунктов!", this.priceShift));
        }

    }


}
