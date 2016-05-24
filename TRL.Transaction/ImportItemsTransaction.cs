using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using TRL.Common.Collections;
using System.IO;

namespace TRL.Transaction
{
    public abstract class ImportItemsTransaction<T> : ITransaction
    {
        protected ObservableCollection<T> collection;
        protected string path;

        public ImportItemsTransaction(ObservableCollection<T> dstCollection, string path)
        {
            this.collection = dstCollection;
            this.path = path;
        }

        public void Execute()
        {
            if (!File.Exists(this.path))
                return;

            StreamReader streamReader = new StreamReader(this.path);

            ReadItemsFromStream(streamReader);

            streamReader.Close();
            streamReader.Dispose();
        }

        private void ReadItemsFromStream(StreamReader streamReader)
        {
            //читаем заголовок
            string line = streamReader.ReadLine();

            line = streamReader.ReadLine();
            while (line != null)
            {
                if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                    continue;

                T item = TryParseItem(line);
                //if(item != null)
                AddItemToCollection(item);

                line = streamReader.ReadLine();
            }
        }

        public abstract T TryParseItem(string src);

        private void AddItemToCollection(T item)
        {
            if (item == null)
                return;

            this.collection.Add(item);
        }
    }
}
