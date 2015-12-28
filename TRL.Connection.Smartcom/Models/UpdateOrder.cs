using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartCOM3Lib;

namespace TRL.Connect.Smartcom.Models
{
    public class UpdateOrder
    {
        private string portfolio;
        public string Portfolio { 
            get
            {
                return this.portfolio;
            }
        }

        private string symbol;
        public string Symbol {
            get
            {
                return this.symbol;
            }
        }

        private StOrder_State state;
        public StOrder_State State {
            get
            {
                return this.state;
            }
        }

        private StOrder_Action action;
        public StOrder_Action Action {
            get
            {
                return this.action;
            }
        }

        private StOrder_Type type;
        public StOrder_Type Type {
            get
            {
                return this.type;
            }
        }

        private StOrder_Validity validity;
        public StOrder_Validity Validity {
            get
            {
                return this.validity;
            }
        }

        private double price;
        public double Price {
            get
            {
                return this.price;
            }
        }

        private double orderAmount;
        public double OrderAmount {
            get
            {
                return this.orderAmount;
            }
        }

        private double stop;
        public double Stop {
            get
            {
                return this.stop;
            }
        }

        private double orderUnfilled;
        public double OrderUnfilled {
            get
            {
                return this.orderUnfilled;
            }
        }

        private DateTime datetime;
        public DateTime Datetime {
            get
            {
                return this.datetime;
            }
        }

        private string orderId;
        public string OrderId {
            get
            {
                return this.orderId;
            }
        }

        private string orderNo;
        public string OrderNo {
            get
            {
                return this.orderNo;
            }
        }

        private int statusMask;
        public int StatusMask {
            get
            {
                return this.statusMask;
            }
        }

        private int cookie;
        public int Cookie {
            get
            {
                return this.cookie;
            }
        }

        private UpdateOrder() { }

        public UpdateOrder(string portfolio, string symbol, StOrder_State state, StOrder_Action action, StOrder_Type type, StOrder_Validity validity, double price, double amount, double stop, double filled, DateTime date, string orderId, string orderNo, int status_mask, int cookie)
        {
            this.portfolio = portfolio;
            this.symbol = symbol;
            this.state = state;
            this.action = action;
            this.type = type;
            this.validity = validity;
            this.price = price;
            this.orderAmount = amount;
            this.stop = stop;
            this.orderUnfilled = filled;
            this.datetime = date;
            this.orderId = orderId;
            this.orderNo = orderNo;
            this.statusMask = status_mask;
            this.cookie = cookie;
        }
    }
}
