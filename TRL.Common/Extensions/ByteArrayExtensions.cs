using System;

namespace TRL.Common.Extensions
{
	public static class ByteArrayExtensions
	{
		public static string AsString(this byte[] bytes)
		{
			char[] result = new char[bytes.Length / sizeof(char)];

			Buffer.BlockCopy(bytes, 0, result, 0, bytes.Length);

			return new string(result);
		}
	}
}
