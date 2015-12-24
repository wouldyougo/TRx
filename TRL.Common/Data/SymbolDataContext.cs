using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;
using TRL.Common.Collections;

namespace TRL.Common.Data
{
    public class SymbolDataContext:RawBaseDataContext
    {
        public HashSetOfNamedMutable<SymbolSettings> SymbolSettings { get; private set; }
        public HashSetOfNamedMutable<SymbolSummary> SymbolSummary { get; private set; }

        public SymbolDataContext()
        {
            this.SymbolSettings = new HashSetOfNamedMutable<SymbolSettings>();
            this.SymbolSummary = new HashSetOfNamedMutable<SymbolSummary>();
        }
    }
}
