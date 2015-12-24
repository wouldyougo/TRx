using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Connect.Smartcom.Models;
using TRL.Common.Handlers;
using TRL.Common.Data;
using TRL.Connect.Smartcom.Data;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;
using TRL.Common.Events;
using TRL.Common.Collections;
using TRL.Logging;
using TRL.Common;

namespace TRL.Connect.Smartcom.Events
{
    public class RawUpdateBidAskHandler:IGenericObserver<UpdateBidAsk>
    {
        private ObservableCollection<UpdateBidAsk> rawData;
        private IDataContext tradingData;
        private ILogger logger;

        public RawUpdateBidAskHandler(ObservableCollection<UpdateBidAsk> rawData, IDataContext tradingData, ILogger logger)
        {
            this.rawData = rawData;
            this.tradingData = tradingData;
            this.logger = logger;
        }

        public void Update(UpdateBidAsk item)
        {
            if (item.Row != 0)
            {
                this.rawData.Remove(item);
                return;
            }

            BidAsk bidAsk = GetBidAsk(item.Symbol);

            BidAsk ba = MakeBidAsk(item);

            this.rawData.Remove(item);


            if (bidAsk != null)
                this.tradingData.Get<ICollection<BidAsk>>().Remove(bidAsk);

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, UpdateBidAsk преобразован в BidAsk, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 
                BrokerDateTime.Make(DateTime.Now),
                this.GetType().Name,
                ba.Symbol, 
                ba.Row, 
                ba.NRows, 
                ba.Bid, 
                ba.BidSize, 
                ba.Ask, 
                ba.AskSize, 
                ba.DateTime));

            this.tradingData.Get<ObservableCollection<BidAsk>>().Add(ba);

        }

        private BidAsk GetBidAsk(string symbol)
        {
            try
            {
                return this.tradingData.Get<IEnumerable<BidAsk>>().Single(i => i.Symbol == symbol);
            }
            catch
            {
                return null;
            }
        }

        private BidAsk MakeBidAsk(UpdateBidAsk src)
        {
            return new BidAsk { Symbol = src.Symbol, 
                Row = src.Row, 
                NRows = src.NRows, 
                Bid = src.Bid, 
                BidSize = src.BidSize, 
                Ask = src.Ask,
                AskSize = src.AskSize,
                DateTime = BrokerDateTime.Make(DateTime.Now),
                Id = SerialIntegerFactory.Make()
            };
        }
    }
}
