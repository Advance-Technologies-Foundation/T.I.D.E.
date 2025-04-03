namespace AtfTIDE.Logging {

	public enum Color {
		Green,
		Red,
		Yellow
	}

	public interface ILiveLogger {
		void Log(string message);
		void LogInfo(string message);  // New method
		void LogError(string message); // New method
		void LogWarn(string message);  // New method
	}

	public class LiveLogger : ILiveLogger {
		private readonly IWebSocket _webSocket;

		public LiveLogger(IWebSocket webSocket) {
			_webSocket = webSocket;
		}

		public void Log(string message) {
			_webSocket.PostMessageToAll("Clio", "Show logs", message);
		}

		private void Log(string message, Color color) {
			string colorTag;
			switch (color) {
				case Color.Green:
					colorTag = "<span style='color:green'>[INF]</span>";
					break;
				case Color.Red:
					colorTag = "<span style='color:red'>[ERR]</span>";
					break;
				case Color.Yellow:
					colorTag = "<span style='color:yellow'>[WAR]</span>";
					break;
				default:
					colorTag = "[UNK]";
					break;
			}
			string colorizedMessage = $"{colorTag} - {message}";
			_webSocket.PostMessageToAll("Clio", "Show logs", colorizedMessage);
		}

		public void LogInfo(string message) {
			Log(message, Color.Green);
		}

		public void LogError(string message) {
			Log(message, Color.Red);
		}

		public void LogWarn(string message) {
			Log(message, Color.Yellow);
		}
	}
}
