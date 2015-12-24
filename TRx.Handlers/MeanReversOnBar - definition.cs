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
        private StrategyHeader strategyHeader;
        private IDataContext tradingData;
        private ObservableQueue<Signal> signalQueue;
        private ILogger logger;

        public IndicatorOnBarMaDeviation maDeviation { get; private set; }
        public Levels Levels { get; private set; }

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
        /// Признак На Открытие при пересечении уровней выше сеседины
        /// </summary>
        public IList<bool> ОткрытиеВерхний { get; private set; }
        /// <summary>
        /// КоличествоУровней пройдено при пересечении уровней выше сеседины
        /// </summary>
        public IList<int> ОткрытиеВерхнийКоличествоУровней { get; private set; }
        /// <summary>
        /// Признак На Открытие при пересечении уровней ниже середины
        /// </summary>
        public IList<bool> ОткрытиеНижний { get; private set; }
        /// <summary>
        /// КоличествоУровней пройдено при пересечении уровней ниже середины
        /// </summary>
        public IList<int> ОткрытиеНижнийКоличествоУровней { get; private set; }
        /// <summary>
        /// Признак На Закрытие при пересечении уровней выше середины
        /// </summary>
        public IList<bool> ЗакрытиеВерхний { get; private set; }
        /// <summary>
        /// КоличествоУровней пройдено при пересечении уровней выше середины
        /// </summary>
        public IList<int> ЗакрытиеВерхнийКоличествоУровней { get; private set; }
        /// <summary>
        /// Признак На Закрытие при пересечении уровней ниже середины
        /// </summary>
        public IList<bool> ЗакрытиеНижний { get; private set; }
        /// <summary>
        /// КоличествоУровней пройдено при пересечении уровней ниже середины
        /// </summary>
        public IList<int> ЗакрытиеНижнийКоличествоУровней { get; private set; }
        /// <summary>
        /// список стеков
        /// </summary>
        public IList<Stack<IList<Signal>>> LevelStack { get; private set; }
        /// <summary>
        /// список позиции
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

            this.Levels = levels;
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
            LevelStack = new System.Collections.Generic.Stack<
                                System.Collections.Generic.IList<Signal>
                                                                        >[levels.КоличествоУровней];
            //здесь создание стеков
            for (int j = 0; j < levels.КоличествоУровней; j++)
            {   //
                LevelStack[j] = new System.Collections.Generic.Stack<
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
    }
}
