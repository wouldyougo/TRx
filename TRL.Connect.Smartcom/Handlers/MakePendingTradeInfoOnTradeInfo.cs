using TRL.Common.Data;
using TRL.Common.Handlers;
using TRL.Connect.Smartcom.Data;
using TRL.Connect.Smartcom.Models;
using TRL.Common.TimeHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Logging;
using TRL.Common;

namespace TRL.Connect.Smartcom.Handlers
{
    public class MakePendingTradeInfoOnTradeInfo:GenericCollectionObserver<TradeInfo>
    {
        private ILogger logger;

        public MakePendingTradeInfoOnTradeInfo()
            : this(RawTradingData.Instance, DefaultLogger.Instance) { }

        public MakePendingTradeInfoOnTradeInfo(BaseDataContext rawData, ILogger logger)
            :base(rawData)
        {
            this.logger = logger;
        }

        public override void Update(TradeInfo item)
        {
            if (HasDuplicate(item))
                return;

            PendingTradeInfo expectedUpdateOrder =
                new PendingTradeInfo(item);

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, {2}",
                BrokerDateTime.Make(DateTime.Now),
                this.GetType().Name,
                expectedUpdateOrder.ToString()));

            this.dataContext.GetData<PendingTradeInfo>().Add(expectedUpdateOrder);
        }

        private bool HasDuplicate(TradeInfo item)
        {
            try
            {
                return this.dataContext.GetData<TradeInfo>()
                    .Where(i => i.TradeNo.Equals(item.TradeNo))
                    .Count() > 1;
            }
            catch
            {
                return false;
            }
        }
    }
}
