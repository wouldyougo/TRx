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
using TRL.Common.Extensions;

namespace TRx.Handlers
{
    /// <summary>
    /// Интерфейс IDataInput<T>
    /// Предоставляет доступ к списку IList<T> с данными из источника данных IDataOutput<T>
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public interface IDataInput<T>
    {
        /// <summary>
        /// Данные источника данных DataOutput
        /// </summary>
        IList<T> Value { get; }
        /// <summary>
        /// Ссылка на экземпляр DataOutput
        /// </summary>
        IDataOutput<T> DataOutput { get; }
        /// <summary>
        /// Номер выхода источника данных IDataSource
        /// </summary>
        int OutputNumber  { get; }
        /// <summary>
        /// Признак готовности данных
        /// </summary>
        bool InputReady { get; set; }
    }
    /// <summary>
    /// Базовый класс реализующий IDataInput<T>
    /// Предоставляет доступ к списку IList<T> с данными из источника данных IDataOutput<T>
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public class DataInput<T>: IDataInput<T>
    {
        /// <summary>
        /// Данные источника данных DataOutput
        /// </summary>
        public IList<T> Value
        {
            get{ return DataOutput[OutputNumber]; }
        }
        /// <summary>
        /// Ссылка на экземпляр DataOutput
        /// </summary>
        public IDataOutput<T> DataOutput { get; private set; }
        /// <summary>
        /// Номер выхода источника данных IDataSource
        /// </summary>
        public int OutputNumber { get; private set; }
        /// <summary>
        /// DataInput
        /// </summary>
        /// <param name="dataOutput">Источник данных</param>
        /// <param name="i">Номер выхода источника данных</param>
        public DataInput(IDataOutput<T> dataOutput, int i = 0)
        {
            this.DataOutput = dataOutput;
            this.OutputNumber = i;
        }
        /// <summary>
        /// Признак готовности данных
        /// </summary>
        public bool InputReady { get; set; }
    }
}
