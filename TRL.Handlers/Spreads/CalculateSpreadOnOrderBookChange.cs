using TRL.Common.Data;
using TRL.Common.Data.Spreads;
using TRL.Common.Models;
using TRL.Common.TimeHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Collections;
using TRL.Logging;
using TRL.Common;

namespace TRL.Handlers.Spreads
{
    public class CalculateSpreadOnOrderBookChange
    {
        private ArbitrageSettings arbitrageSettings;
        private OrderBookContext orderBook;
        private IDataContext tradingData;
        private ILogger logger;

        public CalculateSpreadOnOrderBookChange(ArbitrageSettings arbitrageSettings, OrderBookContext orderBook, IDataContext tradingData, ILogger logger)
        {
            this.arbitrageSettings = arbitrageSettings;
            this.orderBook = orderBook;
            this.tradingData = tradingData;
            this.logger = logger;

            this.orderBook.OnQuotesUpdate += new SymbolDataUpdatedNotification(CalculateSpreadOnQuotesUpdate);
        }

        public void CalculateSpreadOnQuotesUpdate(string symbol)
        {
            if (!arbitrageSettings.HasSymbol(symbol))
                return;

            double buySpreadPrice = new BuySpreadFactory(this.arbitrageSettings.LeftLeg, this.arbitrageSettings.RightLeg, this.orderBook).Make();
            double sellSpreadPrice = new SellSpreadFactory(this.arbitrageSettings.LeftLeg, this.arbitrageSettings.RightLeg, this.orderBook).Make();

            if (buySpreadPrice == 0 || sellSpreadPrice == 0)
                return;

            SpreadValue lastSpread = GetLastSpreadValue();
          
            if (lastSpread != null
                && lastSpread.BuyBeforePrice == buySpreadPrice
                && lastSpread.SellAfterPrice == sellSpreadPrice)
                return;

            SpreadValue spreadValue = new SpreadValue(arbitrageSettings.Id, BrokerDateTime.Make(DateTime.Now), sellSpreadPrice, buySpreadPrice);

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, выполнено вычисление нового значения {2}.", DateTime.Now, this.GetType().Name, spreadValue.ToString()));

            this.tradingData.Get<ObservableCollection<SpreadValue>>().Add(spreadValue);

        }

        private SpreadValue GetLastSpreadValue()
        {
            try
            {
                return this.tradingData.Get<IEnumerable<SpreadValue>>().Last(v => v.Id == this.arbitrageSettings.Id);
            }
            catch
            {
                return null;
            }
        }
    }
}
