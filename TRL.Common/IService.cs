using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common
{
    public interface IService
    {
        void Start();
        void Stop();
        void Restart();
        bool IsRunning { get; }
    }
}
