using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRx.Helpers
{
    /// <summary>
    /// Value, Id, DateTime, Name, Symbol
    /// </summary>
    public class ValueDouble : IValueBase<double>
    {
        public string Name
        {
            get;
            set;
        }

        public string Symbol
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }
        public DateTime DateTime
        {
            get;
            set;
        }

        public double Value
        {
            get;
            set;
        }
    }
    /// <summary>
    /// Value, Id, DateTime, Name, Symbol
    /// </summary>
    public class ValueInt : IValueBase<int>
    {
        public string Name
        {
            get;
            set;
        }

        public string Symbol
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }
        public DateTime DateTime
        {
            get;
            set;
        }

        public int Value
        {
            get;
            set;
        }
    }
    /// <summary>
    /// Value, Id, DateTime, Name, Symbol
    /// </summary>
    public class ValueBool : IValueBase<bool>
    {
        public string Name
        {
            get;
            set;
        }
        public string Symbol
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }
        public DateTime DateTime
        {
            get;
            set;
        }

        public bool Value
        {
            get;
            set;
        }
    }
}
