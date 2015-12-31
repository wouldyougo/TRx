using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Connector
{
    public interface IConnector
    {
        void Connect();
        void Disconnect();
        bool IsConnected { get; }
    }
}
