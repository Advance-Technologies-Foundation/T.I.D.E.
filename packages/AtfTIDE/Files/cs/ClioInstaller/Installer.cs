using System;
using System.IO;
using System.IO.Abstractions;
using ErrorOr;

namespace AtfTIDE.ClioInstaller {
	
	public interface IInstaller {

		IErrorOr<bool> CheckIsClioInstalled();
		IErrorOr<Version> GetClioVersion();
		IErrorOr<Success> UpdateClio();
		IErrorOr<Success> InstallClio();
	}
	
	public class Installer : IInstaller {

		private readonly INugetClient _nugetClient;
		private readonly IFileSystem _fileSystem;

		public Installer(INugetClient nugetClient, IFileSystem fileSystem){
			_nugetClient = nugetClient;
			_fileSystem = fileSystem;
		}

		public IErrorOr<bool> CheckIsClioInstalled(){
			throw new NotImplementedException();
		}

		public IErrorOr<Version> GetClioVersion(){
			throw new NotImplementedException();
		}

		public IErrorOr<Success> UpdateClio(){
			throw new NotImplementedException();
		}

		public IErrorOr<Success> InstallClio(){
			DirectoryInfo clioDir = HelperFunctions.GetClioDirectory();
			return _nugetClient.DownloadClioAsync(clioDir.FullName)
								.GetAwaiter().GetResult();
		}

	}
}