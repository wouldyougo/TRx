using System;
//using TRL.Common.Extensions.Data;
using TRL.Common.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TRL.Common.Test.Extensions
{
	[TestClass]
	public class StringExtensionsTests
	{
		[TestMethod]
		public void Extensions_String_AsByteArray()
		{
			string src = "Hello";

			byte[] dst = src.AsByteArray();

			Assert.AreEqual(10, dst.Length);
			Assert.AreEqual(72, dst[0]);
			Assert.AreEqual(0, dst[1]);
			Assert.AreEqual(101, dst[2]);
			Assert.AreEqual(0, dst[3]);
			Assert.AreEqual(108, dst[4]);
			Assert.AreEqual(0, dst[5]);
			Assert.AreEqual(108, dst[6]);
			Assert.AreEqual(0, dst[7]);
			Assert.AreEqual(111, dst[8]);
			Assert.AreEqual(0, dst[9]);
		}

	}
}
