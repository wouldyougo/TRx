using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using SmartCOM3Lib;
using TRL.Connect.Smartcom.Data;
using TRL.Common.TimeHelpers;
using TRL.Transaction;
using TRL.Logging;
using TRL.Common;

namespace TRL.Connect.Smartcom.Commands
{
    public class GetBarsCommand:ITransaction
    {
        private string symbol;
        private int barIntervalSeconds;
        private int barQuantity;
        private IGenericSingleton<StServer> stServerSingleton;
        private ILogger logger;

        public GetBarsCommand(string symbol, int barIntervalSeconds, int barQuantity)
            : this(symbol, barIntervalSeconds, barQuantity, new StServerSingleton(), DefaultLogger.Instance) { }

        public GetBarsCommand(string symbol, int barIntervalSeconds, int barQuantity, IGenericSingleton<StServer> stServerSingleton, ILogger logger)
        {
            this.symbol = symbol;
            this.barIntervalSeconds = barIntervalSeconds;
            this.barQuantity = barQuantity;
            this.stServerSingleton = stServerSingleton;
            this.logger = logger;
        }

        public void Execute()
        {
            this.stServerSingleton.Instance.GetBars(this.symbol, StBarIntervalFactory.Make(this.barIntervalSeconds), BrokerDateTime.Make(DateTime.Now), this.barQuantity);
            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, отправлен запрос на получение Bar-ов {2}, {3}, {4}",
                BrokerDateTime.Make(DateTime.Now), 
                this.GetType().Name, 
                this.symbol, 
                this.barIntervalSeconds, 
                this.barQuantity));
        }
    }
}
