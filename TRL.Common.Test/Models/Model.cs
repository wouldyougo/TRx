using TRL.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Test.Models
{
    public class Model : INamed, IMutable<Model>
    {
        public string Name { get; set; }
        public double Value { get; set; }

        public Model() { }

        public Model(string name, double value)
        {
            this.Name = name;
            this.Value = value;
        }

        public void Update(Model item)
        {
            if (item.Name == this.Name)
                this.Value = item.Value;
        }
    }
}
