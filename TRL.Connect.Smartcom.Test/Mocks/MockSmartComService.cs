using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common;

namespace TRL.Connect.Smartcom.Test.Mocks
{
    public class MockSmartComService:IService
    {
        private bool isRunning = false;

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
            if(this.isRunning)
                this.Stop();

            this.Start();
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
        }
    }
}
