using System;
using TRL.Common.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TRL.Common.Test.Extensions
{
	[TestClass]
	public class ByteArrayExtensionsTests
	{
		[TestMethod]
		public void Extensions_ByteArray_AsString()
		{
			byte[] src = {72, 0, 101, 0, 108, 0, 108, 0, 111, 0 };

			string dst = src.AsString();

			Assert.AreEqual(5, dst.Length);
			Assert.AreEqual("Hello", dst);
		}
	}
}
