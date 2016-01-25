using System;
using System.Collections.Generic;
using TRL.Logging;

namespace TRx.Handlers
{
    public class IndicatorMACDs
    {
        private IDataInput<double> Input { get; set; }
        private ILogger logger { get; set; }
        private IList<double> Period { get; set; }
        public IList<IndicatorMACDx> Macdx { get; private set; }
        /// <summary>
        /// матрица пересечений, индексами задаем где вычислять пересечения
        /// </summary>
        //public IList<IList<bool>> CrossTo { get; private set; }      
        public IList<Tuple<int, int>> CrossTo { get; private set; }
               

        //public IList<IList<IList<bool>>> CrossUp { get; private set; }
        public IList<bool>[,] CrossUp { get; private set; }
        //public IList<IList<IList<bool>>> CrossDn { get; private set; }
        public IList<bool>[,] CrossDn { get; private set; }

        public IndicatorMACDs(IList<double> period, IDataInput<double> input, ILogger logger)
        {
            this.Period = period;
            this.Input = input;
            this.logger = logger;
            //создаем список индикаторов, связываем первый с входом и между собой
            this.Macdx = new List<IndicatorMACDx>(period.Count);
            var dataInput = input;
            foreach (var p in Period)
            {
                IndicatorMACDx macdx = new IndicatorMACDx(p, dataInput, logger);
                this.Macdx.Add(macdx);
                //предыдущий является входом для следующего
                dataInput = new DataInput<double>(macdx);
            }

            CrossTo = new List<Tuple<int, int>>();
            CrossUp = new List<bool>[period.Count, period.Count];
            CrossDn = new List<bool>[period.Count, period.Count];
            for (int i = 0; i <= CrossUp.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= CrossUp.GetUpperBound(1); j++)
                {
                    CrossUp[i, j] = new List<bool>();
                    CrossDn[i, j] = new List<bool>();
                }
            }
        }
        public void Do(long id)
        {
            //throw new NotImplementedException();
            foreach (var mx in Macdx)
            {
                mx.Do(id);
            }
            foreach (var ct in CrossTo)
            {
                int i = ct.Item1;
                int j = ct.Item2;
                throw new NotImplementedException();
                CrossUp[i, j].Add(false);
                CrossDn[i, j].Add(false);
            }
        }
    }
}