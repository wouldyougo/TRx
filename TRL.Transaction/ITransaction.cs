using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Transaction
{
    public interface ITransaction
    {
        void Execute();
    }
}
