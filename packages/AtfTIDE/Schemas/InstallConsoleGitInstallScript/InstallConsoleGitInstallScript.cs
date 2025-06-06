 using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.IO.Compression;
using AtfTIDE;
using Common.Logging;
using ErrorOr;

namespace Terrasoft.Configuration
{
	using Terrasoft.Core;
	public class InstallConsoleGitInstallScript : IInstallScriptExecutor {
		public void Execute(UserConnection userConnection) {
			
			var logger = LogManager.GetLogger("TIDE");
			
			var archiveZipPath = HelperFunctions.GetArchivePath();
			var destFolder = HelperFunctions.GetGitConsoleFolderPath();
			logger.Info($"Unzipping archive from {archiveZipPath} to {destFolder}");
			
			DeleteDirectoryRecursively(new DirectoryInfo(destFolder));
			logger.Info($"Deleted existing directory: {destFolder}");
			
			if(!Directory.Exists(destFolder)) {
				Directory.CreateDirectory(destFolder);
			}
			
			string destArchivePath = Path.Combine(destFolder, "archive.zip");
			System.IO.File.Copy(archiveZipPath, destArchivePath, true);
			UnzipArchive(destArchivePath, destFolder);
			logger.Info($"Unzipped archive to {destFolder}");
			System.IO.File.Delete(destArchivePath);
			logger.Info($"Deleted temporary archive file: {destArchivePath}");
		}	
		
		private static void DeleteDirectoryRecursively(DirectoryInfo directory) {
			if (!directory.Exists)
				return;

			// Delete all files
			foreach (FileInfo file in directory.GetFiles()) {
				file.Attributes = System.IO.FileAttributes.Normal;
				file.Delete();
			}

			// Recursively delete all subdirectories
			foreach (var subDirectory in directory.GetDirectories()) {
				DeleteDirectoryRecursively(subDirectory);
			}

			// Delete the empty directory
			directory.Delete(false);
		}
		
		
		private static void UnzipArchive(string archivePath, string destinationPath) {
			using (Stream archiveStream = new FileStream(archivePath, FileMode.Open)) {
				using (ZipArchive arch = new ZipArchive(archiveStream, ZipArchiveMode.Read, false)) {
					foreach (ZipArchiveEntry entry in arch.Entries) {
						string fullName = entry.FullName;
						long length = entry.Length;
						string destFilePath = Path.Combine(destinationPath, fullName);
						string dir = Path.GetDirectoryName(destFilePath);
						if (dir != null && !Directory.Exists(dir)) {
							Directory.CreateDirectory(dir);
						}
						if (length > 0) {
							//otherwise it is an empty file
							using (Stream stream = entry.Open()) {
								FileSystem fs = new FileSystem();
								using (FileSystemStream fileStream = fs.File.Create(destFilePath)) {
									stream.CopyTo(fileStream, (int)length);
									fileStream.Flush();
									fileStream.Close();
								}
							}
						}
					}
				}
			}
		}
	}
}