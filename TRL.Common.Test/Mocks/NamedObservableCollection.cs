using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Collections;
using TRL.Common.Models;

namespace TRL.Common.Test.Mocks
{
    public class NamedObservableCollection<T> : ObservableCollection<T>, INamed
    {
        private string name;
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public NamedObservableCollection(string name)
        {
            this.name = name;
        }
    }
}
