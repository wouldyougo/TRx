using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Extensions.Data
{
    public static class CorrelationExtensions
    {
        public static double CalculateEuclideanDistanceWith(this IEnumerable<double> pattern, IEnumerable<double> values)
        {
            double result = 0;

            double[] src = pattern.ToArray();
            double[] dst = values.ToArray();

            for (int i = 0; i < src.Length; i++)
                result += Math.Pow(src[i] - dst[i], 2);

            return 1 / (1 + Math.Sqrt(result));
        }

        public static double CalculatePearsonCorrelationWith(this IEnumerable<double> pattern, IEnumerable<double> values)
        {
            int pCount = pattern.Count();
            int vCount = values.Count();

            if (pCount == 0 || vCount == 0)
                return 0;

            if (pCount != vCount)
                return 0;

            double pSum = pattern.Sum();
            double vSum = values.Sum();

            double pSqSum = pattern.Sum(i => Math.Pow(i, 2));
            double vSqSum = values.Sum(i => Math.Pow(i, 2));

            double[] src = pattern.ToArray();
            double[] dst = values.ToArray();

            double multiplicationSum = 0;

            for (int i = 0; i < pCount; i++)
                multiplicationSum += src[i] * dst[i];

            double num = multiplicationSum - (pSum * vSum / pCount);
            double den = Math.Sqrt((pSqSum - Math.Pow(pSum, 2) / pCount) * (vSqSum - Math.Pow(vSum, 2) / pCount));

            if (den == 0)
                return den;

            return num / den;
        }

        public static double CalculatePearsonCorrelation(this List<double> data, int offset, List<double> pattern)
        {
            double dataSum = 0;
            double patternSum = 0;

            double dataSumOfSqr = 0;
            double patternSumOfSqr = 0;

            double multiplicationSum = 0;

            for (int i = 0; i < pattern.Count; i++)
            {
                dataSum += data[i + offset];
                patternSum += pattern[i];

                dataSumOfSqr += Math.Pow(data[i + offset], 2);
                patternSumOfSqr += Math.Pow(pattern[i], 2);

                multiplicationSum += data[i + offset] * pattern[i];
            }

            double num = multiplicationSum - (dataSum * patternSum / pattern.Count);
            double den = Math.Sqrt((dataSumOfSqr - Math.Pow(dataSum, 2) / pattern.Count) * (patternSumOfSqr - Math.Pow(patternSum, 2) / pattern.Count));

            if (den == 0)
                return den;

            return num / den;
        }
    }
}
