using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using TRL.Common.Models;
using System.IO;
using TRL.Common.Collections;

namespace TRL.Transaction
{
    public class ImportTicksTransaction : ImportItemsTransaction<Tick>
    {
        private string symbol;
        public ImportTicksTransaction(ObservableCollection<Tick> dst, string path, string symbol = "")
            :base(dst, path)
        {
            this.symbol = symbol;
        }

        public override Tick TryParseItem(string src)
        {
            //try
            {
                Tick item = Tick.Parse(src);

                if (!string.IsNullOrEmpty(this.symbol))
                    item.Symbol = this.symbol;

                return item;
            }
            //catch(Exception e)
            {
                //throw e;
                //return null;
            }
        }
    }

}
