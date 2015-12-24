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
        ///Variable Moving Average
        ///EMA(i) = EMA(i−1) + α*(p(i) − EMA(i−1))
        ///EMA(i) = α*p(i) + (1 − α)*EMA(i−1)
        ///α = 2/(Period + 1) - фактор сглаживания;
        ///VMA(i) = VMA(i−1) + b*(p(i) − VMA(i−1))
        ///b(i) = α*VR(i) - переменный фактор сглаживания;
        ///VR — коэффициент волатильности (Volatility Ratio)
        /// </summary>
        /// <param name="p">источник</param>
        /// <param name="period">период средней</param>
        /// <param name="v">переменный фактор сглаживания</param>
        /// <returns></returns>
        public static IList<double> VMA(IList<double> p, double period, IList<double> v)
        {
            double[] vma = new double[p.Count];
            double a = 2.0 / (period + 1);
            double b = 0;

            vma[0] = p[0];
            for (int i = 1; i < p.Count; i++)
            {
             ///b(i) = α*ABS(CMO(i)) - переменный фактор сглаживания;
                b = a * v[i];
             ///EMA(i) = EMA(i−1) + α⋅(p(i) − EMA(i−1))
             ///VIDYA(i) = VIDYA(i-1) + b(i)*(p(i) - VIDYA(i-1))
                vma[i] = vma[i - 1] + b * (p[i] - vma[i - 1]);
            }
            return vma;
        }

        /// <summary>
        ///Variable Index Dynamic Average
        ///EMA(i) = EMA(i−1) + α*(p(i) − EMA(i−1))
        ///EMA(i) = α*p(i) + (1 − α)*EMA(i−1)
        ///α = 2/(Period + 1) - фактор сглаживания;
        ///VIDYA(i) = b(i)*p(i) + (1 - b(i))*VIDYA(i-1)
        ///VIDYA(i) = VIDYA(i-1) + b(i)*(p(i) - VIDYA(i-1))
        ///b(i) = α*ABS(CMO(i)) - переменный фактор сглаживания;
        /// </summary>
        /// <param name="p">источник</param>
        /// <param name="periodV">период весовой функции</param>
        /// <returns></returns>
        public static IList<double> VIDYA(IList<double> p, int periodV)
        {
            IList<double> v = ChandeMomentumOscillator(p, periodV);

            double[] vidya = new double[p.Count];
            //double a = 2.0 / (period + 1);
            double a = 1;
            double b = 0;

            vidya[0] = p[0];
            for (int i = 1; i < p.Count; i++)
            {
                ///b(i) = α*ABS(CMO(i)) - переменный фактор сглаживания;
                //b = a * d[i];
                b = a * Math.Abs(v[i] / 100.0);
                ///EMA(i) = EMA(i−1) + α⋅(p(i) − EMA(i−1))
                ///VIDYA(i) = VIDYA(i-1) + b(i)*(p(i) - VIDYA(i-1))
                vidya[i] = vidya[i - 1] + b * (p[i] - vidya[i - 1]);
            }
            return vidya;
        }

        /// <summary>
        ///Variable Index Dynamic Average
        ///EMA(i) = EMA(i−1) + α*(p(i) − EMA(i−1))
        ///EMA(i) = α*p(i) + (1 − α)*EMA(i−1)
        ///α = 2/(Period + 1) - фактор сглаживания;
        ///VIDYA(i) = b(i)*p(i) + (1 - b(i))*VIDYA(i-1)
        ///VIDYA(i) = VIDYA(i-1) + b(i)*(p(i) - VIDYA(i-1))
        ///b(i) = α*ABS(CMO(i)) - переменный фактор сглаживания;
        /// </summary>
        /// <param name="p">источник</param>
        /// <param name="period">период средней и период весовой функции</param>
        /// <returns></returns>
        public static IList<double> VIDYA2(IList<double> p, int period)
        {
            IList<double> v = ChandeMomentumOscillator(p, period);

            double[] vidya = new double[p.Count];

            //double a = 1;
            double a = 2.0 / (period + 1);
            double b = 0;

            //начало среднее
            int _period = Math.Min(period, p.Count);
            vidya[0] = p.Take(_period).Average();
            //a = 2.0 / (period - I + 1)
            for (int i = 1; i < _period; i++)
            {
                vidya[i] = vidya[i - 1] + 2.0 / (_period - i + 1) * (p[i] - vidya[i - 1]);
            }
            for (int i = _period; i < p.Count; i++)
            {
                ///b(i) = a*ABS(CMO(i)) - переменный фактор сглаживания;
                //b = a * b[i];
                //b = a * Math.Abs(v[i] / 100.0);
                b = Math.Abs(v[i] / 100.0);
                ///EMA(i) = EMA(i−1) + α⋅(p(i) − EMA(i−1))
                ///VIDYA(i) = VIDYA(i-1) + b(i)*(p(i) - VIDYA(i-1))
                vidya[i] = vidya[i - 1] + b * (p[i] - vidya[i - 1]);
            }
            return vidya;
        }
        /// <summary>
        ///Variable Index Dynamic Average
        ///EMA(i) = EMA(i−1) + α*(p(i) − EMA(i−1))
        ///EMA(i) = α*p(i) + (1 − α)*EMA(i−1)
        ///α = 2/(Period + 1) - фактор сглаживания;
        ///VIDYA(i) = b(i)*p(i) + (1 - b(i))*VIDYA(i-1)
        ///VIDYA(i) = VIDYA(i-1) + b(i)*(p(i) - VIDYA(i-1))
        ///b(i) = α*ABS(CMO(i)) - переменный фактор сглаживания;
        /// </summary>
        /// <param name="p">источник</param>
        /// <param name="period">период средней и период весовой функции</param>
        /// <returns></returns>
        public static IList<double> VIDYA1p(IList<double> p, double period)
        {
            IList<double> v = ChandeMomentumOscillator(p, (int)period);

            double[] vidya = new double[p.Count];
            double a = 2.0 / (period + 1);
            double b = 0;

            vidya[0] = p[0];
            for (int i = 1; i < p.Count; i++)
            {
                ///b(i) = α*ABS(CMO(i)) - переменный фактор сглаживания;
                //b = a * d[i];
                b = a * Math.Abs(v[i] / 100.0);
                ///EMA(i) = EMA(i−1) + α⋅(p(i) − EMA(i−1))
                ///VIDYA(i) = VIDYA(i-1) + b(i)*(p(i) - VIDYA(i-1))
                vidya[i] = vidya[i - 1] + b * (p[i] - vidya[i - 1]);
            }
            return vidya;
        }
        /// <summary>
        ///Variable Index Dynamic Average
        ///EMA(i) = EMA(i−1) + α*(p(i) − EMA(i−1))
        ///EMA(i) = α*p(i) + (1 − α)*EMA(i−1)
        ///α = 2/(Period + 1) - фактор сглаживания;
        ///VIDYA(i) = b(i)*p(i) + (1 - b(i))*VIDYA(i-1)
        ///VIDYA(i) = VIDYA(i-1) + b(i)*(p(i) - VIDYA(i-1))
        ///b(i) = α*ABS(CMO(i)) - переменный фактор сглаживания;
        /// </summary>
        /// <param name="p">источник</param>
        /// <param name="period">период средней</param>
        /// <param name="periodV">период весовой функции</param>
        /// <returns></returns>
        public static IList<double> VIDYA2p(IList<double> p, double period, int periodV)
        {
            IList<double> v = ChandeMomentumOscillator(p, periodV);

            double[] vidya = new double[p.Count];
            double a = 2.0 / (period + 1);
            double b = 0;

            vidya[0] = p[0];
            for (int i = 1; i < p.Count; i++)
            {
                ///b(i) = α*ABS(CMO(i)) - переменный фактор сглаживания;
                //b = a * b[i];
                b = a * Math.Abs(v[i] / 100.0);
                ///EMA(i) = EMA(i−1) + α⋅(p(i) − EMA(i−1))
                ///VIDYA(i) = VIDYA(i-1) + b(i)*(p(i) - VIDYA(i-1))
                vidya[i] = vidya[i - 1] + b * (p[i] - vidya[i - 1]);
            }
            return vidya;
        }
    }
}
