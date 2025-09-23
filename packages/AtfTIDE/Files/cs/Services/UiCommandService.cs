using System.Dynamic;

namespace AtfTIDE.Services{

	public interface IUiCommandService{
		void Invoke(string commandName, string message);
		void Invoke(string commandName, object message);
	}
	
	public class UiCommandService : IUiCommandService{
		private readonly IWebSocket _webSocket;

		public UiCommandService(IWebSocket webSocket) {
			_webSocket = webSocket;
		}

		public void Invoke(string commandName, string message) {
			_webSocket.PostMessageToAll(TideConsts.SenderName,commandName,message);
		}

		public void Invoke(string commandName, object message) {
			string messageStr  = System.Text.Json.JsonSerializer.Serialize(message);
			Invoke(commandName, messageStr);
		}
	}

}
