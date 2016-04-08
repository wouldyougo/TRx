using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
//using TRL.Common.Data;
//using TRL.Common.Extensions.Models;
using TRL.Common.Extensions;

namespace TRL.Common.Models
{
    public class BarsFactory
    {
        public static Bar MakeBar(IEnumerable<Tick> ticks, DateTime barDate)
        {
            double open = ticks.First().Price;
            double close = ticks.Last().Price;

            IEnumerable<Tick> orderedByPrice = OrderByPrice(ticks);

            double high = orderedByPrice.Last().Price;
            double low = orderedByPrice.First().Price;

            double volume = orderedByPrice.Sum(i => i.Volume);

            return new Bar{ Symbol = ticks.ElementAt(0).Symbol, DateTime = barDate, Open = open, High = high, Low = low, Close = close, Volume = volume};            
        }

        public static IEnumerable<Bar> Make(string fileName)
        {
            List<Bar> bars = new List<Bar>();

            if (File.Exists(fileName))
            {
                FileInfo fileInfo = new FileInfo(fileName);

                StreamReader streamReader = new StreamReader(fileName);
                StringReader stringReader = new StringReader(streamReader.ReadToEnd());


                while (true)
                {
                    string line = stringReader.ReadLine();

                    if (!string.IsNullOrEmpty(line) && !string.IsNullOrWhiteSpace(line))
                    {
                        try
                        {
                            //bars.Add(Bar.Parse(line));
                            bars.Add(Bar.Parse(line));
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
            }

            return bars;
        }

        public static IEnumerable<Tick> OrderByPrice(IEnumerable<Tick> ticks)
        {
            return ticks.OrderBy(i => i.Price);
        }

        public static IEnumerable<Tick> OrderByDateTime(IEnumerable<Tick> ticks)
        {
            return ticks.OrderBy(i => i.DateTime);
        }

        public static IEnumerable<Tick> GetTicksInterval(IEnumerable<Tick> ticks, long begin, long end)
        {

            return from t in OrderByDateTime(ticks)
                   where (t.DateTime.Ticks > begin && t.DateTime.Ticks < end)
                   select t;

        }
    }
}
