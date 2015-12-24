using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    public class TradingData:TradingDataContext
    {
        private static TradingData instance = null;

        public static TradingData Instance
        {
            get
            {
                if (instance == null)
                    instance = new TradingData();

                return instance;
            }
        }

        private TradingData() { }
    }
}
