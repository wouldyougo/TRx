using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;

namespace TRL.Common.Extensions.Data
{
    public static class OrderBookContextExtensions
    {
        public static double GetOfferVolumeSum(this OrderBookContext orderBook, string symbol, int depth = 50)
        {
            double sum = 0;

            if (depth > orderBook.Depth)
                depth = orderBook.Depth;

            for (int i = 0; i < depth; i++)
                sum += orderBook.GetOfferVolume(symbol, i);

            return sum;
        }

        public static double GetBidVolumeSum(this OrderBookContext orderBook, string symbol, int depth = 50)
        {
            double sum = 0;

            if (depth > orderBook.Depth)
                depth = orderBook.Depth;

            for (int i = 0; i < depth; i++)
                sum += orderBook.GetBidVolume(symbol, i);

            return sum;
        }

        public static double GetBestOfferPrice(this OrderBookContext context, string symbol, double priceStep)
        {
            double offer = context.GetOfferPrice(symbol, 0);
            double bid = context.GetBidPrice(symbol, 0);

            if (offer - bid > priceStep)
                return offer - priceStep;

            return offer;
        }

        public static double GetBestBidPrice(this OrderBookContext context, string symbol, double priceStep)
        {
            double offer = context.GetOfferPrice(symbol, 0);
            double bid = context.GetBidPrice(symbol, 0);

            if (offer - bid > priceStep)
                return bid + priceStep;

            return bid;
        }
    }
}
