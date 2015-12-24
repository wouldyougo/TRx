using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TRL.Csharp.Test
{
    [TestClass]
    public class DoubleTests
    {
        public double Price;
        public double Amount;

        [TestInitialize]
        public void Setup()
        {
            Amount = 10;
        }

        [TestMethod]
        public void doubles_addition_test() 
        {
            double apples = 11.5;
            double oranges = 10;
            double fruits = apples + oranges;
            Assert.AreEqual(21.5, fruits);
        }

        [TestMethod]
        public void doubles_divison_test()
        {
            double numerator = 5;
            double denominator = 2;
            double result = numerator / denominator;
            Assert.AreEqual(2.5, result);
        }

        [TestMethod]
        public void doubles_multiplication_tests()
        {
            Price = 2.5;
            double sum = Price * Amount;
            Assert.AreEqual(25, sum);
        }

        [TestMethod]
        public void default_Price_value_test()
        {
            Assert.AreEqual(0, Price);
        }

        [TestMethod]
        public void change_Price_value_test()
        {
            Price = 55.01;
            Assert.AreEqual(55.01, Price);
        }
    }
}
