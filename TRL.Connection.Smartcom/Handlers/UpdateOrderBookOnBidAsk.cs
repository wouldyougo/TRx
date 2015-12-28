using TRL.Common.Data;
using TRL.Common.Handlers;
using TRL.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Collections;
using TRL.Logging;

namespace TRL.Connect.Smartcom.Handlers
{
    public class UpdateOrderBookOnBidAsk : AddedItemHandler<BidAsk>
    {
        private OrderBookContext storage;
        private ILogger logger;

        public UpdateOrderBookOnBidAsk()
            : this(TradingData.Instance) { }

        public UpdateOrderBookOnBidAsk(IDataContext context)
            : this(OrderBook.Instance, context, DefaultLogger.Instance)
        { }

        public UpdateOrderBookOnBidAsk(OrderBookContext storage, IDataContext context, ILogger logger)
            : base(context.Get<ObservableCollection<BidAsk>>())
        {
            this.storage = storage;
            this.logger = logger;
        }

        public override void OnItemAdded(BidAsk item)
        {
            storage.Update(item.Row, item.Symbol, item.Bid, item.BidSize, item.Ask, item.AskSize);
        }
    }
}
