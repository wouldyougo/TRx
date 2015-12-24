using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;
//using TRL.Common.Data;
using System.IO;

namespace TRL.Common.Data
{
    public class TickCollectionFactory:IGenericFactory<IEnumerable<Tick>>
    {
        private string path;

        public TickCollectionFactory(string path)
        {
            this.path = path;
        }

        public IEnumerable<Tick> Make()
        {
            List<Tick> collection = new List<Tick>();

            if (!File.Exists(this.path))
                return collection;

            StreamReader streamReader = new StreamReader(this.path);
            StringReader stringReader = new StringReader(streamReader.ReadToEnd());

            while (true)
            {
                string line = stringReader.ReadLine();

                if (!string.IsNullOrEmpty(line) && !string.IsNullOrWhiteSpace(line))
                {
                    try
                    {
                        collection.Add(Tick.Parse(line));
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

            return collection;
        }
    }
}
