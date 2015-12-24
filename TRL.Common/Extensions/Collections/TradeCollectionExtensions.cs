using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;

namespace TRL.Common.Extensions.Collections
{
    public static class TradeCollectionExtensions
    {
        /// <summary>
        /// Получить частичную копию исходной коллекции сделок с указанным суммарным объемом (Amount).
        /// </summary>
        /// <param name="srcCollection">Ссылка на исходную коллекцию сделок.</param>
        /// <param name="amount">Суммарный объем всех сделок в частичной копии исходной коллекции.</param>
        /// <returns>Метод возвращает копию исходной коллекции, в которой для уменьшения объема 
        /// могут быть отброшены самые новые сделки, объем которых превышает запрашиваемый.</returns>
        public static IEnumerable<Trade> TakeForAmount(this IEnumerable<Trade> srcCollection, double amount)
        {
            List<Trade> dstCollection = new List<Trade>();
            
            double dstCollectionAmountSum = 0;

            foreach (Trade currentTrade in srcCollection)
            {
                if (dstCollectionAmountSum == amount)
                    return dstCollection;

                if (dstCollectionAmountSum + Math.Abs(currentTrade.Amount) <= amount)
                {
                    dstCollection.Add(currentTrade);
                    dstCollectionAmountSum += Math.Abs(currentTrade.Amount);
                }
                else
                {
                    double wantage = amount - dstCollectionAmountSum;
                    dstCollection.Add(CloneTradeWithModifiedAmount(currentTrade, wantage));
                    dstCollectionAmountSum += wantage;
                }
            }

            return dstCollection;
        }

        private static Trade CloneTradeWithModifiedAmount(Trade trade, double amount)
        {
            double amountSign = (trade.Amount / Math.Abs(trade.Amount));

            return new Trade(trade.Order, 
                trade.Portfolio,
                trade.Symbol,
                trade.Price, 
                amount * amountSign,
                trade.DateTime);
        }

        public static void Combine(this ICollection<Trade> collection, Trade trade)
        {
            List<Trade> zeroAmountTrades = new List<Trade>();

            foreach (Trade t in collection)
            {
                if (t.Action == trade.Action)
                    continue;

                if (trade.Amount == 0)
                    break;

                if (t.AbsoluteAmount > trade.AbsoluteAmount)
                {
                    t.Amount = t.Amount + trade.Amount;
                    trade.Amount = 0;
                }

                if (t.AbsoluteAmount <= trade.AbsoluteAmount)
                {
                    trade.Amount = t.Amount + trade.Amount;
                    t.Amount = 0;
                }

                if(t.Amount == 0)
                    zeroAmountTrades.Add(t);
            }

            foreach (Trade t in zeroAmountTrades)
                collection.Remove(t);

        }
    }
}
