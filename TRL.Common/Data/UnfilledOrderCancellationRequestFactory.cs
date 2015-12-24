using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Models;
using TRL.Common.Extensions.Data;
//using TRL.Common.Extensions;

namespace TRL.Common.Data
{
    public class UnfilledOrderCancellationRequestFactory:IGenericFactory<OrderCancellationRequest>
    {
        private double currentPrice;
        private Order order;
        private IDataContext tradingData;

        //public double currentPrice { get; set; }
        //public Order order { get; set; }
        //public IDataContext tradingData { get; set; }

        public UnfilledOrderCancellationRequestFactory(double currentPrice, Order order, IDataContext tradingData)
        {
            this.currentPrice = currentPrice;
            this.order = order;
            this.tradingData = tradingData;
        }

        public OrderCancellationRequest Make()
        {
            if (order.IsFilled)
                return null;

            if (order.OrderType != OrderType.Limit)
                return null;

            if (order.Signal == null)
                return null;

            if (order.Signal.Strategy == null)
                return null;

            Symbol symbol = this.tradingData.GetSymbol(this.order.Symbol);

            if (symbol == null)
                return null;

            StopPointsSettings slSettings = this.tradingData.GetStopPointsSettings(order.Signal.Strategy);

            if (slSettings == null)
                return null;

            ProfitPointsSettings tpSettings = this.tradingData.GetProfitPointsSettings(order.Signal.Strategy);

            if (tpSettings == null)
                return null;

            if (order.TradeAction == TradeAction.Buy)
            {
                double stopLossPrice = this.order.Signal.Limit - slSettings.Points;

                if (stopLossPrice + symbol.Step >= currentPrice)
                {
                    string descr = String.Format("Текущая цена {0} на расстоянии одного шага от stop loss цены {1} стратегии.", currentPrice, stopLossPrice );
                    return new OrderCancellationRequest(order, descr);
                }

                double takeProfitPrice = this.order.Signal.Limit + tpSettings.Points;

                if(takeProfitPrice - symbol.Step <= currentPrice)
                {
                    string descr = String.Format("Текущая цена {0} на расстоянии одного шага от take profit цены {1} стратегии.", currentPrice, takeProfitPrice);
                    return new OrderCancellationRequest(order, descr);
                }
            }

            if (order.TradeAction == TradeAction.Sell)
            {
                double stopLossPrice = this.order.Signal.Limit + slSettings.Points;

                if (stopLossPrice - symbol.Step <= currentPrice)
                {
                    string descr = String.Format("Текущая цена {0} на расстоянии одного шага от stop loss цены {1} стратегии.", currentPrice, stopLossPrice);
                    return new OrderCancellationRequest(order, descr);
                }

                double takeProfitPrice = this.order.Signal.Limit - tpSettings.Points;

                if (takeProfitPrice + symbol.Step >= currentPrice)
                {
                    string descr = String.Format("Текущая цена {0} на расстоянии одного шага от take profit цены {1} стратегии.", currentPrice, takeProfitPrice);
                    return new OrderCancellationRequest(order, descr);
                }
            }

            return null;
        }
    }
}
