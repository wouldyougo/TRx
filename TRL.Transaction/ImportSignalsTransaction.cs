using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using System.IO;
using TRL.Common.Models;

namespace TRL.Transaction
{
    public class ImportSignalsTransaction:ITransaction
    {
        private IDataContext tradingData;
        private string importFileName;

        private ImportSignalsTransaction() { }

        public ImportSignalsTransaction(IDataContext tradingData, string importFileName)
        {
            this.tradingData = tradingData;
            this.importFileName = importFileName;
        }

        public void Execute()
        {
            if (!File.Exists(this.importFileName))
                return;

            StreamReader streamReader = new StreamReader(this.importFileName);
            StringReader stringReader = new StringReader(streamReader.ReadToEnd());

            while (true)
            {
                string line = stringReader.ReadLine();

                if (!string.IsNullOrEmpty(line) && !string.IsNullOrWhiteSpace(line))
                {
                    try
                    {
                        Signal signal = Signal.Parse(line);

                        StrategyHeader strategyHeader = GetStrategy(signal.StrategyId);

                        if (strategyHeader != null)
                        {
                            signal.Strategy = strategyHeader;
                            this.tradingData.Get<ICollection<Signal>>().Add(signal);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                    break;
            }

            stringReader.Close();
            stringReader.Dispose();
            streamReader.Close();
            streamReader.Dispose();
        }

        private StrategyHeader GetStrategy(int id)
        {
            try
            {
                return this.tradingData.Get<IEnumerable<StrategyHeader>>().Single(s => s.Id == id);
            }
            catch
            {
                return null;
            }
        }

    }
}
