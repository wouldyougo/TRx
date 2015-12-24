using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using TRL.Common.Data;
//using TRL.Common.Models;
using System.IO;

namespace TRL.Common.Data
{
    public class TextFileStringListFactory:IGenericFactory<IEnumerable<string>>
    {
        private string path;

        public TextFileStringListFactory(string path)
        {
            this.path = path;
        }

        public IEnumerable<string> Make()
        {
            List<string> result = new List<string>();

            if (!File.Exists(this.path))
                return result;

            StreamReader streamReader = new StreamReader(this.path);
            StringReader stringReader = new StringReader(streamReader.ReadToEnd());

            while (true)
            {
                string line = stringReader.ReadLine();

                if (!string.IsNullOrEmpty(line) && !string.IsNullOrWhiteSpace(line))
                {
                    result.Add(line);
                }
                else
                    break;
            }

            stringReader.Close();
            stringReader.Dispose();
            streamReader.Close();
            streamReader.Dispose();

            return result;
        }
    }
}
