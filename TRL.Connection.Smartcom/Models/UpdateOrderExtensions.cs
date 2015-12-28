using SmartCOM3Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Connect.Smartcom.Models
{
    public static class UpdateOrderExtensions
    {
        public static bool CanContainFillingMark(this UpdateOrder item)
        {
            if (item.State == StOrder_State.StOrder_State_Pending)
                return false;

            if (item.State == StOrder_State.StOrder_State_Open)
                return false;

            if (item.State == StOrder_State.StOrder_State_ContragentReject)
                return false;

            if (item.State == StOrder_State.StOrder_State_SystemReject)
                return false;

            if (item.State == StOrder_State.StOrder_State_Submited)
                return false;

            return true;
        }
    }
}
