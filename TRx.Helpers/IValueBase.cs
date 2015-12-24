using System;
using TRL.Common.Models;

namespace TRx.Helpers
{
    /// <summary>
    /// Base Data Class: Type, Timestamp, Key -- Base Features.
    /// </summary>
    public interface IValueBase<T> : IDateTime, IIdentified, ISymbol, INamed //: ICloneable
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
