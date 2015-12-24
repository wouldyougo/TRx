using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common;
using TRL.Common.Collections;
using TRL.Common.Extensions.Collections;
using TRL.Common.Data;
using TRL.Common.Extensions.Data;
using TRL.Common.Handlers;
using TRL.Common.Models;
using TRL.Logging;
using TRL.Common.TimeHelpers;
using TRL.Common.Events;
using TRx.Indicators;
using TRx.Helpers;

namespace TRx.Handlers
{
    public class MeanReversOnBar//:AddedItemHandler<Bar>
    {
        private StrategyHeader strategyHeader;
        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;
        private ILogger logger;

        public IndicatorOnBarMaDeviation maDeviation { get; private set; }
        public Levels levels { get; private set; }

        /// <summary>
        /// Номера пересеченых уровней для каждого бара
        /// </summary>
        public IList<int> Уровень { get; private set; }
        /// <summary>
        /// Номера прошлых пересеченых уровней для каждого бара
        /// </summary>
        public IList<int> УровеньПрошлый { get; private set; }
        /// <summary>
        /// Наличие пересечения по направлению
        /// </summary>
        public IList<bool> ПересечениeСверху { get; private set; }
        /// <summary>
        /// Наличие пересечения по направлению
        /// </summary>
        public IList<bool> ПересечениеСнизу { get; private set; }
        /// <summary>
        ///
        /// </summary>
        public IList<bool> ОткрытиеВерхний { get; private set; }
        /// <summary>
        ///
        /// </summary>
        public IList<int> ОткрытиеВерхнийКоличествоУровней { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public IList<bool> ОткрытиеНижний { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public IList<int> ОткрытиеНижнийКоличествоУровней { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public IList<bool> ЗакрытиеВерхний { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public IList<int> ЗакрытиеВерхнийКоличествоУровней { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public IList<bool> ЗакрытиеНижний { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public IList<int> ЗакрытиеНижнийКоличествоУровней { get; private set; }
        /// <summary>
        /// список стеков
        /// </summary>
        public IList<Stack<IList<Signal>>> stackLevel { get; private set; }
        /// <summary>
        ///список позиции
        /// </summary>
        public IList<Signal> Sell { get; private set; }
        /// <summary>
        /// список позиции
        /// </summary>
        public IList<Signal> Buy { get; private set; }


        public MeanReversOnBar(StrategyHeader strategyHeader, 
                               IDataContext tradingData, 
                               ObservableQueue<Signal> signalQueue, 
                               ILogger logger,
                               Levels levels,
                               IndicatorOnBarMaDeviation maDeviation)
            //:base(tradingData.Get<ObservableCollection<Bar>>())
        {
            this.strategyHeader = strategyHeader;
            this.tradingData = tradingData;
            this.signalQueue = signalQueue;
            this.logger = logger;

            this.levels = levels;
            this.maDeviation = maDeviation;
            this.maDeviation.AddHandlerValueDe(this.OnValueItemAdded);

            this.Уровень = new List<int>();
            this.УровеньПрошлый = new List<int>();

            this.ПересечениeСверху = new List<bool>();
            this.ПересечениеСнизу = new List<bool>();

            ОткрытиеВерхний = new List<bool>();
            ОткрытиеНижний = new List<bool>();
            ЗакрытиеВерхний = new List<bool>();
            ЗакрытиеНижний = new List<bool>();

            ОткрытиеВерхнийКоличествоУровней = new List<int>();
            ОткрытиеНижнийКоличествоУровней = new List<int>();
            ЗакрытиеВерхнийКоличествоУровней = new List<int>();
            ЗакрытиеНижнийКоличествоУровней = new List<int>();

            //System.Collections.Generic.IList<
            //     System.Collections.Generic.Stack<
            //         System.Collections.Generic.IList<Signal>>> stackLevel;
            //это создание списка стеков
            stackLevel = new System.Collections.Generic.Stack<
                                System.Collections.Generic.IList<Signal>
                                                                        >[levels.КоличествоУровней];
            //здесь создание стеков
            for (int j = 0; j < levels.КоличествоУровней; j++)
            {   //
                stackLevel[j] = new System.Collections.Generic.Stack<
                                        System.Collections.Generic.IList<Signal>>();
            }
            //так формируем список позиции
            //Sell = new Signal[1];
            //Buy = new Signal[1];
        }
        /*
        public override void OnItemAdded(Bar item)
        {
        }*/
        /// <summary>
        /// здесь проверяем пересечение отклонением уровней
        /// </summary>
        /// <param name="item"></param>
        public void OnValueItemAdded(ValueDouble item)
        {
            //уровень текщий
            //уровень предыдущий
            // -------------------------------------------------
            #region //пересечение уровней
            // -------------------------------------------------
            levels.Do(maDeviation.De);
            //IList<bool> ПересеченияСверху = levels.ПересеченияСверху;
            //IList<bool> ПересеченияСнизу = levels.ПересеченияСнизу;

            ПересечениeСверху.Add(levels.ПересечениеСверху);
            ПересечениеСнизу.Add(levels.ПересечениеСнизу);

            Уровень.Add(levels.УровеньТекущий);                     //уровень текщий
            УровеньПрошлый.Add(levels.УровеньПрошлый);              //уровень предыдущий
            #endregion
            // -------------------------------------------------

            ОткрытиеСверху();
            ОткрытиеСнизу();

            ЗакрытиеСверху();
            ЗакрытиеСнизу();

            // -------------------------------------------------
            #region // Проверка сигналов по времени
            // -------------------------------------------------

            #endregion
            // -------------------------------------------------

            // -------------------------------------------------
            #region // Подача заявок
            // -------------------------------------------------
            СформироватьСигналы();
            #endregion
            // -------------------------------------------------
        }

        private void СформироватьСигналы()
        {
            //throw new NotImplementedException();
            // переделать
            // вопрос - как найти количество активных позиций

            double Price = maDeviation.bar.Close;
            int i = Уровень.Count - 1;
            int MaxLevelPositionCount = 1;
            int ActivePositionCount = 0;
            ActivePositionCount = stackLevel[Уровень[i]].Count;
            // выполнение сигналов для короткой позиции
            if (ОткрытиеВерхний[i])
            {
                Signal signalSell;
                // Если нет активной короткой позиции
                // выдаем ордера на открыте новой короткой позиции.
                //if (ActivePositionCount < levels.ПоловинаУровней)
                if (ActivePositionCount < MaxLevelPositionCount)
                {
                    //ОткрытиеПозицииВерхний[i] = true;
                    {
                        //Источник1.Positions.SellAtMarket(i + 1, 1D, "SellSell" + open_count);
                        signalSell = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, Price, 0, Price);
                        //signal.Amount = strategyHeader.Amount * 2;
                        this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сигнал на открытие короткой позиции {2}", DateTime.Now, this.GetType().Name, signalSell.ToString()));
                        this.signalQueue.Enqueue(signalSell);
                    }
                    Sell = new Signal[1];
                    Sell[0] = signalSell;
                    //Sell[1] = null;
                    stackLevel[Уровень[i]].Push(Sell);
                }
            }
            // выполнение сигналов для длинной позиции           
            if (ОткрытиеНижний[i])
            {
                Signal signalBuy;
                // Если нет активной длинной позиции
                // выдаем ордера на открыте новой длинной позиции
                //if (ActivePositionCount < levels.ПоловинаУровней)
                if (ActivePositionCount < MaxLevelPositionCount)
                {
                    //ОткрытиеПозицииНижний[i] = true;
                    {
                        //Источник1.Positions.BuyAtMarket(i + 1, 1D, "BuyBuy" + open_count);
                        signalBuy = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, Price, 0, Price);
                        //signal.Amount = strategyHeader.Amount * 2;
                        this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сигнал на открытие длинной позиции {2}", DateTime.Now, this.GetType().Name, signalBuy.ToString()));
                        this.signalQueue.Enqueue(signalBuy);
                    }
                    Buy = new Signal[1];
                    Buy[0] = signalBuy;
                    //Buy[1] = null;
                    stackLevel[Уровень[i]].Push(Buy);
                }
            }
            // выполнение сигналов на закрытие короткой позиции
            if (ЗакрытиеВерхний[i])
            //if (b_sell_close)
            {
                Signal signalBuy;
                Signal lastSignalSell;
                for (int j = levels.КоличествоУровней - 1; j > Уровень[i]; j--)
                {   //
                    Sell = null;
                    if (stackLevel[j].Count > 0)
                    {
                        Sell = stackLevel[j].Peek();
                    }
                    lastSignalSell = null;
                    if (Sell != null)
                    {
                        lastSignalSell = Sell[0];
                        stackLevel[j].Pop();
                    }
                    // Если есть активная короткая позиция
                    // выдаем ордера на закрыте короткой позиции
                    if ((lastSignalSell != null))
                    {
                        //if ((lastSignalSell.EntryBarNum <= i))
                        {
                            signalBuy = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Buy, OrderType.Market, Price, 0, Price);
                            //signal.Amount = strategyHeader.Amount * 2;
                            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сигнал на закрытие короткой позиции {2}", DateTime.Now, this.GetType().Name, signalBuy.ToString()));
                            this.signalQueue.Enqueue(signalBuy);

                        }
                    }
                }
            }
            // выполнение сигналов на закрытие длинной позиции
            if (ЗакрытиеНижний[i])
            //if (b_buy_close)
            {
                Signal signalSell;
                Signal lastSignalBuy;
                for (int j = 0; j < Уровень[i]; j++)
                {   //
                    Buy = null;
                    if (stackLevel[j].Count > 0)
                    {
                        Buy = stackLevel[j].Peek();
                    }
                    lastSignalBuy = null;
                    if (Buy != null)
                    {
                        lastSignalBuy = Buy[0];
                        stackLevel[j].Pop();
                    }
                    // Если есть активная длинная позиция 
                    // выдаем ордера на закрыте длинной позиции
                    if ((lastSignalBuy != null))
                    {
                        //if ((lastSignalBuy.EntryBarNum <= i))
                        {
                            signalSell = new Signal(this.strategyHeader, BrokerDateTime.Make(DateTime.Now), TradeAction.Sell, OrderType.Market, Price, 0, Price);
                            //signal.Amount = strategyHeader.Amount * 2;
                            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сигнал на закрытие длинной позиции {2}", DateTime.Now, this.GetType().Name, signalSell.ToString()));
                            this.signalQueue.Enqueue(signalSell);

                        }
                    }
                }
            }
        }

        /// <summary>
        /// Закрытие при пересечении уровней снизу
        /// </summary>
        private void ЗакрытиеСнизу()
        {
            try
            {
                ЗакрытиеНижний.Add(false);
                ЗакрытиеНижнийКоличествоУровней.Add(0);
                //int i = Уровень.Count - 1;
                int i = ЗакрытиеНижний.Count - 1;
                {
                    //ПересечСнизу
                    //Предыдущий уровень был ниже
                    //Уровень ниже середины или было пересечение середины
                    ЗакрытиеНижний[i] = ПересечениеСнизу[i];
                    ЗакрытиеНижний[i] = (ЗакрытиеНижний[i] && (Уровень[i] > УровеньПрошлый[i]));
                    ЗакрытиеНижний[i] = (ЗакрытиеНижний[i] && (Уровень[i] > Уровень[i - 1]));
                    ЗакрытиеНижний[i] = (ЗакрытиеНижний[i] && (Уровень[i - 1] < levels.УровеньСредний));
                    if (ЗакрытиеНижний[i])
                    {
                        ЗакрытиеНижнийКоличествоУровней[i] = System.Math.Min(Уровень[i], levels.УровеньСредний) - УровеньПрошлый[i];
                    }
                }
            }
            catch (System.ArgumentOutOfRangeException)
            {
                //throw new Exception("Ошибка при вычислении блока \'ЗакрытиеВерхний\'. Индекс за пределам диапазона.");
                //logger.Log("Ошибка при вычислении блока \'ЗакрытиеВерхний\'. Индекс за пределам диапазона.");
                Console.WriteLine("Ошибка при вычислении блока \'ЗакрытиеВерхний\'. Индекс за пределам диапазона.");
            }
        }
        /// <summary>
        /// Закрытие при пересечении уровней сверху
        /// </summary>
        private void ЗакрытиеСверху()
        {
            try
            {
                ЗакрытиеВерхний.Add(false);
                ЗакрытиеВерхнийКоличествоУровней.Add(0);
                //int i = Уровень.Count - 1;
                int i = ЗакрытиеВерхний.Count - 1;
                {
                    //ПересечСверху
                    //Предыдущий уровень был Выше
                    //Уровень выше середины или было пересечение середины
                    ЗакрытиеВерхний[i] = ПересечениeСверху[i];
                    ЗакрытиеВерхний[i] = (ЗакрытиеВерхний[i] && (Уровень[i] < УровеньПрошлый[i]));
                    ЗакрытиеВерхний[i] = (ЗакрытиеВерхний[i] && (Уровень[i] < Уровень[i - 1]));
                    ЗакрытиеВерхний[i] = (ЗакрытиеВерхний[i] && (Уровень[i - 1] > levels.УровеньСредний));
                    if (ЗакрытиеВерхний[i])
                    {
                        ЗакрытиеВерхнийКоличествоУровней[i] = УровеньПрошлый[i] - System.Math.Max(Уровень[i], levels.УровеньСредний);
                    }
                }
            }
            catch (System.ArgumentOutOfRangeException)
            {
                //throw new Exception("Ошибка при вычислении блока \'ЗакрытиеВерхний\'. Индекс за пределам диапазона.");
                //logger.Log("Ошибка при вычислении блока \'ЗакрытиеВерхний\'. Индекс за пределам диапазона.");
                Console.WriteLine("Ошибка при вычислении блока \'ЗакрытиеВерхний\'. Индекс за пределам диапазона.");
            }
        }
        /// <summary>
        /// Открытие при пересечении уровней снизу
        /// </summary>
        private void ОткрытиеСнизу()
        {
            try
            {
                ОткрытиеНижний.Add(false);
                ОткрытиеНижнийКоличествоУровней.Add(0);
                //int i = Уровень.Count - 1;
                int i = ОткрытиеНижний.Count - 1;
                {
                    //ПересечСверху
                    //Предыдущий уровень был Выше
                    //Уровень ниже середины
                    ОткрытиеНижний[i] = ПересечениeСверху[i];
                    ОткрытиеНижний[i] = (ОткрытиеНижний[i] && (Уровень[i] < УровеньПрошлый[i]));
                    ОткрытиеНижний[i] = (ОткрытиеНижний[i] && (Уровень[i] < Уровень[i - 1]));
                    ОткрытиеНижний[i] = (ОткрытиеНижний[i] && (Уровень[i] < levels.УровеньСредний));
                    if (ОткрытиеНижний[i])
                    {
                        ОткрытиеНижнийКоличествоУровней[i] = System.Math.Min(УровеньПрошлый[i], levels.УровеньСредний) - Уровень[i];
                    }
                }
            }
            catch (System.ArgumentOutOfRangeException)
            {
                //throw new Exception("Ошибка при вычислении блока \'ОткрытиеНижний\'. Индекс за пределам диапазона.");
                //logger.Log("Ошибка при вычислении блока \'ОткрытиеВерхний\'. Индекс за пределам диапазона.");
                Console.WriteLine("Ошибка при вычислении блока \'ОткрытиеВерхний\'. Индекс за пределам диапазона.");
            }
        }
        /// <summary>
        /// Открытие при пересечении уровней сверху
        /// </summary>
        private void ОткрытиеСверху()
        {
            try
            {
                ОткрытиеВерхний.Add(false);
                ОткрытиеВерхнийКоличествоУровней.Add(0);
                //int i = Уровень.Count - 1;
                int i = ОткрытиеВерхний.Count - 1;
                {
                    //ПересечениеСнизу
                    //Предыдущий уровень был ниже
                    //Уровень выше середины
                    ОткрытиеВерхний[i] = ПересечениеСнизу[i];
                    ОткрытиеВерхний[i] = (ОткрытиеВерхний[i] && (Уровень[i] > УровеньПрошлый[i]));
                    ОткрытиеВерхний[i] = (ОткрытиеВерхний[i] && (Уровень[i] > Уровень[i - 1]));
                    ОткрытиеВерхний[i] = (ОткрытиеВерхний[i] && (Уровень[i] > levels.УровеньСредний));
                    if (ОткрытиеВерхний[i])
                    {
                        ОткрытиеВерхнийКоличествоУровней[i] = Уровень[i] - System.Math.Max(УровеньПрошлый[i], levels.УровеньСредний);
                    }
                }
            }
            catch (System.ArgumentOutOfRangeException)
            {
                //throw new Exception("Ошибка при вычислении блока \'ОткрытиеВерхний\'. Индекс за пределам диапазона.");
                //logger.Log("Ошибка при вычислении блока \'ОткрытиеВерхний\'. Индекс за пределам диапазона.");
                Console.WriteLine("Ошибка при вычислении блока \'ОткрытиеВерхний\'. Индекс за пределам диапазона.");
            }
        }
    }
}
