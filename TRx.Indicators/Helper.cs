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
    }
}
