using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace TSLab.Script.Helpers
namespace TRx.Indicators
{
    public static partial class Indicator
	{      
        /// <summary>
        /// Chande Momentum Oscillator (CMO)
        /// CMO(i) = (UpSum(i) - DnSum(i))/(UpSum(i) + DnSum(i))
        /// UpSum(i) - текущая сумма положительных приращений цены за период;
        /// DnSum(i) - текущая сумма отрицательных приращений цены за период.
        /// </summary>
        /// <param name="p">источник</param>
        /// <param name="period">период</param>
        /// <returns></returns>
        public static IList<double> ChandeMomentumOscillator(IList<double> p, int period)
        {
            int count = p.Count;
            double[] cmo = new double[count];
            double[] arrayUp = new double[count];
            double[] arrayDn = new double[count];
            double UpSum = 0.0;
            double DnSum = 0.0;
            for (int i = 0; i < count; i++)
            {
                if (i < period)
                {
                    cmo[i] = 0.0;
                }
                else
                {
                    double diff = p[i] - p[i - 1];
                    arrayUp[i] = ((diff > 0.0) ? diff : 0.0);
                    arrayDn[i] = ((diff < 0.0) ? (-diff) : 0.0);
                    /// UpSum(i) - текущая сумма положительных приращений цены за период;
                    /// DnSum(i) - текущая сумма отрицательных приращений цены за период.
                    UpSum += arrayUp[i];
                    DnSum += arrayDn[i];
                    if (i >= period)
                    {
                        UpSum -= arrayUp[i - period];
                        DnSum -= arrayDn[i - period];
                    }
                    //CMO(i) = (UpSum(i) - DnSum(i))/(UpSum(i) + DnSum(i))
                    cmo[i] = (UpSum - DnSum) / (UpSum + DnSum) * 100.0;
                }
            }
            return cmo;
        }

        /// <summary>
        ///Variable Index Dynamic
        ///b(i) = α*ABS(CMO(i)) - переменный фактор сглаживания;
        ///EMA(i) = EMA(i−1) + α*(p(i) − EMA(i−1))
        ///EMA(i) = α*p(i) + (1 − α)*EMA(i−1)
        ///α = 2/(Period + 1) - фактор сглаживания;
        ///VIDYA(i) = b(i)*p(i) + (1 - b(i))*VIDYA(i-1)
        ///VIDYA(i) = VIDYA(i-1) + b(i)*(p(i) - VIDYA(i-1))
        /// </summary>
        /// <param name="p">источник</param>
        /// <param name="period">период</param>
        /// <returns></returns>
        public static IList<double> VIDY(IList<double> p, int periodOsc)
        {
            IList<double> v = ChandeMomentumOscillator(p, periodOsc);

            double[] b = new double[p.Count];
            int period = 1;
            double a = 2.0 / (period + 1);

            for (int i = 0; i < p.Count; i++)
            {
                ///b(i) = α*ABS(CMO(i)) - переменный фактор сглаживания;
                //b = a * d[i];
                b[i] = a * Math.Abs(v[i] / 100.0);
                ///EMA(i) = EMA(i−1) + α⋅(p(i) − EMA(i−1))
                ///VIDYA(i) = VIDYA(i-1) + b(i)*(p(i) - VIDYA(i-1))
            }
            return b;
        }

        /// <summary>
        ///Variable Index Dynamic
        ///b(i) = α*ABS(CMO(i)) - переменный фактор сглаживания;
        ///EMA(i) = EMA(i−1) + α*(p(i) − EMA(i−1))
        ///EMA(i) = α*p(i) + (1 − α)*EMA(i−1)
        ///α = 2/(Period + 1) - фактор сглаживания;
        ///VIDYA(i) = b(i)*p(i) + (1 - b(i))*VIDYA(i-1)
        ///VIDYA(i) = VIDYA(i-1) + b(i)*(p(i) - VIDYA(i-1))
        /// </summary>
        /// <param name="p">источник</param>
        /// <param name="period">период</param>
        /// <returns></returns>
        public static IList<double> VIDY2p(IList<double> p, double period, int periodOsc)
        {
            IList<double> v = ChandeMomentumOscillator(p, periodOsc);

            double[] b = new double[p.Count];
            double a = 2.0 / (period + 1);

            for (int i = 0; i < p.Count; i++)
            {
                ///b(i) = α*ABS(CMO(i)) - переменный фактор сглаживания;
                //b = a * d[i];
                b[i] = a * Math.Abs(v[i] / 100.0);
                ///EMA(i) = EMA(i−1) + α⋅(p(i) − EMA(i−1))
                ///VIDYA(i) = VIDYA(i-1) + b(i)*(p(i) - VIDYA(i-1))
            }
            return b;
        }
    }
}
