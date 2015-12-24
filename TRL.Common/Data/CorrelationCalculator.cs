using TRL.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using TRL.Common.Extensions;
using TRL.Common.Extensions.Data;

namespace TRL.Common.Data
{
    public class CorrelationCalculator
    {
        private string srcFile;
        private string dstFile;

        public CorrelationCalculator(string sourceFile, string destinationFile)
        {
            srcFile = sourceFile;
            dstFile = destinationFile;
        }

        public void Calculate(List<double> pattern)
        {
            List<Tick> ticks = new List<Tick>(100000);

            using (StreamReader reader = new StreamReader(srcFile, Encoding.ASCII, false))
            {
                while (!reader.EndOfStream)
                    ticks.Add(Tick.Parse(reader.ReadLine()));
            }

            List<double> prices = ticks.Select(i => i.Price).ToList<double>();

            int maxOffset = prices.Count - pattern.Count;

            List<double> correlations = new List<double>(maxOffset);

            for (int i = 0; i < prices.Count - pattern.Count; i++)
                correlations.Add(prices.CalculatePearsonCorrelation(i, pattern));

            using (StreamWriter writer = new StreamWriter(dstFile, true, Encoding.ASCII, 4096))
            {
                for (int i = 0; i < maxOffset; i++)
                {
                    if (correlations[i] > 0.95)
                        writer.WriteLine(String.Format("{0}, {1}", correlations[i], prices[i + pattern.Count]));

                    if (correlations[i] > 0.75 && correlations[i] < 0.76)
                        writer.WriteLine(String.Format("{0}, {1}", correlations[i], prices[i + pattern.Count]));
                }
                //    writer.WriteLine(value.ToString());
            }
        }
    }
}
