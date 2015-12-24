using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Data
{
    /// <summary>
    /// Фабрика int ID
    /// </summary>
    public static class SerialIntegerFactory
    {
        private static int counter = 0;

        public static int Make()
        {
            int seconds = (int)(DateTime.UtcNow - new DateTime(2000, 1, 1)).TotalSeconds;

            return Math.Abs(seconds - 418385000 + counter++);
        }
    }
}
