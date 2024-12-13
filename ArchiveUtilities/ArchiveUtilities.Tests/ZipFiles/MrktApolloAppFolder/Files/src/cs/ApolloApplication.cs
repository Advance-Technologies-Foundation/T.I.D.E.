namespace MrktApolloApp
{
	public static class ApolloApplication
	{

		#region Fields: Private

		private static IApolloApp _apolloApp;

		#endregion

		#region Properties: Public

		/// <summary>
		/// Application instance.
		/// </summary>
		public static IApolloApp Instance => _apolloApp ?? (_apolloApp = new ApolloApp());

		#endregion

		#region Methods: Public

		/// <summary>
		/// Restarts application instance.
		/// </summary>
		/// <returns>New instance of the application</returns>
		public static IApolloApp Restart(){
			if(Instance != null) {
				_apolloApp.Dispose();
				_apolloApp = null;
			}
			_apolloApp = new ApolloApp();
			return _apolloApp;
		}
		
		/// <summary>
		/// This is required for unit tests.
		/// </summary>
		/// <param name="instance"></param>
		internal static void SetInstance(IApolloApp instance) {
			_apolloApp = instance;
		}
		
		#endregion

	}
}