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
    /// Интерфейс IDataOutput<T> Выход данных
    /// Предоставляет доступ по индексу i к [i] списку IList<T> с данными
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public interface IDataOutput<T>
    {
        /// <summary>
        /// Предоставляет доступ по индексу i к [i] списку IList<T> с данными
        /// </summary>
        /// <param name="index">индекс для доступа к [i] списку IList<T></param>
        /// <returns></returns>
        IList<T> this[int index]
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Базовый класс реализующий IDataOutput<T> Выход данных
    /// Предоставляет доступ по индексу i к [i] списку IList<T> с данными
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public class DataOutput<T> : IDataOutput<T>
    {
        /// <summary>
        /// Предоставляет доступ по индексу i к [i] списку IList<T> с данными
        /// </summary>
        protected IList<T>[] Output { get; set; }
        /// <summary>
        /// Предоставляет доступ по индексу i к [i] списку IList<T> с данными
        /// </summary>
        /// <param name="index">индекс i для доступа к [i] списку IList<T> с данными</param>
        /// <returns>IList<T></returns>
        public IList<T> this[int index]   // indexer declaration
        {
            get
            {
                // The Source object will throw IndexOutOfRange exception.
                return Output[index];
            }
            set
            {
                Output[index] = value;
            }
        }
        /// <summary>
        /// Конструктор выхода данных
        /// </summary>
        /// <param name="OutputCount">Количество списков IList<T> с данными</param>
        public DataOutput(int OutputCount = 1)
        {
            Output = new IList<T>[OutputCount];
        }
    }

    /// <summary>
    /// Базовый класс реализующий IDataOutput<T> Выход данных
    /// Предоставляет доступ по индексу i к [i] списку IList<T> с данными
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public class DataOutput1<T> : IDataOutput<T>
    {
        /// <summary>
        /// Пример списка данных
        /// </summary>
        protected IList<T> Source { get; set; }
        /// <summary>
        /// Предоставляет доступ по индексу i к [i] списку IList<T> с данными
        /// </summary>
        /// <param name="index">индекс i для доступа к [i] списку IList<T> с данными</param>
        /// <returns>IList<T></returns>
        public IList<T> this[int index]   // indexer declaration
        {
            get
            {
                if (index == 0)
                {
                    return this.Source;
                }
                else {
                    throw new IndexOutOfRangeException();
                }
            }
            set
            {
                if (index == 0)
                {
                    this.Source = value;
                }
                else {
                    throw new IndexOutOfRangeException();
                }
            }
        }
        /// <summary>
        /// Конструктор выхода данных
        /// </summary>
        public DataOutput1(IList<T> source)
        {
            Source = source;
        }
    }
}
