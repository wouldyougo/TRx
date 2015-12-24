using System;
using System.Collections.Generic;
using System.Text;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Collections.Generic;
//using Newtonsoft.Json;

namespace TRL.Common.Enums
{
    /// <summary>
    /// Market data style: is the market data a summary (OHLC style) bar, or is it a time-price value.
    /// </summary>
    public enum DataModelType
    {
        /// Base market data type
        Base,
        /// Tick market data type (price-time pair)
        Tick,
        /// TradeBar market data type (OHLC summary bar)
        TimeBar,
        /// RangeBar market data type (OHLC summary bar)
        RangeBar,
        /// VolumeBar market data type (OHLC summary bar)
        VolumeBar,
        ///Indicator data type
        Indicator,
        /// Data associated with an instrument
        Auxiliary
    }
}