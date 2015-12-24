using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Collections;
using System.Reflection;
using TRL.Common.Models;
//using TRL.Configuration;

namespace TRL.Common.Data
{
    /*
    Контекст торговых данных реализует интерфейс:

    public interface DataContext
    {
        T Get<T>();
    }

    Поэтому у вас есть по-меньшей мере три способа обращения к коллекциям и наборам, содержащимся в контексте торговых данных:

    /// Получите ссылку на контекст торговых данных
    DataContext tradingDataContext = TradingData.Instance;

    /// Если вам нужно только читать данные из коллекции, используйте такой вызов
    IEnumerable<Tick> ticks = tradingDataContext.Get<IEnumerable<Tick>>();

    /// Если вы хотите изменять содержимое коллекции, добавляя или удаляя ее элементы
    /// но чтобы при этом не срабатывали алгоритмы, наблюдающие изменение коллекции
    /// используйте следующий вызов
    ICollection<Bar> bars = tradingDataContext.Get<ICollection<Bar>>();

    /// Если вы хотите изменять содержимое коллекции, заставляя при этом срабатывать
    /// алгоритмы, наблюдающие за ее содержимым, используйте такой вызов
    ObservableCollection<Trade> trades = tradingDataContext.Get<ObservableCollection<Trade>>();
     * */

    /// <summary>
    /// Торговых данных контекст
    /// </summary>
    public class TradingDataContext:RawBaseDataContext, IObservableHashSetFactory
    {
        public ObservableHashSet<StrategyHeader> Strategies { get; private set; }
        public ObservableHashSet<Signal> Signals { get; private set; }
        public ObservableHashSet<Order> Orders { get; private set; }
        public ObservableHashSet<Trade> Trades { get; private set; }
        public ObservableHashSet<OrderCancellationRequest> CancellationRequests { get; private set; }
        public ObservableHashSet<OrderCancellationConfirmation> CancellationConfirmations { get; private set; }
        public ObservableHashSet<OrderCancellationFailedNotification> CancellationFaults { get; private set; }
        public ObservableHashSet<OrderRejection> OrderRejections { get; private set; }
        public ObservableHashSet<Position> Positions { get; private set; }
        public ObservableHashSet<OrderSettings> OrderSettings { get; private set; }
        public ObservableHashSet<StopLossOrderSettings> StopLossOrderSettings { get; private set; }
        public ObservableHashSet<TakeProfitOrderSettings> TakeProfitOrderSettings { get; private set; }
        public ObservableHashSet<BarSettings> BarSettings { get; private set; }
        public ObservableHashSet<StopPointsSettings> StopLossSettings { get; private set; }
        public ObservableHashSet<ProfitPointsSettings> TakeProfitSettings { get; private set; }
        public ObservableHashSet<OrderDeliveryConfirmation> OrderDeliveryConfirmations { get; private set; }
        public ObservableHashSet<SMASettings> SMASettings { get; private set; }
        public ObservableHashSet<OpenOrder> OpenOrders { get; private set; }
        public ObservableHashSet<CloseOrder> CloseOrders { get; private set; }
        public ObservableHashSet<PositionSettings> PositionsSettings { get; private set; }
        public ObservableHashSet<Trend> Trends { get; private set; }
        public HashSetOfNamedMutable<Symbol> Symbols { get; private set; }
        public ObservableCollection<Tick> Ticks { get; private set; }
        public ObservableCollection<Bar> Bars { get; private set; }
        public ObservableCollection<BidAsk> BidAsks { get; private set; }
        public ObservableCollection<OrderMoveRequest> OrderMoveRequests { get; private set; }
        public ObservableHashSet<StrategyVolumeChangeStep> StrategyVolumeChangeSteps { get; private set; }
        public ObservableCollection<SpreadValue> SpreadValues { get; set; }
        public ObservableHashSet<ArbitrageSettings> ArbitrageSettings { get; set; }
        public ObservableHashSet<MoveOrder> MoveOrders { get; set; }

        public TradingDataContext()
        {
            //this.Strategies = new ObservableHashSet<Strategy>(new IdentifiedComparer());
            //this.Signals = new ObservableHashSet<Signal>(new IdentifiedComparer());
            //this.Orders = new ObservableHashSet<Order>(new IdentifiedComparer());
            //this.Trades = new ObservableHashSet<Trade>(new IdentifiedComparer());
            //this.CancellationRequests = new ObservableHashSet<OrderCancellationRequest>(new IdentifiedComparer());
            //this.CancellationConfirmations = new ObservableHashSet<OrderCancellationConfirmation>(new IdentifiedComparer());
            //this.CancellationFaults = new ObservableHashSet<OrderCancellationFailedNotification>(new IdentifiedComparer());
            //this.OrderRejections = new ObservableHashSet<OrderRejection>(new IdentifiedComparer());
            //this.Positions = new ObservableHashSet<Position>(new PositionEqualityComparer());
            //this.OrderSettings = new ObservableHashSet<OrderSettings>(new IdentifiedComparer());
            //this.StopLossOrderSettings = new ObservableHashSet<StopLossOrderSettings>(new IdentifiedComparer());
            //this.TakeProfitOrderSettings = new ObservableHashSet<TakeProfitOrderSettings>(new IdentifiedComparer());
            //this.BarSettings = new ObservableHashSet<BarSettings>(new IdentifiedComparer());
            //this.StopLossSettings = new ObservableHashSet<StopPointsSettings>(new IdentifiedComparer());
            //this.TakeProfitSettings = new ObservableHashSet<ProfitPointsSettings>(new IdentifiedComparer());
            //this.OrderDeliveryConfirmations = new ObservableHashSet<OrderDeliveryConfirmation>(new OrderDeliveryConfirmationComparer());
            //this.SMASettings = new ObservableHashSet<SMASettings>(new IdentifiedComparer());
            //this.OpenOrders = new ObservableHashSet<OpenOrder>(new IdentifiedComparer());
            //this.CloseOrders = new ObservableHashSet<CloseOrder>(new IdentifiedComparer());
            //this.PositionsSettings = new ObservableHashSet<PositionSettings>(new IdentifiedComparer());
            //this.Trends = new ObservableHashSet<Trend>(new TrendEqualityComparer());
            //this.Symbols = new HashSetOfNamedMutable<Symbol>();
            //this.Ticks = new ObservableCollection<Tick>();
            //this.Bars = new ObservableCollection<Bar>();
            //this.BidAsks = new ObservableCollection<BidAsk>();
            //this.OrderMoveRequests = new ObservableCollection<OrderMoveRequest>();
            //this.StrategyVolumeChangeSteps = new ObservableHashSet<StrategyVolumeChangeStep>(new IdentifiedComparer());
            //this.SpreadValues = new ObservableCollection<SpreadValue>();
            //this.ArbitrageSettings = new ObservableHashSet<ArbitrageSettings>(new IdentifiedComparer());
            //this.MoveOrders = new ObservableHashSet<MoveOrder>();

            this.Strategies = new ObservableHashSet<StrategyHeader>(new IdentifiedComparer());
            this.Signals = new ObservableHashSet<Signal>(new IdentifiedComparer());
            this.Orders = new ObservableHashSet<Order>(new IdentifiedComparer());
            this.Trades = new ObservableHashSet<Trade>(new IdentifiedComparer());

            this.Symbols = new HashSetOfNamedMutable<Symbol>();
            this.Ticks = new ObservableCollection<Tick>();
            this.Bars = new ObservableCollection<Bar>();
            this.BidAsks = new ObservableCollection<BidAsk>();

            this.Positions = new ObservableHashSet<Position>(new PositionEqualityComparer());
            this.PositionsSettings = new ObservableHashSet<PositionSettings>(new IdentifiedComparer());

            this.CancellationRequests = new ObservableHashSet<OrderCancellationRequest>(new IdentifiedComparer());
            this.CancellationConfirmations = new ObservableHashSet<OrderCancellationConfirmation>(new IdentifiedComparer());
            this.CancellationFaults = new ObservableHashSet<OrderCancellationFailedNotification>(new IdentifiedComparer());
            this.OrderRejections = new ObservableHashSet<OrderRejection>(new IdentifiedComparer());
            this.MoveOrders = new ObservableHashSet<MoveOrder>();
            this.OrderMoveRequests = new ObservableCollection<OrderMoveRequest>();

            this.OrderDeliveryConfirmations = new ObservableHashSet<OrderDeliveryConfirmation>(new OrderDeliveryConfirmationComparer());
            this.OpenOrders = new ObservableHashSet<OpenOrder>(new IdentifiedComparer());
            this.CloseOrders = new ObservableHashSet<CloseOrder>(new IdentifiedComparer());

            this.OrderSettings = new ObservableHashSet<OrderSettings>(new IdentifiedComparer());
            this.StopLossOrderSettings = new ObservableHashSet<StopLossOrderSettings>(new IdentifiedComparer());
            this.TakeProfitOrderSettings = new ObservableHashSet<TakeProfitOrderSettings>(new IdentifiedComparer());

            this.BarSettings = new ObservableHashSet<BarSettings>(new IdentifiedComparer());
            this.StopLossSettings = new ObservableHashSet<StopPointsSettings>(new IdentifiedComparer());
            this.TakeProfitSettings = new ObservableHashSet<ProfitPointsSettings>(new IdentifiedComparer());

            this.StrategyVolumeChangeSteps = new ObservableHashSet<StrategyVolumeChangeStep>(new IdentifiedComparer());
            this.SpreadValues = new ObservableCollection<SpreadValue>();
            this.ArbitrageSettings = new ObservableHashSet<ArbitrageSettings>(new IdentifiedComparer());
            this.Trends = new ObservableHashSet<Trend>(new TrendEqualityComparer());
            this.SMASettings = new ObservableHashSet<SMASettings>(new IdentifiedComparer());

            // добавить
        }

        public ObservableHashSet<T> Make<T>()
        {
            PropertyInfo[] properties = this.GetType().GetProperties();

            foreach (PropertyInfo item in properties)
            {
                if (item.PropertyType.FullName.Equals(typeof(ObservableHashSet<T>).FullName))
                    return (ObservableHashSet<T>)item.GetValue(this, null);
            }

            return null;
        }
    }
}
