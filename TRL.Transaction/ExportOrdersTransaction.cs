using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TRL.Common.Models;
using TRL.Common.Data;

namespace TRL.Transaction
{
    public class ExportOrdersTransaction:ITransaction
    {
        private IObservableHashSetFactory tradingData;
        private string path;

        public ExportOrdersTransaction(IObservableHashSetFactory tradingData, string path)
        {
            this.tradingData = tradingData;
            this.path = path;
        }

        public void Execute()
        {
            if (this.tradingData.Make<Order>().Count == 0)
            {
                if (File.Exists(this.path))
                    File.Delete(this.path);

                return;
            }

            FileStream fileStream = new FileStream(this.path, FileMode.Create, FileAccess.ReadWrite);
            StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);

            foreach (Order item in this.tradingData.Make<Order>())
            {
                streamWriter.WriteLine(item.ToImportString());
                streamWriter.Flush();
            }

            streamWriter.Close();
            streamWriter.Dispose();
            fileStream.Close();
            fileStream.Dispose();
        }

    }
}
