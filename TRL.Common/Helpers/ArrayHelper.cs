using System;

namespace Ecng.Common
{
    public static class ArrayHelper
    {
        public static T[] Empty<T>()
        {
            return ArrayHelper.EmptyArrayHolder<T>.Array;
        }

        public static void Clear(this Array array)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            ArrayHelper.Clear(array, 0, array.Length);
        }

        public static void Clear(this Array array, int index, int count)
        {
            Array.Clear(array, index, count);
        }

        public static T[] Range<T>(this T[] array, int index)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            return ArrayHelper.Range<T>(array, index, array.Length - index);
        }

        public static T[] Range<T>(this T[] array, int index, int count)
        {
            T[] objArray = new T[count];
            Array.Copy((Array)array, index, (Array)objArray, 0, count);
            return objArray;
        }

        public static Array CreateArray(this Type type, int count)
        {
            return Array.CreateInstance(type, count);
        }

        public static int IndexOf<T>(this T[] array, T item)
        {
            return Array.IndexOf<T>(array, item);
        }

        public static T[] Clone<T>(this T[] array)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            return (T[])array.Clone();
        }

        public static T[] Reverse<T>(this T[] array)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            T[] objArray = (T[])array.Clone();
            Array.Reverse((Array)objArray);
            return objArray;
        }

        public static T[] Concat<T>(this T[] first, T[] second)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");
            T[] objArray = new T[first.Length + second.Length];
            if (objArray.Length == 0)
                return objArray;
            Array.Copy((Array)first, (Array)objArray, first.Length);
            Array.Copy((Array)second, 0, (Array)objArray, first.Length, second.Length);
            return objArray;
        }

        private static class EmptyArrayHolder<T>
        {
            public static readonly T[] Array = new T[0];
        }
    }
}