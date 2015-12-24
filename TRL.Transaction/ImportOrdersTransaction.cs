using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using System.IO;
using TRL.Common.Models;

namespace TRL.Transaction
{
    public class ImportOrdersTransaction:ITransaction
    {
        private IObservableHashSetFactory tradingData;
        private string path;

        private ImportOrdersTransaction() { }

        public ImportOrdersTransaction(IObservableHashSetFactory tradingData, string path)
        {
            this.tradingData = tradingData;
            this.path = path;
        }

        public void Execute()
        {
            if (!File.Exists(this.path))
                return;

            StreamReader streamReader = new StreamReader(this.path);
            StringReader stringReader = new StringReader(streamReader.ReadToEnd());

            while (true)
            {
                string line = stringReader.ReadLine();

                if (!string.IsNullOrEmpty(line) && !string.IsNullOrWhiteSpace(line))
                {
                    try
                    {
                        Order order = Order.Parse(line);

                        Signal signal = GetSignal(order.SignalId);

                        if (signal != null)
                        {
                            order.Signal = signal;
                            this.tradingData.Make<Order>().Add(order);
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

        private Signal GetSignal(int id)
        {
            try
            {
                return this.tradingData.Make<Signal>().Single(s => s.Id == id);
            }
            catch
            {
                return null;
            }
        }

    }
}
