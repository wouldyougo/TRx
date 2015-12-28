using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using System.Reflection;
using TRL.Connect.Smartcom.Models;
using TRL.Common.Collections;

namespace TRL.Connect.Smartcom.Data
{
    public class RawTradingDataContext:BaseDataContext
    {
        public ObservableCollection<OrderFailed> OrderFailed { get; private set; }
        public ObservableCollection<OrderSucceeded> OrderSucceeded { get; private set; }
        public ObservableCollection<UpdateOrder> UpdateOrder { get; private set; }
        public ObservableCollection<SetPortfolio> SetPortfolio { get; private set; }
        public ObservableCollection<TradeInfo> Trade { get; private set; }
        public ObservableCollection<RawSymbol> Symbols { get; private set; }
        public ObservableCollection<OrderMoveSucceeded> OrderMoveSucceeded { get; private set; }
        public ObservableCollection<OrderMoveFailed> OrderMoveFailed { get; private set; }
        public ObservableCollection<CookieToOrderNoAssociation> ExpectedTradeInfo { get; private set; }
        public ObservableCollection<PendingTradeInfo> ExpectedUpdateOrder { get; private set; }

        public RawTradingDataContext()
        {
            this.OrderFailed = new ObservableCollection<OrderFailed>();
            this.OrderSucceeded = new ObservableCollection<OrderSucceeded>();
            this.UpdateOrder = new ObservableCollection<UpdateOrder>();
            this.SetPortfolio = new ObservableCollection<SetPortfolio>();
            this.Trade = new ObservableCollection<TradeInfo>();
            this.Symbols = new ObservableCollection<RawSymbol>();
            this.OrderMoveSucceeded = new ObservableCollection<OrderMoveSucceeded>();
            this.OrderMoveFailed = new ObservableCollection<OrderMoveFailed>();
            this.ExpectedTradeInfo = new ObservableCollection<CookieToOrderNoAssociation>();
            this.ExpectedUpdateOrder = new ObservableCollection<PendingTradeInfo>();
        }
    }
}
