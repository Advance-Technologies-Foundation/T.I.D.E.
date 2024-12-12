using System;

namespace MrktApolloApp.CustomException
{
	public class SysSettingsNotFoundException : Exception
	{
		public SysSettingsNotFoundException(string code) : base($"System settings with code {code} not found.")
		{
		}
	}
}