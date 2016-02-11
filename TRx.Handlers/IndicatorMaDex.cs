using System;
using System.Collections.Generic;
using TRL.Logging;
using TRx.Indicators;

namespace TRx.Handlers
{
    /// <summary>
    /// Обработчик появления новых данных
    /// Последовательный конвейер вычисления 
    /// Вычисляет последовательные средние за период
    /// Вычисляет отклонение от средних за период
    /// </summary>
    public class IndicatorMaDex
    {
        private ILogger logger { get; set; }

        /// <summary>
        /// Вход индикатора
        /// </summary>
        private IDataInput<double> Input { get; set; }
        /// <summary>
        /// Список периодов для создания индикаторов MAx
        /// </summary>
        private IList<double> Period { get; set; }
        /// <summary>
        /// Список индикаторов MAx
        /// </summary>
        public IList<IndicatorMaDe> MAx { get; private set; }
        /// <summary>
        /// Список для опеределения пересечений, значениями списка I,J задаем где вычислять пересечения
        /// </summary>
        public IList<Tuple<int, int>> CrossTo { get; private set; }
        /// матрица пересечений, индексами задаем где вычислять пересечения
        //public IList<IList<bool>> CrossTo { get; private set; }      

        /// <summary>
        /// Пересечение Снизу = CrossUnder = CrossUp
        /// CrossUp
        /// массив [I,J] списков List<bool> CrossUp
        /// </summary>
        public IList<bool>[,] CrossUp { get; private set; }
        /// <summary>
        /// Пересечение Сверху = CrossOver = CrossDn
        /// массив [I,J] списков List<bool> CrossDn
        /// </summary>
        public IList<bool>[,] CrossDn { get; private set; }
        //public IList<IList<IList<bool>>> CrossUp { get; private set; }
        //public IList<IList<IList<bool>>> CrossDn { get; private set; }

        /// <summary>
        /// Конструктор индикатора MAxs
        /// </summary>
        /// <param name="period">Список периодов для создания индикаторов MAx</param>
        /// <param name="input">Вход индикатора</param>
        /// <param name="logger">Логгер</param>
        public IndicatorMaDex(IList<double> period, IDataInput<double> input, ILogger logger)
        {
            this.Period = period;
            this.Input = input;
            this.logger = logger;
            //создаем список индикаторов, связываем первый с входом и между собой
            this.MAx = new List<IndicatorMaDe>(period.Count);
            var dataInput = input;

            //Создаем список индикаторов MAx, по количеству Period.Count
            foreach (var p in Period)
            {
                IndicatorMaDe ma_x = new IndicatorMaDe(p, dataInput, logger);
                this.MAx.Add(ma_x);
                //предыдущий является входом для следующего
                dataInput = new DataInput<double>(ma_x);
            }

            CrossTo = new List<Tuple<int, int>>();
            /// создаем массив [I,J] списков List<bool> CrossUp
            /// создаем массив [I,J] списков List<bool> CrossDn
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
        /// <summary>
        /// Определяем пересечения по Списку CrossTo
        /// значенийя элемента списка i,j задают индексы, где требуется вычислять пересечения
        /// </summary>
        /// <param name="id"></param>
        public void Do(long id)
        {
            //throw new NotImplementedException();
            foreach (var mx in MAx)
            {
                mx.Do(id);
            }
            foreach (var ct in CrossTo)
            {
                //throw new NotImplementedException();
                int i = ct.Item1;
                int j = ct.Item2;

                IList<double> src1 = MAx[i].Ma;
                IList<double> src2 = MAx[j].Ma;
                /// Пересечение Снизу = CrossUnder = CrossUp
                bool crossUp = Indicator.CrossUnder(src1, src2);
                /// Пересечение Сверху = CrossOver = CrossDn
                bool crossDn = Indicator.CrossOver(src1, src2);

                CrossUp[i, j].Add(crossUp);
                CrossDn[i, j].Add(crossDn);
            }
        }
    }
}