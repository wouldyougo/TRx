using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using System.IO;
using TRL.Common.Models;

namespace TRL.Transaction
{
    public class ExportTradesTransaction:ITransaction
    {
        private IObservableHashSetFactory tradingData;
        private string path;

        public ExportTradesTransaction(IObservableHashSetFactory tradingData, string path)
        {
            this.tradingData = tradingData;
            this.path = path;
        }

        public void Execute()
        {
            if (this.tradingData.Make<Trade>().Count == 0)
            {
                if (File.Exists(this.path))
                    File.Delete(this.path);

                return;
            }

            FileStream fileStream = new FileStream(this.path, FileMode.Create, FileAccess.ReadWrite);
            StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);

            foreach (Trade item in this.tradingData.Make<Trade>())
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
