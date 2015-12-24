using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.TimeHelpers
{
    public interface ITimeTrackable
    {
        DateTime StartAt { get; }
        TimeSpan Duration { get; }
    }
}
