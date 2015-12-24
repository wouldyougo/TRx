using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using TRL.Common.Models;

//namespace TRL.Common.Models
//namespace TRL.Common.Extensions

namespace TRL.Common.Extensions.Models
{
    public static class BarExtensions
    {
        #region Text
        //public static Bar Parse(this Bar bar, string barString)
        //{
        //    string[] parts = barString.Split(',');

        //    CultureInfo provider = CultureInfo.InvariantCulture;

        //    string pattern = "yyyyMMdd HHmmss";

        //    string dateTime = string.Empty;

        //    if (parts.Count() == 9)
        //    {
        //        dateTime = String.Concat(parts[2], " ", parts[3]);

        //        return new Bar
        //        {
        //            Symbol = parts[0],
        //            Interval = Convert.ToInt16(parts[1], provider) * 60,
        //            DateTime = DateTime.ParseExact(dateTime, pattern, provider),
        //            Open = Convert.ToDouble(parts[4], provider),
        //            High = Convert.ToDouble(parts[5], provider),
        //            Low = Convert.ToDouble(parts[6], provider),
        //            Close = Convert.ToDouble(parts[7], provider),
        //            Volume = Convert.ToDouble(parts[8], provider)

        //            //Open = Convert.Todouble(parts[4], provider),
        //            //High = Convert.Todouble(parts[5], provider),
        //            //Low = Convert.Todouble(parts[6], provider),
        //            //Close = Convert.Todouble(parts[7], provider),
        //            //Volume = Convert.Todouble(parts[8], provider)
        //        };
        //    }
        //    else if (parts.Count() == 8)
        //    {
        //        dateTime = String.Concat(parts[1], " ", parts[2]);

        //        return new Bar
        //        {
        //            Symbol = parts[0],
        //            DateTime = DateTime.ParseExact(dateTime, pattern, provider),
        //            Open = Convert.ToDouble(parts[3], provider),
        //            High = Convert.ToDouble(parts[4], provider),
        //            Low = Convert.ToDouble(parts[5], provider),
        //            Close = Convert.ToDouble(parts[6], provider),
        //            Volume = Convert.ToDouble(parts[7], provider)

        //            //Open = Convert.Todouble(parts[3], provider),
        //            //High = Convert.Todouble(parts[4], provider),
        //            //Low = Convert.Todouble(parts[5], provider),
        //            //Close = Convert.Todouble(parts[6], provider),
        //            //Volume = Convert.Todouble(parts[7], provider)
        //        };
        //    }

        //    dateTime = String.Concat(parts[0], " ", parts[1]);

        //    return new Bar(DateTime.ParseExact(dateTime, pattern, provider),
        //        Convert.ToDouble(parts[2], provider),
        //        Convert.ToDouble(parts[3], provider),
        //        Convert.ToDouble(parts[4], provider),
        //        Convert.ToDouble(parts[5], provider),
        //        Convert.ToDouble(parts[6], provider));

        //        //Convert.Todouble(parts[2], provider),
        //        //Convert.Todouble(parts[3], provider),
        //        //Convert.Todouble(parts[4], provider),
        //        //Convert.Todouble(parts[5], provider),
        //        //Convert.Todouble(parts[6], provider));
        //}

        public static bool FirstFieldContainsDate(this Bar bar, string barString)
        {
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;

                string pattern = "yyyyMMdd";

                string first = barString.Split(',').ElementAt(0);

                DateTime date = DateTime.ParseExact(first, pattern, provider);

                return true;
            }
            catch
            {
                return false;
            }
        }

        //public static string ToString(this Bar bar)
        //{
        //    return bar.ToString("Symbol: {0}, Interval: {1}, DateTime: {2}, Open: {3}, High: {4}, Low: {5}, Close: {6}, Volume: {7}");
        //}

        //public static string ToImportString(this Bar bar)
        //{
        //    return bar.ToString("{0},{1},{2:yyyyMMdd,HHmmss},{3},{4},{5},{6},{7}");
        //}

        //public static string ToString(this Bar bar, string format)
        //{
        //    CultureInfo ci = CultureInfo.InvariantCulture;

        //    return String.Format(format, bar.Symbol, bar.Interval, bar.DateTime.ToString(ci),
        //        bar.Open.ToString("0.0000", ci), bar.High.ToString("0.0000", ci), bar.Low.ToString("0.0000", ci), bar.Close.ToString("0.0000", ci),
        //        bar.Volume.ToString("0.0000", ci));
        //}
        #endregion

        #region Размер и направление
        /// <summary>
        /// Min(Open, Close)
        /// </summary>
        /// <param name="bar"></param>
        /// <returns>Min(Open, Close)</returns>
        public static double BarDn(this Bar bar)
        {
            double res = 0;
            res = System.Math.Min(bar.Open, bar.Close);
            return res;
        }

        /// <summary>
        /// Max(Open, Close)
        /// </summary>
        /// <param name="bar"></param>
        /// <returns>Max(Open, Close)</returns>
        public static double BarUp(this Bar bar)
        {
            double res = 0;
            res = System.Math.Max(bar.Open, bar.Close);
            return res;
        }

        /// <summary>
        /// High - Low
        /// </summary>
        /// <param name="bar"></param>
        /// <returns>High - Low</returns>
        public static double BarLengthHL(this Bar bar)
        {
            double res = 0;
            res = bar.High - bar.Low;
            return res;
        }

        /// <summary>
        /// Open - Close
        /// </summary>
        /// <param name="bar"></param>
        /// <returns>Open - Close</returns>
        public static double BarLengthOC(this Bar bar)
        {
            double res = 0;
            res = bar.Open - bar.Close;
            return res;
        }

        /// <summary>
        /// Abs(Open - Close)
        /// </summary>
        /// <param name="bar"></param>
        /// <returns>Abs(Open - Close)</returns>
        public static double BarLengthOCAbs(this Bar bar)
        {
            double res = 0;
            res = bar.Open - bar.Close;
            res = Math.Abs(res);
            return res;
        }

        /// <summary>
        /// Дистанция, пройденная ценой за свечку
        /// </summary>
        /// <param name="bar"></param>
        /// <returns>HL + (HL - OC)</returns>
        public static double BarDistance(this Bar bar)
        {
            double res = 0;
            double hl = BarLengthHL(bar);
            double oc = BarLengthOCAbs(bar);

            res = hl + (hl - oc);
            return res;
        }


        /// <summary>
        /// Направление бара
        /// </summary>
        /// <param name="bar"></param>
        /// <returns>-1 0 1</returns>
        public static int BarDirection(this Bar bar)
        {
            int res = 0;
            if (bar.Close > bar.Open)
            {
                res = 1;
            }
            else if (bar.Open > bar.Close)
            {
                res = -1;
            }
            return res;
        }

        /// <summary>
        /// Направление бара
        /// </summary>
        /// <param name="bar"></param>
        /// <returns>true null false</returns>
        public static bool? BarDirectionBool(this Bar bar)
        {
            bool? res = null;
            if (bar.Close > bar.Open)
            {
                res = true;
            }
            else if (bar.Open > bar.Close)
            {
                res = false;
            }
            return res;
        }

        #endregion
    }
}
