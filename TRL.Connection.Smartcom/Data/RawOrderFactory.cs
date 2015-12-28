using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Connect.Smartcom.Models;
using TRL.Common.Models;
using SmartCOM3Lib;
using TRL.Common;
using TRL.Common.TimeHelpers;

namespace TRL.Connect.Smartcom.Data
{
    public class RawOrderFactory
    {
        private ITradingSchedule schedule;

        public RawOrderFactory(ITradingSchedule schedule)
        {
            this.schedule = schedule;
        }

        public RawOrder Make(Order order)
            {
            RawOrder smartComOrder = new RawOrder();

            smartComOrder.Portfolio = order.Portfolio;
            smartComOrder.Symbol = order.Symbol;
            smartComOrder.Action = Make(order.TradeAction);
            smartComOrder.Amount = order.Amount;
            smartComOrder.Type = GetStOrderType(order.OrderType);
            smartComOrder.Validity = MakeValidity(order.ExpirationDate);
            smartComOrder.Cookie = order.Id;
            smartComOrder.Id = smartComOrder.Cookie;
            smartComOrder.ExpirationDate = DateTime.Now.AddSeconds(60);

            if (smartComOrder.Type == StOrder_Type.StOrder_Type_Stop)
                smartComOrder.Stop = order.Stop;
            else if (smartComOrder.Type == StOrder_Type.StOrder_Type_Limit)
                smartComOrder.Price = order.Price;
            else
                smartComOrder.Price = 0;

            return smartComOrder;
        }

        private StOrder_Validity MakeValidity(DateTime date)
        {
            if (date > this.schedule.SessionEnd)
                return StOrder_Validity.StOrder_Validity_Gtc;

            return StOrder_Validity.StOrder_Validity_Day;
        }

        private static StOrder_Type GetStOrderType(OrderType type)
        {
            if (type == OrderType.Stop)
                return StOrder_Type.StOrder_Type_Stop;

            if (type == OrderType.Limit)
                return StOrder_Type.StOrder_Type_Limit;

            return StOrder_Type.StOrder_Type_Market;
        }

        private static StOrder_Action Make(TradeAction action)
        {
            if (action.Equals(TradeAction.Buy))
                return StOrder_Action.StOrder_Action_Buy;

            return StOrder_Action.StOrder_Action_Sell;
        }

        private static StOrder_Type Make(double price)
        {
            if (price > 0)
                return StOrder_Type.StOrder_Type_Limit;

            return StOrder_Type.StOrder_Type_Market;
        }
    }
}
