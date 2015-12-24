using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRL.Message;
using TRL.Common.Extensions;

namespace TRL.Message.Test
{
	[TestClass]
	public class ManagementProtocolMessagesTests
	{
		[TestMethod]
        public void Message_ConnectMessage()
		{
			Guid srcAddr = Guid.NewGuid();
			Guid dstAddr = Guid.NewGuid();
			
			IMessage connectMsg = 
				new ConnectMessage(srcAddr, dstAddr);

			Assert.AreEqual(srcAddr, new Guid(connectMsg.GetSourceAddressBytes()));
			Assert.AreEqual(dstAddr, new Guid(connectMsg.GetDestinationAddressBytes()));
			Assert.AreEqual("connect", connectMsg.GetContentBytes().AsString());
		}

		[TestMethod]
        public void Message_DisconnectMessage()
		{
			Guid srcAddr = Guid.NewGuid();
			Guid dstAddr = Guid.NewGuid();

			IMessage msg = 
				new DisconnectMessage(srcAddr, dstAddr);

			Assert.AreEqual(srcAddr, new Guid(msg.GetSourceAddressBytes()));
			Assert.AreEqual(dstAddr, new Guid(msg.GetDestinationAddressBytes()));

			Assert.AreEqual("disconnect", msg.GetContentBytes().AsString());
		}

		[TestMethod]
        public void Message_KeepaliveMessage()
		{
			Guid srcAddr = Guid.NewGuid();
			Guid dstAddr = Guid.NewGuid();

			IMessage msg = 
				new KeepaliveMessage(srcAddr, dstAddr);

			Assert.AreEqual(srcAddr, new Guid(msg.GetSourceAddressBytes()));
			Assert.AreEqual(dstAddr, new Guid(msg.GetDestinationAddressBytes()));

			Assert.AreEqual("ping", msg.GetContentBytes().AsString());
		}

		[TestMethod]
        public void Message_OutOfServiceMessage()
		{
			Guid srcAddr = Guid.NewGuid();
			Guid dstAddr = Guid.NewGuid();
			
			IMessage msg = 
				new OutOfServiceMessage(srcAddr, dstAddr);

			Assert.AreEqual(srcAddr, new Guid(msg.GetSourceAddressBytes()));
			Assert.AreEqual(dstAddr, new Guid(msg.GetDestinationAddressBytes()));
			Assert.AreEqual("out of service", msg.GetContentBytes().AsString());
		}

		[TestMethod]
        public void Message_InServiceMessage()
		{
			Guid srcAddr = Guid.NewGuid();
			Guid dstAddr = Guid.NewGuid();

			IMessage msg = 
				new InServiceMessage(srcAddr, dstAddr);

			Assert.AreEqual(srcAddr, new Guid(msg.GetSourceAddressBytes()));
			Assert.AreEqual(dstAddr, new Guid(msg.GetDestinationAddressBytes()));
			Assert.AreEqual("in service", msg.GetContentBytes().AsString());
		}
	}
}
