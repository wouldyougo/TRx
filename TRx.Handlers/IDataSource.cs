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
    public interface IDataInput<T>
    {
        IList<T> Value { get; }
        //содержит ссылку на экземпляр IDataSource
        IDataSource<T> DataSource { get; }
        //содержит номер исочника данных в IDataSource
        int Number  { get; }
    }
    public class DataInput<T>: IDataInput<T>
    {
        public IList<T> Value
        {
            get{ return DataSource.Source(Number); }
        }
        //содержит ссылку на экземпляр IDataSource
        public IDataSource<T> DataSource { get; private set; }
        //содержит номер исочника данных в IDataSource
        public int Number { get; private set; }
        public DataInput(IDataSource<T> dataSource, int i = 0)
        {
            this.DataSource = dataSource;
            this.Number = i;
        }
    }
    public interface IDataSource<T>
    {
        IList<T> Source(int i = 0);
    }
    public class DataSource<T> : IDataSource<T>
    {
        protected IList<T> _Source { get; set; }

        IList<T> IDataSource<T>.Source(int i = 0)
        {
            if (i == 0)
            {
                return this._Source;
            }
            else {
                return null;
            }
        }
    }
}
