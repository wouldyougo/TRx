using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using System.IO;
using TRL.Common.Models;

namespace TRL.Transaction
{
    public class ImportTradesTransaction:ITransaction
    {
        private IObservableHashSetFactory tradingData;
        private string path;

        private ImportTradesTransaction() { }

        public ImportTradesTransaction(IObservableHashSetFactory tradingData, string path)
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
                        Trade trade = Trade.Parse(line);

                        Order order = GetOrder(trade.OrderId);

                        if (order != null)
                        {
                            trade.Order = order;
                            this.tradingData.Make<Trade>().Add(trade);
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

        private Order GetOrder(int id)
        {
            try
            {
                return this.tradingData.Make<Order>().Single(s => s.Id == id);
            }
            catch
            {
                return null;
            }
        }

    }
}
