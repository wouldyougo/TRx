using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TRL.Csharp.Test
{
    [TestClass]
    public class DelegateTests
    {
        public int Count;
        public delegate void Update();

        public void ChangeCount(Update method)
        {
            method();
        }

        public void Increment()
        {
            Count++;
        }

        [TestMethod]
        public void change_Count_with_Increment_test()
        {
            ChangeCount(Increment);
            Assert.AreEqual(1, Count);
        }

        public void Decrement()
        {
            Count--;
        }

        [TestMethod]
        public void change_Count_with_Decrement_test()
        {
            ChangeCount(Decrement);
            Assert.AreEqual(-1, Count);
        }

        public delegate void Calculate(int value);

        public void ChangeCount(Calculate method, int value)
        {
            method(value);
        }

        public void Add(int value)
        {
            Count = Count + value;
        }

        [TestMethod]
        public void add_value_to_Count_test()
        {
            int value = 10;
            ChangeCount(Add, value);
            Assert.AreEqual(value, Count);
        }

        public void Subtract(int value)
        {
            Count = Count - value;
        }

        [TestMethod]
        public void subtract_value_from_Count_test()
        {
            int value = 10;
            ChangeCount(Subtract, value);
            Assert.AreEqual(-value, Count);
        }
    }
}
