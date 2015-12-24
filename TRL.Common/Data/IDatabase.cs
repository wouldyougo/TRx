using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Data
{
    public interface IDatabase
    {
        void Add<T>(T item) where T : class;
        void Remove<T>(T item) where T : class;
    }
}
