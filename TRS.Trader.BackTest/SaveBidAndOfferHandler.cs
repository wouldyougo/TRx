using TRL.Common;
using TRL.Common.Data;
using TRL.Common.TimeHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRL.Logging;

namespace TRx.Trader.BackTest
{
    public class SaveBidAndOfferHandler
    {
        private string symbol;
        private OrderBookContext orderBook;
        private ILogger logger;
        private int currentIndex, maxIndex;

        public SaveBidAndOfferHandler(string symbol,
            OrderBookContext orderBook,
            int maxIndex,
            ILogger logger)
        {
            this.symbol = symbol;
            this.orderBook = orderBook;
            this.logger = logger;
            this.currentIndex = 0;
            this.maxIndex = maxIndex;

            this.orderBook.OnQuotesUpdate += new SymbolDataUpdatedNotification(OnChange);
        }

        private void OnChange(string symbol)
        {
            if (this.symbol != symbol)
                return;

            if (this.currentIndex == this.maxIndex - 1)
                this.currentIndex = 0;

            CultureInfo ci = CultureInfo.InvariantCulture;

            string message =
                String.Format("{0:dd/MM/yyyy H:mm:ss.fff},{1},{2},{3},{4},{5}",
                BrokerDateTime.Make(DateTime.Now),
                this.currentIndex,
                this.orderBook.GetBidPrice(this.symbol, this.currentIndex).ToString("0.0000", ci),
                this.orderBook.GetBidVolume(this.symbol, this.currentIndex).ToString("0.0000", ci),
                this.orderBook.GetOfferPrice(this.symbol, this.currentIndex).ToString("0.0000", ci),
                this.orderBook.GetOfferVolume(this.symbol, this.currentIndex).ToString("0.0000", ci));

            this.currentIndex++;

            this.logger.Log(message);

        }
    }
}
