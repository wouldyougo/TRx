﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Data
{
    public interface IGenericFactory<T>
    {
        T Make();
    }
}
