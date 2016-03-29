using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace TRx.Indicators
{
    /// <summary>
    /// Вспомогательный класс, содержащий хелпер методы. В том числе и методы расширения.
    /// </summary>
    public static class Helper
    {
        #region Общие хелперы
        /// <summary>
        /// Метод упрощающий форматирование строк.
        /// </summary>
        /// <param name="str">Строка для форматирования.</param>
        /// <param name="args">Аргументы для форматирования.</param>
        /// <returns></returns>
        public static string Put(this string str, params object[] args)
        {
            // Вводим культуру для того чтобы double числа не переводились как числа с запятой! Это гадит.
            return string.Format(CultureInfo.InvariantCulture, str, args);
        }
        #endregion


        /// <summary>
        /// Дает расчет параболического стопа для позиции. Расчет идет по формуле 
        /// y = a*x^2 + b*x + c, где x - число бар удержания позы.
        /// </summary>
        /// <param name="posEntryBarNum">x = barNumber - posEntryBarNum</param>
        /// <param name="barNumber">x = barNumber - posEntryBarNum</param>
        /// <param name="posIsLong">Направление параболы по y</param>
        /// <param name="a">Коэффициент меняющий вид параболы. Положительное число.</param>
        /// <param name="posEntryPrice">c = posEntryPrice +(-) StopDelta</param>
        /// <param name="StopDelta">c = posEntryPrice +(-) StopDelta</param>
        /// <returns>StopPrice</returns>
        public static double StopParabola(  int posEntryBarNum,
                                            int barNumber,
                                            bool posIsLong,
                                            double a,
                                            double posEntryPrice,
                                            double StopDelta
                                            )
        {
            if (barNumber < posEntryBarNum)
                throw new ArgumentOutOfRangeException("barNumber", "номер бара должен быть больше чем бар открытия позиции.");

            if (StopDelta < 0)
                throw new ArgumentOutOfRangeException("StopDelta", "delta должен быть положительным числом или 0.");

            if (a <= 0)
                throw new ArgumentOutOfRangeException("k", "k должен быть положительным числом.");

            var x = barNumber - posEntryBarNum + 1;      // Время удержания позиции

            double c;
            if (posIsLong)
            {
                //a = a;
                c = posEntryPrice - StopDelta;
            }
            else {
                a = -a;
                c = posEntryPrice + StopDelta;
            }
            //a*x*x + c;
            return Parabola(x, a, 0, c);
        }
        /// <summary>
        /// y = a*x^2 + b*x + c
        /// вершина xe = -b/(2*a)
        /// вершина ye = -(b^2 - 4*a*c)/4*a
        /// </summary>
        /// <remarks>
        /// <see ref="https://ru.wikipedia.org/wiki/%D0%9A%D0%B2%D0%B0%D0%B4%D1%80%D0%B0%D1%82%D0%B8%D1%87%D0%BD%D0%B0%D1%8F_%D1%84%D1%83%D0%BD%D0%BA%D1%86%D0%B8%D1%8F"/>
        /// </remarks> 
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Parabola(double x, double a, double b, double c)
        {
            return a*x*x + b*x + c;
        }
    }
}
