using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common.Extensions
{
    public static class CrossLinesExtensions
    {
        public static bool CrossUnder(this IEnumerable<double> first, IEnumerable<double> second)
        {
            int fCount = first.Count();
            int sCount = second.Count();

            if (fCount == 0 || sCount == 0)
                return false;

            double[] fArray = first.ToArray();
            double[] sArray = second.ToArray();

            bool under = false;

            int iCount = (fCount < sCount) ? fCount : sCount;

            for (int i = 0; i < iCount; i++)
            {
                if (fArray[i] == 0 || sArray[i] == 0)
                    continue;

                if (fArray[i] < sArray[i])
                    under = true;

                if (fArray[i] >= sArray[i])
                {
                    if (under)
                        return true;
                }
            }
            return false;
        }

        public static bool CrossOver(this IEnumerable<double> first, IEnumerable<double> second)
        {
            int fCount = first.Count();
            int sCount = second.Count();

            if (fCount == 0 || sCount == 0)
                return false;

            double[] fArray = first.ToArray();
            double[] sArray = second.ToArray();

            bool over = false;
            int iCount = (fCount < sCount) ? fCount : sCount;

            for (int i = 0; i < iCount; i++)
            {
                if (fArray[i] == 0 || sArray[i] == 0)
                    continue;

                if (fArray[i] > sArray[i])
                    over = true;

                if (fArray[i] <= sArray[i])
                {
                    if (over)
                        return true;
                }
            }
            return false;
        }
    }
}
