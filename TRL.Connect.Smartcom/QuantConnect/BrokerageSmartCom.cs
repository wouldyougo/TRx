/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System;
using System.Collections.Generic;
//using QuantConnect.Brokerages;
using TRL.Connect.Smartcom;
using TRL.Common.Models;
//using QuantConnect.Logging;
//using QuantConnect.Orders;
//using QuantConnect.Securities;

namespace QuantConnect.Brokerages.SmartCom
{
    /// <summary>
    /// Represents the SmartCom Brokerage implementation. This provides logging on brokerage events.
    /// </summary>
    public class BrokerageSmartCom : Brokerage
    {
        public SmartComOrderManager manager { get; set; }

        private SmartComAdapter adapter { get; set; }

        /// <summary>
        /// Event that fires each time an order is filled
        /// </summary>
        //public event EventHandler<OrderEvent> OrderStatusChanged;

        /// <summary>
        /// Event that fires each time a user's brokerage account is changed
        /// </summary>
        //public event EventHandler<AccountEvent> AccountChanged;

        /// <summary>
        /// Event that fires when an error is encountered in the brokerage
        /// </summary>
        //public event EventHandler<BrokerageMessageEvent> Message;

        /// <summary>
        /// Returns true if we're currently connected to the broker
        /// </summary>
        public override bool IsConnected
        {
            get { //throw new NotImplementedException();
                return this.adapter.IsConnected;
            }
        }

        /// <summary>
        /// Creates a new SmartCom Brokerage instance with the specified name
        /// </summary>
        protected BrokerageSmartCom():base("SmartCom")
        {
            //Name = name;
        }

        /// <summary>
        /// Places a new order and assigns a new broker ID to the order
        /// </summary>
        /// <param name="order">The order to be placed</param>
        /// <returns>True if the request for a new order has been placed, false otherwise</returns>
        public override bool PlaceOrder(Order order) {
            throw new NotImplementedException();
            try
            {
                manager.PlaceOrder(order);
                //Log.Trace("BrokerageSmartCom.PlaceOrder(): Symbol: " + order.Symbol.Value + " Quantity: " + order.Quantity);
                return true;
            }
            catch (Exception err)
            {
                throw new NotImplementedException();
                //Log.Error("BrokerageSmartCom.PlaceOrder(): " + err);
                return false;
            }
        }

        /// <summary>
        /// Updates the order with the same id
        /// </summary>
        /// <param name="order">The new order information</param>
        /// <returns>True if the request was made for the order to be updated, false otherwise</returns>
        public override bool UpdateOrder(Order order) {
            throw new NotImplementedException();
            try
            {
                //здесь ошибка:
                manager.MoveOrder(order, order.Price);
                //Log.Trace("BrokerageSmartCom. UpdateOrder(): Symbol: " + order.Symbol.Value + " Quantity: " + order.Quantity);
                return true;
            }
            catch (Exception err)
            {
                throw new NotImplementedException();
                //Log.Error("BrokerageSmartCom. UpdateOrder(): " + err);
                return false;
            }
        }

        /// <summary>
        /// Cancels the order with the specified ID
        /// </summary>
        /// <param name="order">The order to cancel</param>
        /// <returns>True if the request was made for the order to be canceled, false otherwise</returns>
        public override bool CancelOrder(Order order) {
            throw new NotImplementedException();
            try
            {
                //Log.Trace("BrokerageSmartCom.CancelOrder(): Symbol: " + order.Symbol.Value + " Quantity: " + order.Quantity);
                manager.CancelOrder(order);
                // canceled order events fired upon confirmation, see HandleError
            }
            catch (Exception err)
            {
                //Log.Error("BrokerageSmartCom.CancelOrder(): OrderID: " + order.Id + " - " + err);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Connects the client to the broker's remote servers
        /// </summary>
        public override void Connect() {
            //throw new NotImplementedException();
            this.adapter.Start();
        }

        /// <summary>
        /// Disconnects the client from the broker's remote servers
        /// </summary>
        public override void Disconnect() {
            //throw new NotImplementedException();
            this.adapter.Stop();
        }

        /// <summary>
        /// Event invocator for the OrderFilled event
        /// </summary>
        /// <param name="e">The OrderEvent</param>
        //protected virtual void OnOrderEvent(OrderEvent e)
        //{
        //    try
        //    {
        //        Log.Debug("Brokerage.OnOrderEvent(): " + e);

        //        var handler = OrderStatusChanged;
        //        if (handler != null) handler(this, e);
        //    }
        //    catch (Exception err)
        //    {
        //        Log.Error(err);
        //    }
        //}

        /// <summary>
        /// Event invocator for the AccountChanged event
        /// </summary>
        /// <param name="e">The AccountEvent</param>
        //protected virtual void OnAccountChanged(AccountEvent e)
        //{
        //    try
        //    {
        //        Log.Trace("Brokerage.OnAccountChanged(): " + e);

        //        var handler = AccountChanged;
        //        if (handler != null) handler(this, e);
        //    }
        //    catch (Exception err)
        //    {
        //        Log.Error(err);
        //    }
        //}

        /// <summary>
        /// Event invocator for the Message event
        /// </summary>
        /// <param name="e">The error</param>
        //protected virtual void OnMessage(BrokerageMessageEvent e)
        //{
        //    try
        //    {
        //        if (e.Type == BrokerageMessageType.Error)
        //        {
        //            Log.Error("Brokerage.OnMessage(): " + e);
        //        }
        //        else
        //        {
        //            Log.Trace("Brokerage.OnMessage(): " + e);
        //        }

        //        var handler = Message;
        //        if (handler != null) handler(this, e);
        //    }
        //    catch (Exception err)
        //    {
        //        Log.Error(err);
        //    }
        //}

        /// <summary>
        /// Gets all open orders on the account. 
        /// NOTE: The order objects returned do not have QC order IDs.
        /// </summary>
        /// <returns>The open orders returned from IB</returns>
        public override List<Order> GetOpenOrders() {
            throw new NotImplementedException();
            /*
            var orders = new List<Order>();

            var manualResetEvent = new ManualResetEvent(false);

            // define our handlers
            EventHandler<IB.OpenOrderEventArgs> clientOnOpenOrder = (sender, args) =>
            {
                // convert IB order objects returned from RequestOpenOrders
                orders.Add(ConvertOrder(args.Order, args.Contract));
            };
            EventHandler<EventArgs> clientOnOpenOrderEnd = (sender, args) =>
            {
                // this signals the end of our RequestOpenOrders call
                manualResetEvent.Set();
            };

            _client.OpenOrder += clientOnOpenOrder;
            _client.OpenOrderEnd += clientOnOpenOrderEnd;
            
            _client.RequestOpenOrders();

            // wait for our end signal
            if (!manualResetEvent.WaitOne(15000))
            {
                throw new TimeoutException("InteractiveBrokersBrokerage.GetOpenOrders(): Operation took longer than 15 seconds.");
            }

            // remove our handlers
            _client.OpenOrder -= clientOnOpenOrder;
            _client.OpenOrderEnd -= clientOnOpenOrderEnd;

            return orders;
            */
        }

        /// <summary>
        /// Gets all holdings for the account
        /// </summary>
        /// <returns>The current holdings from the account</returns>
        //public abstract List<Holding> GetAccountHoldings();

        /// <summary>
        /// Gets the current cash balance for each currency held in the brokerage account
        /// </summary>
        /// <returns>The current cash balance for each currency available for trading</returns>
        //public abstract List<Cash> GetCashBalance();
    }
}
