using System;

namespace TRL.Common.Extensions
{
	public static class StringExtensions
	{
		public static byte[] AsByteArray(this string str)
		{
			byte[] result = new byte[str.Length * sizeof(char)];

			Buffer.BlockCopy(str.ToCharArray(), 0, result, 0, result.Length);

			return result;
		}

	}
}
