using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Events;

namespace TRL.Common.Test.Mocks
{
    public delegate void MockEvent();

    public class MockEventBinder : IGenericBinder
    {
        public int BindedEventsCounter { get; set; }

        private MockEvent mockEvent;

        public void Bind<T>(T method)
        {
            if (method is MockEvent)
            {
                this.mockEvent += method as MockEvent;
                this.BindedEventsCounter++;
            }
        }

        public void Unbind<T>(T method)
        {
            if (this.BindedEventsCounter == 0)
                return;

            if (method is MockEvent)
            {
                this.mockEvent -= method as MockEvent;
                this.BindedEventsCounter--;
            }
        }
    }
}
