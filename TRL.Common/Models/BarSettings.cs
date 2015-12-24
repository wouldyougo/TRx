using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using TRL.Common.Enums;

namespace TRL.Common.Models
{
    /// <summary>
    /// Настройка бара, определяет:
    /// Код инструмента
    /// Тип бара
    /// Интервал
    /// в секундах - для времеонного интервала
    /// в единицах - для volume или range бара
    /// и т.д.
    /// </summary>
    public class BarSettings:IIdentified
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        /// <summary>
        /// Интервал;
        /// длительность бара в секундах - для временного интервала;
        /// --в единицах - для volume или range бара.
        /// </summary>
        public int Interval { get; set; }
        /// <summary>
        /// Период;
        /// начальное количество баров в списке
        /// </summary>
        public int Period { get; set; }
        public int StrategyId { get; set; }
        public virtual StrategyHeader Strategy { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="strategyHeader"></param>
        ///// <param name="symbol"></param>
        ///// <param name="interval">длительность бара</param>
        ///// <param name="period">количество баров</param>
        //public BarSettings(Strategy strategyHeader, string symbol, int interval, int period)
        //{
        //    this.Id = SerialIntegerFactory.Make();
        //    this.Symbol = symbol;
        //    this.Interval = interval;
        //    this.Period = period;
        //    this.StrategyId = strategyHeader.Id;
        //    this.Strategy = strategyHeader;
        //    this.BarSettings = DataModelType.TimeBar;
        //}

        /// <summary>
        /// BarSettings
        /// </summary>
        /// <param name="strategyHeader">strategyHeader</param>
        /// <param name="symbol">symbol</param>
        /// <param name="interval">Длительность бара</param>
        /// <param name="period">Количество баров</param>
        /// <param name="barType">Тип бара</param>
        public BarSettings(StrategyHeader strategyHeader, string symbol, int interval, int period)
            : this(strategyHeader, symbol, interval, period, DataModelType.TimeBar)
        {
        }

        /// <summary>
        /// NoTest
        /// </summary>
        /// <param name="strategyHeader">strategyHeader</param>
        /// <param name="symbol">symbol</param>
        /// <param name="interval">Длительность бара</param>
        /// <param name="period">Количество баров</param>
        /// <param name="barType">Тип бара</param>
        public BarSettings(StrategyHeader strategyHeader, string symbol, int interval, int period, DataModelType barType)
        {
            this.Id = SerialIntegerFactory.Make();
            this.Symbol = symbol;
            this.Interval = interval;
            this.Period = period;
            this.StrategyId = strategyHeader.Id;
            this.Strategy = strategyHeader;
            this.BarType = barType;
        }

        /// <summary>
        /// NoTest
        /// Тип бара
        /// </summary>
        public DataModelType BarType { get; set; }
    }
}
