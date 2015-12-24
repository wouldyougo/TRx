using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Test.Mocks
{
    public class MockTraderService:IService
    {
        private bool isRunning;

        public void Start()
        {
            this.isRunning = true;
        }

        public void Stop()
        {
            this.isRunning = false;
        }

        public void Restart()
        {
            if (this.isRunning)
                this.Stop();

            this.Start();
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
        }
    }
}
