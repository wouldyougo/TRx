using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common;
using TRL.Common.Collections;
using TRL.Common.Data;
using TRL.Common.Models;

namespace TRL.Common.Test.Mocks
{
    public class FakeHistoryDataProvider:IHistoryDataProvider
    {
        private IDataContext tradingData;

        public FakeHistoryDataProvider(IDataContext tradingData)
        {
            this.tradingData = tradingData;
        }

        public void Send(IHistoryDataRequest request)
        {
            for (int i = 0; i < request.Quantity; i++)
                this.tradingData.Get<ObservableCollection<Tick>>().Add(new Tick(request.Symbol, request.FromDate.AddSeconds(-1), 1, 1));
        }
    }
}
