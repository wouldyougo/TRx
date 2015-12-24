using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;

namespace TRL.Common.Data.Spreads
{
    public class BuySpreadFactory:IGenericFactory<double>
    {
        private IQuoteProvider quoteProvider;
        private IEnumerable<StrategyHeader> leftLeg;
        private IEnumerable<StrategyHeader> rightLeg;

        public BuySpreadFactory(IEnumerable<StrategyHeader> leftLeg, IEnumerable<StrategyHeader> rightLeg, IQuoteProvider quoteProvider)
        {
            this.quoteProvider = quoteProvider;
            this.leftLeg = leftLeg;
            this.rightLeg = rightLeg;
        }

        public double Make()
        {
            if (!OffersExists())
                return 0;

            if (!BidsExists())
                return 0;

            double offerSum = MakeOfferSum();
            double bidSum = MakeBidSum();

            if (offerSum == 0)
                return 0;

            if (bidSum == 0)
                return 0;

            return Math.Round(offerSum / bidSum, 4);
        }

        private bool OffersExists()
        {
            try
            {
                foreach (StrategyHeader s in this.leftLeg)
                {
                    if (this.quoteProvider.GetOfferPrice(s.Symbol, 0) == 0)
                        return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private double MakeOfferSum()
        {
            double result = 0;

            foreach (StrategyHeader s in this.leftLeg)
            {
                result += this.quoteProvider.GetOfferPrice(s.Symbol, 0) * s.Amount;
            }

            return result;
        }

        private double MakeBidSum()
        {
            double result = 0;

            foreach (StrategyHeader s in this.rightLeg)
            {
                result += this.quoteProvider.GetBidPrice(s.Symbol, 0) * s.Amount;
            }

            return result;
        }

        private bool BidsExists()
        {
            try
            {
                foreach (StrategyHeader s in this.rightLeg)
                {
                    if (this.quoteProvider.GetBidPrice(s.Symbol, 0) == 0)
                        return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
