using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Data
{
    public class SymbolsSummary:SymbolDataContext
    {
        private static SymbolsSummary symbolSummary = null;

        public static SymbolsSummary Instance
        {
            get
            {
                if (symbolSummary == null)
                    symbolSummary = new SymbolsSummary();

                return symbolSummary;
            }
        }

        private SymbolsSummary() { }
        
    }
}
