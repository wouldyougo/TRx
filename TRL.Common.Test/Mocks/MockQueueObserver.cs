using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Collections;
using TRL.Common.Events;

namespace TRL.Common.Test.Mocks
{
    public class MockQueueObserver : IObserver
    {

        private string data = "No data";
        public string Data
        {
            get
            {
                return this.data;
            }
        }

        private ObservableQueue<string> queue;

        public MockQueueObserver(ObservableQueue<string> queue)
        {
            this.queue = queue;
        }

        public void Update()
        {
            this.data = this.queue.Dequeue();
        }
    }
}
