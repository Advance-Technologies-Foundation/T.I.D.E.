using System;
using System.IO;
using ErrorOr;

namespace AtfTIDE.ClioInstaller {
	
	public interface IInstaller {

		ErrorOr<bool> CheckIsClioInstalled();
		ErrorOr<Version> GetClioVersion();
		ErrorOr<Success> UpdateClio();
		ErrorOr<Success> InstallClio();
	}
	
	public class Installer : IInstaller {

		private readonly INugetClient _nugetClient;

		public Installer(INugetClient nugetClient){
			_nugetClient = nugetClient;
			
		}

		public ErrorOr<bool> CheckIsClioInstalled(){
			throw new NotImplementedException();
		}

		public ErrorOr<Version> GetClioVersion(){
			throw new NotImplementedException();
		}

		public ErrorOr<Success> UpdateClio(){
			throw new NotImplementedException();
		}

		public ErrorOr<Success> InstallClio(){
			
			DirectoryInfo clioDir = HelperFunctions.GetClioDirectory();
			return _nugetClient.DownloadClioAsync(clioDir.FullName)
								.GetAwaiter().GetResult();
		}

	}
}