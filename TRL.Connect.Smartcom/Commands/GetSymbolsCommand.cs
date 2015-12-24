using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using SmartCOM3Lib;
using TRL.Common.TimeHelpers;
using TRL.Transaction;
using TRL.Logging;
using TRL.Common;

namespace TRL.Connect.Smartcom.Commands
{
    public class GetSymbolsCommand:ITransaction
    {
        private IGenericSingleton<StServer> singleton;
        private ILogger logger;

        public GetSymbolsCommand() :
            this(new StServerSingleton(), DefaultLogger.Instance) { }

        public GetSymbolsCommand(IGenericSingleton<StServer> singleton, ILogger logger)
        {
            this.singleton = singleton;
            this.logger = logger;
        }

        public void Execute()
        {
            this.singleton.Instance.GetSymbols();

            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, отправлен запрос на получение информации о торгуемых инструментах.", 
                BrokerDateTime.Make(DateTime.Now),
                this.GetType().Name));
        }
    }
}
