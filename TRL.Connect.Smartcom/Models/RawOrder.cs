using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartCOM3Lib;
using TRL.Common.Models;

namespace TRL.Connect.Smartcom.Models
{
    public class RawOrder
    {
        public string Portfolio { get; set; }

        public string Symbol { get; set; }

        public StOrder_State State { get; set; }

        public StOrder_Action Action { get; set; }

        public StOrder_Type Type { get; set; }

        public StOrder_Validity Validity { get; set; }

        public double Price { get; set; }

        public Double Amount { get; set; }

        public Double Stop { get; set; }

        public Double Filled { get; set; }

        public DateTime DateTime { get; set; }

        public string OrderNo { get; set; }

        public int Cookie { get; set; }

        public DateTime ExpirationDate { get; set; }

        public TradeAction TradeAction { get; set; }

        public OrderType OrderType { get; set; }

        public bool RequestsOpenPosition()
        {
            if (this.Action.Equals(StOrder_Action.StOrder_Action_Buy))
                return true;

            if (this.Action.Equals(StOrder_Action.StOrder_Action_Short))
                return true;

            return false;
        }

        public bool RequestsClosePosition()
        {
            if (this.Action.Equals(StOrder_Action.StOrder_Action_Cover))
                return true;

            if (this.Action.Equals(StOrder_Action.StOrder_Action_Sell))
                return true;

            return false;
        }

        public int Id { get; set; }

        public bool IsExpired
        {
            get { return DateTime.Now > this.ExpirationDate; }
        }

        public bool IsFilled
        {
            get { return false; }
        }
    }
}
