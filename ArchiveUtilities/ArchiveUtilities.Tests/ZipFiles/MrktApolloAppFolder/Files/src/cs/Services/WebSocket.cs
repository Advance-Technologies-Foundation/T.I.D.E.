using System;
using Terrasoft.Core;
using Terrasoft.Core.Factories;
using Terrasoft.Messaging.Common;

namespace MrktApolloApp.Services
{
	internal interface IWebSocket
	{

		#region Methods: Public

		void PostMessage(string senderName, string commandName, string schemaName, Guid recordId, string message,
			UserConnection userConnection);

		void PostMessage(string senderName, string commandName, string schemaName, Guid recordId, string message);

		void PostMessageToAll(string senderName, string commandName, string schemaName, Guid recordId, string message);

		#endregion

	}

	internal class WebSocket : IWebSocket
	{

		#region Fields: Private

		private readonly IMsgChannelManager _msgChannelManager;

		#endregion

		#region Constructors: Public

		public WebSocket(IMsgChannelManager msgChannelManager){
			_msgChannelManager = msgChannelManager;
		}

		#endregion

		#region Methods: Private

		private static IMsg CreateMessage(string sender, string msg){
			IMsg simpleMessage = new SimpleMessage {
				Id = Guid.NewGuid(),
				Body = msg
			};
			simpleMessage.Header.Sender = sender;
			return simpleMessage;
		}

		#endregion

		#region Methods: Public

		public void PostMessage(string senderName, string commandName, string schemaName, Guid recordId, string message,
			UserConnection userConnection){
			if (userConnection == null) {
				userConnection = ClassFactory.Get<UserConnection>();
			}

			IMsgChannel userChannel = _msgChannelManager.FindItemByUId(userConnection.CurrentUser.Id);
			string msgText = new Dto.WebSocket(commandName, schemaName, recordId, message).ToString();
			IMsg msg = CreateMessage(senderName, msgText);
			userChannel.PostMessage(msg);
		}

		public void PostMessage(string senderName, string commandName, string schemaName, Guid recordId, string message) =>
			PostMessage(senderName, commandName, schemaName, recordId, message, ClassFactory.Get<UserConnection>());

		public void PostMessageToAll(string senderName, string commandName, string schemaName, Guid recordId, string message){
			string msgText = new Dto.WebSocket(commandName, schemaName, recordId, message).ToString();
			IMsg msg = CreateMessage(senderName, msgText);
			_msgChannelManager.PostToAll(msg);
		}

		#endregion

	}
}