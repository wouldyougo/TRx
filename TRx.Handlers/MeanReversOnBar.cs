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
    public partial class MeanReversOnBar//:AddedItemHandler<Bar>
    {
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
            Levels.Do(maDeviation.De);
            //IList<bool> ПересеченияСверху = levels.ПересеченияСверху;
            //IList<bool> ПересеченияСнизу = levels.ПересеченияСнизу;

            ПересечениeСверху.Add(Levels.ПересечениеСверху);
            ПересечениеСнизу.Add(Levels.ПересечениеСнизу);

            Уровень.Add(Levels.УровеньТекущий);                     //уровень текщий
            УровеньПрошлый.Add(Levels.УровеньПрошлый);              //уровень предыдущий
            #endregion
            // -------------------------------------------------

            ПроверитьПризнакНаОткрытиеВерхний();
            ПроверитьПризнакНаОткрытиеНижний();

            ПроверитьПризнакНаЗакрытиеВерхний();
            ПроверитьПризнакНаЗакрытиеНижний();

            // -------------------------------------------------
            #region // Проверка сигналов по времени
            // -------------------------------------------------

            #endregion
            // -------------------------------------------------

            // -------------------------------------------------
            #region // Подача заявок
            // -------------------------------------------------
            /// Сформировать Сигналы на открытие закрытие позиций
            СформироватьСигналы();
            #endregion
            // -------------------------------------------------
        }
        /// <summary>
        /// Сформировать Сигналы на открытие закрытие позиций
        /// </summary>
        private void СформироватьСигналы()
        {
            //throw new NotImplementedException();
            // переделать
            // вопрос - как найти количество активных позиций

            double Price = maDeviation.bar.Close;
            int i = Уровень.Count - 1;
            int MaxLevelPositionCount = 1;
            int ActivePositionCount = 0;
            ActivePositionCount = LevelStack[Уровень[i]].Count;
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
                    LevelStack[Уровень[i]].Push(Sell);
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
                    LevelStack[Уровень[i]].Push(Buy);
                }
            }
            // выполнение сигналов на закрытие короткой позиции
            if (ЗакрытиеВерхний[i])
            //if (b_sell_close)
            {
                Signal signalBuy;
                Signal lastSignalSell;
                for (int j = Levels.КоличествоУровней - 1; j > Уровень[i]; j--)
                {   //
                    Sell = null;
                    if (LevelStack[j].Count > 0)
                    {
                        Sell = LevelStack[j].Peek();
                    }
                    lastSignalSell = null;
                    if (Sell != null)
                    {
                        lastSignalSell = Sell[0];
                        LevelStack[j].Pop();
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
                    if (LevelStack[j].Count > 0)
                    {
                        Buy = LevelStack[j].Peek();
                    }
                    lastSignalBuy = null;
                    if (Buy != null)
                    {
                        lastSignalBuy = Buy[0];
                        LevelStack[j].Pop();
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
        /// Проверить Признак На Закрытие при пересечении уровней ниже середины
        /// </summary>
        private void ПроверитьПризнакНаЗакрытиеНижний()
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
                    ЗакрытиеНижний[i] = (ЗакрытиеНижний[i] && (Уровень[i - 1] < Levels.УровеньСредний));
                    if (ЗакрытиеНижний[i])
                    {
                        ЗакрытиеНижнийКоличествоУровней[i] = System.Math.Min(Уровень[i], Levels.УровеньСредний) - УровеньПрошлый[i];
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
        /// Проверить Признак На Закрытие при пересечении уровней выше середины
        /// </summary>
        private void ПроверитьПризнакНаЗакрытиеВерхний()
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
                    ЗакрытиеВерхний[i] = (ЗакрытиеВерхний[i] && (Уровень[i - 1] > Levels.УровеньСредний));
                    if (ЗакрытиеВерхний[i])
                    {
                        ЗакрытиеВерхнийКоличествоУровней[i] = УровеньПрошлый[i] - System.Math.Max(Уровень[i], Levels.УровеньСредний);
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
        /// Проверить Признак На Открытие при пересечении уровней ниже середины
        /// </summary>
        private void ПроверитьПризнакНаОткрытиеНижний()
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
                    ОткрытиеНижний[i] = (ОткрытиеНижний[i] && (Уровень[i] < Levels.УровеньСредний));
                    if (ОткрытиеНижний[i])
                    {
                        ОткрытиеНижнийКоличествоУровней[i] = System.Math.Min(УровеньПрошлый[i], Levels.УровеньСредний) - Уровень[i];
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
        /// Проверить Признак На Открытие при пересечении уровней выше сеседины
        /// </summary>
        private void ПроверитьПризнакНаОткрытиеВерхний()
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
                    ОткрытиеВерхний[i] = (ОткрытиеВерхний[i] && (Уровень[i] > Levels.УровеньСредний));
                    if (ОткрытиеВерхний[i])
                    {
                        ОткрытиеВерхнийКоличествоУровней[i] = Уровень[i] - System.Math.Max(УровеньПрошлый[i], Levels.УровеньСредний);
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
