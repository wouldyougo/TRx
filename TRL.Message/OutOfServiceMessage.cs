using System;
using TRL.Common.Extensions;

namespace TRL.Message
{
	public class OutOfServiceMessage : IMessage
	{
		private Guid sourceAddress;
		private Guid destinationAddress;

		private OutOfServiceMessage(){}

		public OutOfServiceMessage(Guid srcAddr, Guid dstAddr)
		{
			this.sourceAddress = srcAddr;
			this.destinationAddress = dstAddr;
		}

		public byte[] GetSourceAddressBytes()
		{
			return this.sourceAddress.ToByteArray();
		}

		public byte[] GetDestinationAddressBytes()
		{
			return this.destinationAddress.ToByteArray();
		}

		public byte[] GetContentBytes()
		{
			string content = "out of service";

			return content.AsByteArray();
		}
	}
}
