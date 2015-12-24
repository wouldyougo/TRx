using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Models
{
    public class StrategyPosition:IIdentified
    {
        public int Id { get; set; }
        
        public StrategyHeader Strategy { get; set; }
        
        public int StrategyId { get; set; }
        
        private List<Trade> trades;

        public IEnumerable<Trade> Trades
        {
            get
            {
                return this.trades;
            }
        }

        private StrategyPosition() 
        {
            this.trades = new List<Trade>();
        }

        public StrategyPosition(StrategyHeader strategyHeader)
            :this()
        {
            this.Id = strategyHeader.Id;
            this.StrategyId = strategyHeader.Id;
            this.Strategy = strategyHeader;
            this.amount = 0;
            this.sum = 0;
        }

        private double amount;
        public double Amount
        {
            get
            {
                return this.amount;
            }
        }

        private double sum;
        public double Sum
        {
            get
            {
                return this.sum;
            }
        }
    }
}
