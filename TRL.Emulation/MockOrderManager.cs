using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common;
using TRL.Common.Handlers;
using TRL.Common.Models;

namespace TRL.Emulation
{
    public class MockOrderManager:IOrderManager
    {
        private int cancelCounter;
        public int CancelCounter
        {
            get
            {
                return this.cancelCounter;
            }
        }

        private int placeCounter;
        public int PlaceCounter
        {
            get
            {
                return this.placeCounter;
            }
        }

        private int moveCounter;
        public int MoveCounter
        {
            get
            {
                return this.moveCounter;
            }
        }

        private double lastMovePrice;
        public double LastMovePrice
        {
            get
            {
                return this.lastMovePrice;
            }
        }

        public void PlaceOrder(Order order)
        {
            this.placeCounter++;
        }

        public void MoveOrder(Order order, double price)
        {
            this.moveCounter++;
            this.lastMovePrice = price;
        }

        public void CancelOrder(Order order)
        {
            this.cancelCounter++;
        }
    }
}
