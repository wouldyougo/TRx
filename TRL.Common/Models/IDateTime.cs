using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public interface IDateTime
    {
        //DateTime DateTime { get; }
        /// <summary>
        /// Time keeper of data -- all data is timeseries based.
        /// </summary>
        DateTime DateTime
        {
            get;
            //set;
        }
    }
}
