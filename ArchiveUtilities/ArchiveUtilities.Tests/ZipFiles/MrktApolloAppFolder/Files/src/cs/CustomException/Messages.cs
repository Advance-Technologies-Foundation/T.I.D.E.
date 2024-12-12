namespace MrktApolloApp.CustomException
{
	public static class Messages
	{
		public static string Forbidden => "Insufficient credits! Upgrade your Apollo.io plan to increase number of credits.";
		public static string Unauthorized => "Your API key appears to be invalid. Please check and ensure that it's accurately entered in the 'Apollo API Key' system setting.";
		public static string InsufficientCredits=> "Insufficient credits! Upgrade your Apollo.io plan to increase number of credits.";
		public static string Unknown=> "Error while getting response";
		public static string ApiKeyMissing=> "Your API key is missing. Please fill in \"Apollo API Key\" system setting with the correct value and try again.";
	}
}