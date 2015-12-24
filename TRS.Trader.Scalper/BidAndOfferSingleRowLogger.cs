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

namespace TRx.Trader.Scalper
{
    public class BidAndOfferSingleRowLogger
    {
        private string symbol;
        private int rowIndex;

        private OrderBookContext orderBook;
        private ILogger logger;

        private double previousBidPrice;
        public double PreviousBidPrice
        {
            get
            {
                return this.previousBidPrice;
            }
        }

        private double previousOfferPrice;
        public double PreviousOfferPrice
        {
            get
            {
                return this.previousOfferPrice;
            }
        }

        private double currentBidPrice;
        private double currentOfferPrice;

        private int loggedRowsCounter;
        public int LoggedRowsCounter
        {
            get
            {
                return this.loggedRowsCounter;
            }
        }

        public BidAndOfferSingleRowLogger(string symbol, int rowIndex, OrderBookContext orderBook, ILogger logger)
        {
            this.symbol = symbol;
            this.rowIndex = rowIndex;
            this.orderBook = orderBook;
            this.logger = logger;

            this.orderBook.OnQuotesUpdate += new SymbolDataUpdatedNotification(OnChange);
        }

        private void OnChange(string symbol)
        {
            if (this.symbol != symbol)
                return;

            UpdateCurrentRowPrices();

            if (CurrentRowPricesAreSameAsPrevious())
                return;

            CultureInfo ci = CultureInfo.InvariantCulture;

            string message =
                String.Format("{0:dd/MM/yyyy H:mm:ss.fff},{1},{2}",
                BrokerDateTime.Make(DateTime.Now),
                this.currentBidPrice.ToString("0.0000", ci),
                this.currentOfferPrice.ToString("0.0000", ci));

            this.logger.Log(message);
            this.loggedRowsCounter++;

            UpdatePreviousRowPricesWithCurrent();
        }

        private void UpdatePreviousRowPricesWithCurrent()
        {
            this.previousBidPrice = this.currentBidPrice;
            this.previousOfferPrice = this.currentOfferPrice;
        }

        private void UpdateCurrentRowPrices()
        {
            this.currentBidPrice = this.orderBook.GetBidPrice(this.symbol, this.rowIndex);
            this.currentOfferPrice = this.orderBook.GetOfferPrice(this.symbol, this.rowIndex);
        }

        private bool CurrentRowPricesAreSameAsPrevious()
        {
            return this.previousBidPrice == this.currentBidPrice 
                && this.previousOfferPrice == this.currentOfferPrice;
        }
    }
}
