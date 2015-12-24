using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRx.Indicators
{
    /// <summary>
    /// Уровни
    /// </summary>
    public class Levels
	{
        /// <summary>
        /// ТипУровней
        /// </summary>
        public int ТипУровней { get; set; }
        /// <summary>
        /// количество уровней в каждую сторону с нулевым
        /// </summary>
        public int ПоловинаУровней { get; set; }
        /// <summary>
        /// всего уровней котирования с учетом нулевого
        /// </summary>
        public int КоличествоУровней { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public double ШагУровней { get; set; }

        /// <summary>
        /// значение на каждом уровне от нуля 
        /// </summary>
        public System.Collections.Generic.IList<double> LevelValueUp { get; set; }
        /// <summary>
        /// значение на каждом уровне до нуля 
        /// </summary>
        public System.Collections.Generic.IList<double> LevelValueDn { get; set; }
        /// <summary>
        /// значение на каждом уровне
        /// </summary>
        public System.Collections.Generic.IList<double> LevelValue { get; set; }
        /// <summary>
        /// значения уровней от нуля (всего 2 точки на уровень или для каждого бара)
        /// </summary>
        public System.Collections.Generic.IList<IList<double>> ValuesUp { get; set; }
        /// <summary>
        /// значения уровней до нуля (всего 2 точки на уровень или для каждого бара)
        /// </summary>
        public System.Collections.Generic.IList<IList<double>> ValuesDn { get; set; }
        /// <summary>
        /// значения уровней (всего 2 точки на уровень или для каждого бара)
        /// </summary>
        public System.Collections.Generic.IList<IList<double>> Values { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IList<bool> ПересеченияСверху { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public IList<bool> ПересеченияСнизу { get; private set; }

        /// <summary>
        /// НомерТекущегоУровня
        /// </summary>
        public int УровеньТекущий { get; private set; }

        /// <summary>
        /// НомерТекущегоУровня
        /// </summary>
        public int УровеньПрошлый { get; private set; }

        /// <summary>
        /// НомерСреднегоУровня
        /// </summary>
        public int УровеньСредний { get; private set; }
        public bool ПересечениеСверху { get; private set; }
        public bool ПересечениеСнизу { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_КоличествоУровней">в каждую сторону</param>
        /// <param name="_ШагУровней"></param>
        public Levels(int _КоличествоУровней, double _ШагУровней, int _ТипУровней = 0)
        {
            this.ПоловинаУровней = _КоличествоУровней + 1;
            this.КоличествоУровней = _КоличествоУровней * 2 + 1;
            this.ТипУровней = _ТипУровней;
            this.ШагУровней = _ШагУровней;
            SetLevelValueUp();
            SetLevelValueDn();
            SetLevelValue();

            SetLevelValueUp();
            SetLevelValueDn();
            SetLevelValue();

            ПересеченияСверху = new bool[this.КоличествоУровней];
            ПересеченияСнизу  = new bool[this.КоличествоУровней];
            this.УровеньСредний = this.ПоловинаУровней - 1;
            this.УровеньТекущий = УровеньСредний;
            this.УровеньПрошлый = УровеньСредний;
        }

        /// <summary>
        /// значение на каждом уровне до нуля
        /// </summary>
        private void SetLevelValueDn()
        {
            LevelValueDn = new double[ПоловинаУровней];
            for (int j = 0; (j < ПоловинаУровней); j++)
            {
                LevelValueDn[j] = -this.ШагУровней * j;
                if (ТипУровней == 1)
                {
                    LevelValueDn[j] = -this.ШагУровней * (System.Math.Exp(j) - 1);
                }
                else if (ТипУровней == 2)
                {
                    LevelValueDn[j] = -this.ШагУровней * (System.Math.Log(j + 1));
                }
                else if (ТипУровней == 3)
                {
                    LevelValueDn[j] = -this.ШагУровней * (j + System.Math.Log(j + 1));
                }
                else if (ТипУровней == 4)
                {
                    LevelValueDn[j] = -this.ШагУровней * (j * System.Math.Log(j + 1));
                }
            }
        }
        /// <summary>
        /// значение на каждом уровне от нуля
        /// </summary>
        private void SetLevelValueUp()
        {
            LevelValueUp = new double[ПоловинаУровней];
            for (int j = 0; (j < ПоловинаУровней); j++)
            {
                LevelValueUp[j] = this.ШагУровней * j;
                if (ТипУровней == 1)
                {
                    LevelValueUp[j] = this.ШагУровней * (System.Math.Exp(j) - 1);
                }
                else if (ТипУровней == 2)
                {
                    LevelValueUp[j] = this.ШагУровней * (System.Math.Log(j + 1));
                }
                else if (ТипУровней == 3)
                {
                    LevelValueUp[j] = this.ШагУровней * (j + System.Math.Log(j + 1));
                }
                else if (ТипУровней == 4)
                {
                    LevelValueUp[j] = this.ШагУровней * (j * System.Math.Log(j + 1));
                }
            }
        }
        /// <summary>
        /// значение на каждом уровне
        /// </summary>
        private void SetLevelValue()
        {
            //System.Collections.Generic.IList<double> levelValue = new System.Collections.Generic.List<double>();
            LevelValue = new System.Collections.Generic.List<double>();
            //добавляем значения уровней
            for (int j = ПоловинаУровней - 1; j > 0; j--)
            {
                LevelValue.Add(LevelValueDn[j]);
            }
            for (int j = 0; (j < ПоловинаУровней); j++)
            {
                LevelValue.Add(LevelValueUp[j]);
            }
        }
        /// <summary>
        /// базовый список уровней верхних
        /// всего 2 точки на уровень
        /// </summary>
        private void SetValueUp()
        {
            ValuesUp = new IList<double>[ПоловинаУровней];
            for (int j = 0; (j < ПоловинаУровней); j++)
            {
                int count = 2;
                double[] list = new double[count];
                for (int i = 0; (i < count); i++)
                {
                    list[i] = LevelValueUp[j];
                }
                ValuesUp[j] = list;
            }
        }
        /// <summary>
        /// базовый список уровней нижних
        /// всего 2 точки на уровень
        /// </summary>
        private void SetValueDn()
        {
            ValuesDn = new IList<double>[ПоловинаУровней];
            for (int j = 0; (j < ПоловинаУровней); j++)
            {
                int count = 2;
                double[] list = new double[count];
                for (int i = 0; (i < count); i++)
                {
                    list[i] = LevelValueDn[j];
                }
                ValuesDn[j] = list;
            }
        }
        /// <summary>
        /// базовый список всех уровней
        /// всего 2 точки на уровень
        /// </summary>
        private void SetValue()
        {
            //список всех уровней
            Values = new System.Collections.Generic.List<IList<double>>();
            for (int j = ПоловинаУровней - 1; j > 0; j--)
            {
                Values.Add(ValuesDn[j]);
            }
            for (int j = 0; (j < ПоловинаУровней); j++)
            {
                Values.Add(ValuesUp[j]);
            }
        }

        /// <summary>
        /// Определяем ПересеченияСверху[КоличествоУровней] последними значениями источника данных
        /// </summary>
        /// <param name="src2">источник данных</param>
        /// <returns>ПересеченияСверху[КоличествоУровней]</returns>
        public IList<bool> ПересеченияУровенейСверху(IList<double> src2)
        {
            for (int j = 0; j < КоличествоУровней; j++)
            {
                ПересеченияСверху[j] = Indicator.CrossOver(LevelValue[j], src2, src2.Count-1);
            }
            return ПересеченияСверху;
        }
        /// <summary>
        /// Определяем ПересеченияСнизу[КоличествоУровней] последними значениями источника данных
        /// </summary>
        /// <param name="src2">источник данных</param>
        /// <returns>ПересеченияСнизу[КоличествоУровней]</returns>
        public IList<bool> ПересеченияУровенейСнизу(IList<double> src2)
        {
            for (int j = 0; j < КоличествоУровней; j++)
            {
                ПересеченияСнизу[j] = Indicator.CrossUnder(LevelValue[j], src2, src2.Count - 1);
            }
            return ПересеченияСнизу;
        }

        /// <summary>
        /// Определяем ПересечениеСверху. Группируем ПересеченияСверху
        /// </summary>
        /// <returns>ПересечениеСверху</returns>
        public bool ПересечениеУровенейСверху()
        {
            ПересечениеСверху = false;
            for (int j = 0; j < КоличествоУровней; j++)
            {
                ПересечениеСверху = ПересечениеСверху || ПересеченияСверху[j];
            }
            return ПересечениеСверху;
        }
        /// <summary>
        /// Определяем ПересечениеСнизу. Группирует ПересеченияСнизу
        /// </summary>
        /// <returns>ПересечениеСнизу</returns>
        public bool ПересечениеУровенейСнизу()
        {
            ПересечениеСнизу = false;
            for (int j = 0; j < КоличествоУровней; j++)
            {
                ПересечениеСнизу = ПересечениеСнизу || ПересеченияСнизу[j];
            }
            return ПересечениеСнизу;
        }

        /// <summary>
        /// Номер пересеченого уровня
        /// </summary>
        /// <returns>номер последнего пересеченого уровня</returns>
        public int НайтиТекущийУровень()
        {
            //Уровень
            int Уровень = -1;
            for (int j = 0; j < КоличествоУровней; j++)
            {
                if (ПересеченияСнизу[j])
                {
                    Уровень = j;
                }
            }
            for (int j = КоличествоУровней - 1; j >= 0; j--)
            {
                if (ПересеченияСверху[j])
                {
                    Уровень = j;
                }
            }
            //if (Уровень == -1)
            //{
            //    Уровень[i] = Уровень[i - 1];
            //}
            //если было пересечение какого-то уровня
            if (Уровень != -1)
            {
                if (Уровень != УровеньТекущий)
                {
                    УровеньПрошлый = УровеньТекущий;
                    УровеньТекущий = Уровень;
                }
            }
            return УровеньТекущий;
        }
        public void Do(IList<double> src)
        {
            ПересеченияУровенейСверху(src);
            ПересечениеУровенейСверху();
            //переделать
            //можно вычислять только половину
            //если есть ПересечениеСверху
            //то ПересечениеСнизу быть не может
            ПересеченияУровенейСнизу(src);
            ПересечениеУровенейСнизу();

            НайтиТекущийУровень();
        }
    }
}
