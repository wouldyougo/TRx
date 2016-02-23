using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common;
using TRL.Common.Collections;
using TRL.Common.Data;
using TRL.Common.Handlers;
using TRL.Common.Extensions;
using TRL.Common.Models;
using TRL.Common.Extensions.Models;
using TRL.Common.Extensions.Collections;
using TRL.Logging;

namespace TRL.Common.Handlers
{
    public class PlaceCancelOrderRequestOnTick:AddedItemHandler<Tick>
    {
        private IDataContext tradingData;
        private ILogger logger;

        //private UnfilledOrderCancellationRequestFactory cancellationFactory;

        public PlaceCancelOrderRequestOnTick(IDataContext tradingData, ILogger logger)
            :base(tradingData.Get<ObservableCollection<Tick>>())
        {
            this.tradingData = tradingData;
            this.logger = logger;

            //this.cancellationFactory = new UnfilledOrderCancellationRequestFactory(0, new Order(), tradingData);
        }

        public override void OnItemAdded(Tick item)
        {
            //TODO здесь что-то очень медленно:
            IEnumerable<Order> unfilled = this.tradingData.Get<ICollection<Order>>().GetUnfilled(item.Symbol);

            if (unfilled == null)
                return;

            if (unfilled.Count() == 0)
                return;
            try
            {
                foreach (Order o in unfilled)
                {
                    if (!this.tradingData.Get<ICollection<Position>>().Exists(o.Portfolio, o.Symbol))
                    {
                        IGenericFactory<OrderCancellationRequest> cancellationFactory = new UnfilledOrderCancellationRequestFactory(item.Price, o, (TradingDataContext)this.tradingData);
                        //cancellationFactory.currentPrice = item.Price;
                        //cancellationFactory.order = o;
                        //cancellationFactory.tradingData = (TradingDataContext)this.tradingData;

                        OrderCancellationRequest request = cancellationFactory.Make();

                        if (request != null)
                        {
                            this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сформирован новый запрос {2} на отмену заявки {3}.", DateTime.Now, this.GetType().Name, o.ToString(), request.Description));
                            this.tradingData.Get<ObservableHashSet<OrderCancellationRequest>>().Add(request);
                        }
                    }
                }
            }
            catch 
            {
                this.logger.Log(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, Ошибка в {2}.", DateTime.Now, this.GetType().Name, "OnItemAdded"));
            }
        }
    }
}
