using System;

namespace TRL.Message
{
	public interface IMessage
	{
		byte[] GetSourceAddressBytes();
		byte[] GetDestinationAddressBytes();
		byte[] GetContentBytes();
	}
}
