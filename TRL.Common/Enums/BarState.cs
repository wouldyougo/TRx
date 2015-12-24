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
    /// Состояния свечи
    /// </summary>
    public enum BarState
    {
        /// <summary>
        /// Пустое состояние (свеча отсутствует).
        /// </summary>
        //[EnumMember]
        None,

        /// <summary>
        /// Свеча запущена на формирование.
        /// </summary>
        //[EnumMember]
        Started,

        /// <summary>
        /// Свеча изменена.
        /// </summary>
        //[EnumMember]
        Changed,        

        /// <summary>
        /// Свеча закончена.
        /// </summary>
        //[EnumMember]
        Finished
    }
}