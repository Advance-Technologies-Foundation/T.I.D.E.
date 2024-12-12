using System.Linq;
using ATF.Repository;
using ATF.Repository.Providers;
using MrktApolloApp.CustomException;
using MrktApolloApp.Repository.Models;
using Terrasoft.Core;
using Terrasoft.Core.Factories;

namespace MrktApolloApp.Utils
{
	internal interface ISysSettingsUtil
	{
		object GetSysSettingValueByCode(string code);
	}
	
	
	internal class SysSettingsUtil : ISysSettingsUtil
	{
		private readonly IDataProvider _dataProvider;

		public SysSettingsUtil(IDataProvider dataProvider)
		{
			_dataProvider = dataProvider;
		}


		public object GetSysSettingValueByCode(string code)
		{
			var ctx = AppDataContextFactory.GetAppDataContext(_dataProvider);

			var value = ctx.GetSysSettingValue<object>(code);

			if (value.Value == null) {
				throw new SysSettingsNotFoundException(code);
			}
			
			return value.Value;
			
		}
	}
}