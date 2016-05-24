using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using TRL.Common.Enums;

namespace TRL.Common.Models
{
    //void AddBar(
    //    int row, 
    //    int nrows, 
    //1    string symbol, 
    //2    StClientLib.StBarInterval interval, 
    //3    System.DateTime datetime, 
    //4    double open, 
    //5    double high, 
    //6    double low, 
    //7    double close, 
    //8    double volume, 
    //9    double open_int)

    // Row int Номер бара в списке 
    // NRows int Всего баров в списке 
    //1 Symbol String Код ЦБ из таблицы котировок SmartTrade 
    //2 Interval StBarInterval Интервал времени.
    //                StBarInterval_1Min = 1,  
    //                StBarInterval_5Min = 2,  
    //                StBarInterval_10Min = 3,  
    //                StBarInterval_15Min = 4,  
    //                StBarInterval_30Min = 5,  
    //                StBarInterval_60Min = 6,  
    //                StBarInterval_2Hour = 7,  
    //                StBarInterval_4Hour = 8,  
    //                StBarInterval_Day = 9,  
    //                StBarInterval_Week = 10,  
    //                StBarInterval_Month = 11,  
    //                StBarInterval_Quarter = 12,  
    //                StBarInterval_Year = 13  
    //3 Datetime DateTime Дата и время интервала  
    //4 Open Double Цена первой сделки после открытия в интервале 
    //5 High Double Максимальная цена сделки в интервале  
    //6 Low Double Минимальная цена сделки в интервале 
    //7 Close Double Цена последней сделки в интервале 
    //8 Volume Double Объём сделок в интервале 
    //9 Open_Int Double Открытый интерес

    public class Bar
    {
        ///TODO 0. Добавить UID
        ///TODO 2. Добавить DateTime Open
        ///TODO 2. Добавить TimeSpan = DateTimeClose - DateTimeOpen 
        /// практически готовый индикатор скорости Range bara

        public string Symbol { get; set; }
        /// <summary>
        /// Интервал
        /// в секундах - для времеонного интервала
        /// в единицах - для volume или range бара
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// long DateTime.Ticks
        /// </summary>
        public long DateID { get { return DateTime.Ticks; } }

        /// <summary>
        /// Open DateTime
        /// </summary>
        public DateTime DateTimeOpen { get; set; }

        /// <summary>
        /// Close DateTime
        /// </summary>
        public DateTime DateTime { get; set; }

        public double Open { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double Close { get; set; }

        public double Volume { get; set; }

        public double OpenInterest { get; set; }

        public Bar() { }

        public Bar(DateTime dateTime, double open, double high, double low, double close, double volume)
            :this("", 0, dateTime, open, high, low, close, volume){}

        public Bar(string symbol, int interval, DateTime dateTime)
            : this(symbol, interval, dateTime, 0, 0, 0, 0, 0) { }

        public Bar(string symbol, int interval, DateTime dateTime, double open, double high, double low, double close, double volume)
        {
            this.Symbol = symbol;
            this.Interval = interval;
            this.DateTime = dateTime;
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;
            this.Volume = volume;

            this.DateTimeOpen = dateTime.AddSeconds(-interval);
        }
        public Bar(string symbol, DateTime dateTimeOpen, DateTime dateTimeClose)
            : this(symbol, dateTimeOpen, dateTimeClose, 0, 0, 0, 0, 0) {}
        public Bar(string symbol, DateTime dateTimeOpen, DateTime dateTimeClose, double open, double high, double low, double close, double volume)
        {
            this.Symbol = symbol;
            this.DateTimeOpen = dateTimeOpen;
            this.DateTime = dateTimeClose;
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;
            this.Volume = volume;

            this.Interval = (int)(dateTimeClose - DateTimeOpen).TotalSeconds;
        }

        /// <summary>
        /// В общем случае для других типов Bar может быть 
        /// interval <> dateTimeClose - dateTimeOpen
        /// </summary>
        public Bar(string symbol, int interval, DateTime dateTimeOpen, DateTime dateTimeClose, double open, double high, double low, double close, double volume)
        {
            this.Symbol = symbol;
            this.Interval = interval;
            this.DateTimeOpen = dateTimeOpen;
            this.DateTime = dateTimeClose;
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;
            this.Volume = volume;
        }
        /// <summary>
        /// Параметр свечи
        /// </summary>
        public virtual object Arg
        {
            get { return Interval; }
            //set { Interval = (int)(value); }
            set { Interval = Convert.ToInt32(value); }
            
        }
        /// <summary>
        /// Суммарный оборот по сделкам.
        /// Price*Volume
        /// </summary>
        public double VolumePrice { get; set; }

        /// <summary>
        /// Средняя цена по сделкам.
        /// TotalPrice/Volume
        /// </summary>
        public double AveragePrice
        {
            get
            {
                return VolumePrice / Volume;
            }
        }

        public bool IsWhite
        {
            get
            {
                return this.Close > this.Open;
            }
        }

        public bool IsBlack
        {
            get
            {
                return this.Close < this.Open;
            }
        }

        /// <summary>
        /// Состояние.
        /// </summary>
        public BarState State { get; set; }

        #region Text
        public static Bar Parse(string barString)
        {
            string[] parts = barString.Split(',');

            CultureInfo provider = CultureInfo.InvariantCulture;

            string pattern = "yyyyMMdd HHmmss";

            string dateTime = string.Empty;

            if (parts.Count() == 10)
            {   // Собственный формат
                //<TICKER>,<PER>,<DATETIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOL>,<OI>,<DATEID>
                pattern = "yyyyMMdd HHmmss fffffff";
                return new Bar
                {
                    Symbol = parts[0],
                    Interval = Convert.ToInt16(parts[1], provider) * 60,
                    DateTime = DateTime.ParseExact(parts[2], pattern, provider),
                    Open = Convert.ToDouble(parts[3], provider),
                    High = Convert.ToDouble(parts[4], provider),
                    Low = Convert.ToDouble(parts[5], provider),
                    Close = Convert.ToDouble(parts[6], provider),
                    Volume = Convert.ToDouble(parts[7], provider),
                    OpenInterest = Convert.ToDouble(parts[8], provider)

                    //Open = Convert.Todouble(parts[4], provider),
                    //High = Convert.Todouble(parts[5], provider),
                    //Low = Convert.Todouble(parts[6], provider),
                    //Close = Convert.Todouble(parts[7], provider),
                    //Volume = Convert.Todouble(parts[8], provider)
                };
            }
            else if (parts.Count() == 9)
            {
                //<TICKER>,<PER>,<DATE>,<TIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOL>
                //pattern = "yyyyMMdd HHmmss fffffff";
                dateTime = String.Concat(parts[2], " ", parts[3]);
                return new Bar
                {
                    Symbol = parts[0],
                    Interval = Convert.ToInt16(parts[1], provider) * 60,
                    DateTime = DateTime.ParseExact(dateTime, pattern, provider),
                    Open = Convert.ToDouble(parts[4], provider),
                    High = Convert.ToDouble(parts[5], provider),
                    Low = Convert.ToDouble(parts[6], provider),
                    Close = Convert.ToDouble(parts[7], provider),
                    Volume = Convert.ToDouble(parts[8], provider)

                    //Open = Convert.Todouble(parts[4], provider),
                    //High = Convert.Todouble(parts[5], provider),
                    //Low = Convert.Todouble(parts[6], provider),
                    //Close = Convert.Todouble(parts[7], provider),
                    //Volume = Convert.Todouble(parts[8], provider)
                };
            }
            else if (parts.Count() == 8)
            {   //<TICKER>,<DATE>,<TIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOL>
                dateTime = String.Concat(parts[1], " ", parts[2]);

                return new Bar
                {
                    Symbol = parts[0],
                    DateTime = DateTime.ParseExact(dateTime, pattern, provider),
                    Open = Convert.ToDouble(parts[3], provider),
                    High = Convert.ToDouble(parts[4], provider),
                    Low = Convert.ToDouble(parts[5], provider),
                    Close = Convert.ToDouble(parts[6], provider),
                    Volume = Convert.ToDouble(parts[7], provider)

                    //Open = Convert.Todouble(parts[3], provider),
                    //High = Convert.Todouble(parts[4], provider),
                    //Low = Convert.Todouble(parts[5], provider),
                    //Close = Convert.Todouble(parts[6], provider),
                    //Volume = Convert.Todouble(parts[7], provider)
                };
            }

            dateTime = String.Concat(parts[0], " ", parts[1]);

            return new Bar(DateTime.ParseExact(dateTime, pattern, provider),
                Convert.ToDouble(parts[2], provider),
                Convert.ToDouble(parts[3], provider),
                Convert.ToDouble(parts[4], provider),
                Convert.ToDouble(parts[5], provider),
                Convert.ToDouble(parts[6], provider));

            //Convert.Todouble(parts[2], provider),
            //Convert.Todouble(parts[3], provider),
            //Convert.Todouble(parts[4], provider),
            //Convert.Todouble(parts[5], provider),
            //Convert.Todouble(parts[6], provider));
        }
        public override string ToString()
        {
            //return ToString("Symbol: {0}, Interval: {1}, DateTime: {2}, Open: {3}, High: {4}, Low: {5}, Close: {6}, Volume: {7}");
            return ToString("Symbol: {0}, Interval: {1}, DateTime: {2:yyyyMMdd HHmmss fffffff}, Open: {3}, High: {4}, Low: {5}, Close: {6}, Volume: {7}");
        }

        public string ToImportString()
        {
            //return ToString("{0},{1},{2:yyyyMMdd,HHmmss},{3},{4},{5},{6},{7}");
            return ToString("{0},{1},{2:yyyyMMdd,HHmmssfffffff},{3},{4},{5},{6},{7}");            
        }

        public string ToStringFinam()
        {
            //"<TICKER>,<PER>,<DATE>,<TIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOL>"
            CultureInfo ci = CultureInfo.InvariantCulture;
            //return String.Format("{0},{1},{2:yyyyMMdd,HHmmss},{3},{4},{5},{6},{7}", 
            return String.Format("{0},{1},{2:yyyyMMdd,HHmmssfffffff},{3},{4},{5},{6},{7}",
                this.Symbol, this.Interval, this.DateTime, this.Open.ToString("0.0000", ci), this.High.ToString("0.0000", ci), this.Low.ToString("0.0000", ci), this.Close.ToString("0.0000", ci), this.Volume.ToString("0.0000", ci));
        }
        public static string ToStringHeader()
        {   //10
            //return "<TICKER>,<PER>,<DATE>,<TIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOL>,<DATEID>";
            // Собственный формат
            return "<TICKER>,<PER>,<DATETIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOL>,<OI>,<DATEID>";
        }
        //Millisecond
        //Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff"));
        //Microseconds
        //Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.ffffff"));
        //Tick
        //Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fffffff"));
        //Console.WriteLine(DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fffffff"));
        public string ToStringShort()
        {   // Собственный формат
            //"<TICKER>,<PER>,<DATE>,<TIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOL>,<DATEID>"
            //"<TICKER>,<PER>,<DATETIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOL>,<DATEID>"            
            //"<TICKER>,<PER>,<DATETIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOL>,<OI>,<DATEID>"
            CultureInfo ci = CultureInfo.InvariantCulture;
            return String.Format("{0},{1},{2:yyyyMMdd HHmmss fffffff},{3},{4},{5},{6},{7},{8},{9}",
                this.Symbol, this.Interval, this.DateTime, this.Open.ToString("0.0000", ci), this.High.ToString("0.0000", ci), this.Low.ToString("0.0000", ci), this.Close.ToString("0.0000", ci), this.Volume.ToString("0.0000", ci), this.OpenInterest.ToString("0.0000", ci), this.DateID);
        }

        private string ToString(string format)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;
            //TODO 2. Заменил  this.DateTime.ToString(ci) на this.DateTime, протестировать
            //return String.Format(format, this.Symbol, this.Interval, this.DateTime.ToString(ci), this.Open.ToString("0.0000", ci), this.High.ToString("0.0000", ci), this.Low.ToString("0.0000", ci), this.Close.ToString("0.0000", ci), this.Volume.ToString("0.0000", ci));
            return String.Format(format, this.Symbol, this.Interval, this.DateTime, this.Open.ToString("0.0000", ci), this.High.ToString("0.0000", ci), this.Low.ToString("0.0000", ci), this.Close.ToString("0.0000", ci), this.Volume.ToString("0.0000", ci));
        }

        #endregion
    }

    /// <summary>
    /// Свеча, группируемая по ценовому диапазону
    /// </summary>
    public class RangeBar : Bar
    {
        /// <summary>
        /// Дельта цены, в рамках которой свеча может содержать сделки
        /// </summary>
        public double Range { get; set; }

        /// <summary>
        /// Параметр свечи
        /// </summary>
        public override object Arg
        {
            get { return Range; }
            set { Range = Convert.ToDouble(value); }
        }
        public RangeBar(string symbol, int interval, DateTime dateTime, double open, double high, double low, double close, double volume)
        {
            this.Symbol = symbol;
            this.Interval = interval;
            this.DateTime = dateTime;
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;
            this.Volume = volume;
        }
    }

    /// <summary>
    /// Свеча, группируемая по количеству контрактов.
    /// </summary>
    public class VolumeBar : Bar
    {
        /// <summary>
        /// Дельта цены, в рамках которой свеча может содержать сделки.
        /// </summary>
        public double VolumeLimit { get; set; }

        /// <summary>
        /// Параметр свечи
        /// </summary>
        public override object Arg
        {
            get { return VolumeLimit; }
            set { VolumeLimit = Convert.ToDouble(value); }
        }
        public VolumeBar(string symbol, int interval, DateTime dateTime, double open, double high, double low, double close, double volume)
        {
            this.Symbol = symbol;
            this.Interval = interval;
            this.DateTime = dateTime;
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;
            this.Volume = volume;
        }
    }

}
