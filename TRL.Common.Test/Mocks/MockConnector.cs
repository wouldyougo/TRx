using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Message;

namespace TRL.Common.Test.Mocks
{
    public class MockConnector:IConnector
    {
        private bool isConnected;
        private bool connectionLost;

        public void Connect()
        {
            this.isConnected = true;
            this.connectionLost = false;
        }

        public void Disconnect()
        {
            this.isConnected = false;
            this.connectionLost = false;
        }

        public bool IsConnected
        {
            get
            {
                return this.isConnected;
            }
        }

        public void EmulateDisconnect()
        {
            this.isConnected = false;
            this.connectionLost = true;
        }

        public bool ConnectionLost
        {
            get
            {
                return this.connectionLost;
            }
        }
    }
}
