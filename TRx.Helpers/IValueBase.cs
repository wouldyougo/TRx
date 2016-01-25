using System;
using TRL.Common.Models;

namespace TRx.Helpers
{

    public interface IIdLong
    {
        long Id
        {
            get;
            //set;
        }
    }

    /// <summary>
    /// Base Data Class: Type, Timestamp, Key -- Base Features.
    /// </summary>
    public interface IValueBase<T> : IDateTime, IIdLong, ISymbol, INamed //: ICloneable
    {
        /// <summary>
        ///
        /// </summary>
        T Value
        {
            get;
            set;
        }
    }
}
