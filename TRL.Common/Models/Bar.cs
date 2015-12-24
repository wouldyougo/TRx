using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using TRL.Common.Enums;

namespace TRL.Common.Models
{
    public class Bar
    {
        public string Symbol { get; set; }
        /// <summary>
        /// Интервал
        /// в секундах - для времеонного интервала
        /// в единицах - для volume или range бара
        /// </summary>
        public int Interval { get; set; }

        public DateTime DateTime { get; set; }

        public double Open { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double Close { get; set; }

        public double Volume { get; set; }

        public Bar() { }

        public Bar(DateTime dateTime, double open, double high, double low, double close, double volume)
        :this("", 0, dateTime, open, high, low, close, volume){}

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

            if (parts.Count() == 9)
            {
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
            {
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
            return ToString("Symbol: {0}, Interval: {1}, DateTime: {2}, Open: {3}, High: {4}, Low: {5}, Close: {6}, Volume: {7}");
        }

        public string ToImportString()
        {
            return ToString("{0},{1},{2:yyyyMMdd,HHmmss},{3},{4},{5},{6},{7}");
        }

        public string ToFinamString()
        {
            CultureInfo ci = CultureInfo.InvariantCulture;
            return String.Format("{0},{1},{2:yyyyMMdd,HHmmss},{3},{4},{5},{6},{7}", 
                this.Symbol, this.Interval, this.DateTime, this.Open.ToString("0.0000", ci), this.High.ToString("0.0000", ci), this.Low.ToString("0.0000", ci), this.Close.ToString("0.0000", ci), this.Volume.ToString("0.0000", ci));
        }

        private string ToString(string format)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;
            // заменить  this.DateTime.ToString(ci) на this.DateTime, протестировать
            return String.Format(format, this.Symbol, this.Interval, this.DateTime.ToString(ci), this.Open.ToString("0.0000", ci), this.High.ToString("0.0000", ci), this.Low.ToString("0.0000", ci), this.Close.ToString("0.0000", ci), this.Volume.ToString("0.0000", ci));
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
    }

}
