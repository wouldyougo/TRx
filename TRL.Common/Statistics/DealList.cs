using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRL.Common.Models;

namespace TRL.Common.Statistics
{
    public class DealList
    {
        public string Symbol { get; set; }
        public Queue<Trade> BuyTrades { get; set; }
        public Queue<Trade> SellTrades { get; set; }
        public List<Deal> Deals { get; set; }
        public double Count
        {
            get
            {
                return Deals.Count;
            }
        }
        public double CountP
        {
            get
            {
                return Deals.Count(i => i.PnL > 0);
            }
        }
        public double CountL
        {
            get
            {
                return Deals.Count(i => i.PnL <= 0);
            }
        }
        public double Sum
        {
            get
            {
                return Deals.Sum(i => i.PnL);
            }
        }
        public double SumP
        {
            get
            {
                return Deals.Where(i => i.PnL > 0).Sum(i => i.PnL);
            }
        }
        public double SumL
        {
            get
            {
                return Deals.Where(i => i.PnL <= 0).Sum(i => i.PnL);
            }
        }
        public double CountPercentP
        {
            get
            {
                if (this.Count > 0)
                    return 100 * Deals.Count(i => i.PnL > 0)/ this.Count;
                return 0;
            }
        }
        public double CountPercentL
        {
            get
            {
                if (this.Count > 0)
                    return 100 * Deals.Count(i => i.PnL <= 0)/ this.Count;
                return 0;
            }
        }
        public double AverageSum
        {
            get
            {
                if (this.Count > 0)
                    return this.Sum / this.Count;
                return 0;
            }
        }
        public double AverageSumP
        {
            get
            {
                if (this.CountP > 0)
                    return this.SumP/ this.CountP;
                return 0;
            }
        }
        public double AverageSumL
        {
            get
            {
                if (this.CountL > 0)
                    return this.SumL / this.CountL;
                return 0;
            }
        }
        public double AverageSeriesP
        {
            get
            {
                if (this.CountP > 0)
                    return Deals.Where(i => i.PnL > 0).Sum(i => i.InSeries) / this.CountP;

                return 0;
            }
        }
        public double AverageSeriesL
        {
            get
            {
                if (this.CountL > 0)
                    return Deals.Where(i => i.PnL <= 0).Sum(i => i.InSeries) / this.CountL;

                return 0;
            }
        }
        public double MaxSeriesP
        {
            get
            {
                if (this.CountP > 0)
                    return Deals.Where(i => i.PnL > 0).Max(i => i.InSeries);
                return 0;
            }
        }
        public double MaxSeriesL
        {
            get
            {
                if (this.CountL > 0)
                    return Deals.Where(i => i.PnL <= 0).Max(i => i.InSeries);
                return 0;
            }
        }


        public DealList(StrategyHeader strategyHeader) {
            if (strategyHeader == null)
            {
                throw new NotImplementedException();
            }
            this.Symbol = strategyHeader.Symbol;
            Deals = new List<Deal>();
            BuyTrades = new Queue<Trade>();
            SellTrades = new Queue<Trade>();
        }
        public void OnItemAdded(Trade item)
        {
            if (Symbol != item.Symbol)
            {
                return;
            }
            //разобрать трейд по модулю лота, т.е. на максимальнео количество частей
            //определить направление трейда
            //проверить очередь противоположного направления, на наличие соответсвующего открывающего трейда
            //если есть соответсвующий открывающий трейд, извлекаем
            //определяем направление закрытой позиции
            //определяем результат закрытой позиции
            //если нет соответствия, помещаем в очередь своего направления

            //определить направление трейда
            int count = (int)System.Math.Abs(item.Amount);
            if (item.Action == TradeAction.Buy)
            {
                //разобрать трейд по модулю лота, т.е. на максимальнео количество частей
                for (int i = 0; i < count; i++)
                {
                    Trade tradeBuy = new Trade(item);
                    tradeBuy.Amount = 1;
                    //проверить очередь противоположного направления, на наличие соответсвующего открывающего трейда
                    if (SellTrades.Any())
                    {
                        //если есть соответсвующий открывающий трейд, извлекаем
                        Trade tradeSell_1 = SellTrades.Dequeue();
                        //определяем направление закрытой позиции
                        //определяем результат закрытой позиции
                        Deal deal = new Deal(TradeAction.Sell, tradeBuy, tradeSell_1);
                        SetCumulativePnL(deal);
                        Deals.Add(deal);
                    }
                    else
                    {
                        //если нет соответствия, помещаем в очередь своего направления
                        BuyTrades.Enqueue(tradeBuy);
                    }
                }
            }
            else
            if (item.Action == TradeAction.Sell)
            {
                for (int i = 0; i < count; i++)
                {
                    Trade tradeSell = new Trade(item);
                    tradeSell.Amount = -1;
                    if (BuyTrades.Any())
                    {
                        Trade tradeBuy_1 = BuyTrades.Dequeue();
                        Deal deal = new Deal(TradeAction.Buy, tradeBuy_1, tradeSell);
                        SetCumulativePnL(deal);
                        Deals.Add(deal);
                    }
                    else
                    {
                        SellTrades.Enqueue(tradeSell);
                    }
                }
            }
        }

        private void SetCumulativePnL(Deal deal)
        {
            if (Deals.Count > 0)
            {
                deal.CumulativePnL = deal.PnL + Deals.Last().CumulativePnL;
                if (deal.IsProfit == Deals.Last().IsProfit)
                {
                    deal.InSeries = Deals.Last().InSeries + 1;
                    deal.InSeriesMax = deal.InSeries;
                    Deals.Last().InSeriesMax = 0;
                }
                else
                {
                    deal.InSeries = 1;
                    deal.InSeriesMax = 1;
                }
            }
            else
            {
                deal.CumulativePnL = deal.PnL;
                deal.InSeries = 1;
                deal.InSeriesMax = 1;
            }
        }
    }
}
