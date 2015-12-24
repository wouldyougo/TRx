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
    public class ImportBarsTransaction : ImportItemsTransaction<Bar>
    {
        public ImportBarsTransaction(ObservableCollection<Bar> dst, string path)
            :base(dst, path){}

        public override Bar TryParseItem(string src)
        {
            try
            {
                return Bar.Parse(src);
            }
            catch
            {
                return null;
            }
        }
    }

}
