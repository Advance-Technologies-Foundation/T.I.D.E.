using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using FluentAssertions;

namespace ArchiveUtilities.Tests;

[TestFixture]
public class ArchiveUtilitiesTests {

	#region Setup/Teardown

	[SetUp]
	public void Setup(){
		_fileSystemMock = new MockFileSystem();
		_driveDataMock = new MockDriveData();
		_fileSystemMock.AddDrive(DriveName, _driveDataMock);
	}

	#endregion

	#region Constants: Private

	private const string DriveName = "T";

	#endregion

	#region Fields: Private

	private MockFileSystem _fileSystemMock;
	private ArchiveUtilities _sut;
	private MockDriveData _driveDataMock;

	#endregion

	#region Methods: Private

	private async Task AssertOn1(string destinationDirectoryName){
		_fileSystemMock.Directory.Exists(destinationDirectoryName).Should().BeTrue();
		_fileSystemMock.Directory.GetFiles(destinationDirectoryName).Should()
			.Contain(Path.Combine(destinationDirectoryName, "1.txt"));
		string content = await _fileSystemMock.File.ReadAllTextAsync(Path.Combine(destinationDirectoryName, "1.txt"));
		content.Should().Be("1");
	}

	private async Task AssertOn2(string destinationDirectoryName){
		_fileSystemMock.Directory.Exists(destinationDirectoryName).Should().BeTrue();

		_fileSystemMock.Directory.GetFiles(destinationDirectoryName).Should()
			.Contain(Path.Combine(destinationDirectoryName, "1.txt"));
		string content1 = await _fileSystemMock.File.ReadAllTextAsync(Path.Combine(destinationDirectoryName, "1.txt"));
		content1.Should().Be("1.txt");

		_fileSystemMock.Directory.GetFiles(destinationDirectoryName).Should()
			.Contain(Path.Combine(destinationDirectoryName, "2.txt"));
		string content2 = await _fileSystemMock.File.ReadAllTextAsync(Path.Combine(destinationDirectoryName, "2.txt"));
		content2.Should().Be("2.txt");

		_fileSystemMock.Directory.GetDirectories(destinationDirectoryName).Should()
			.Contain(Path.Combine(destinationDirectoryName, "SubDir"));
		string content3
			= await _fileSystemMock.File.ReadAllTextAsync(Path.Combine(destinationDirectoryName, "SubDir", "3.txt"));
		content3.Should().Be("3.txt");
	}

	#endregion

	[TestCase("ZipFiles\\1.zip", "ResultDir")]
	[TestCase("ZipFiles\\2.zip", "ResultDir")]
	public async Task UnpackZipFile(string zipFilePath, string destinationDirectoryName){
		//Arrange
		byte[] bytes = await File.ReadAllBytesAsync(zipFilePath, CancellationToken.None);
		string fullDirName = Path.Combine($"{DriveName}:", Path.GetDirectoryName(zipFilePath)!);

		if (!_fileSystemMock.Directory.Exists(fullDirName)) {
			_fileSystemMock.Directory.CreateDirectory(fullDirName);
		}

		string zipFilePathInMockFileSystem = Path.Combine($"{DriveName}:", zipFilePath);
		FileSystemStream file = _fileSystemMock.File.Create(zipFilePathInMockFileSystem);
		await file.WriteAsync(bytes, 0, bytes.Length, CancellationToken.None);
		await file.FlushAsync();

		destinationDirectoryName = Path.Combine($"{DriveName}:", destinationDirectoryName);
		_sut = new ArchiveUtilities(_fileSystemMock);
		//Act

		await _sut.UnpackZipFileAsync(zipFilePathInMockFileSystem, destinationDirectoryName, true);

		//Assert
		if (zipFilePath.Contains("1.zip")) {
			await AssertOn1(destinationDirectoryName);
		} else if (zipFilePath.Contains("2.zip")) {
			await AssertOn2(destinationDirectoryName);
		}
	}

	
	[TestCase("ZipFiles\\MrktApolloApp.gz", "ResultDir")]
	public async Task UnpackGz(string gzFilePath, string destinationDirectoryName){
		// Arrange
		byte[] bytes = await File.ReadAllBytesAsync(gzFilePath, CancellationToken.None);
		string fullDirName = Path.Combine($"{DriveName}:", Path.GetDirectoryName(gzFilePath)!);

		if (!_fileSystemMock.Directory.Exists(fullDirName)) {
			_fileSystemMock.Directory.CreateDirectory(fullDirName);
		}
		string gzFilePathInMockFileSystem = Path.Combine($"{DriveName}:", gzFilePath);
		FileSystemStream file = _fileSystemMock.File.Create(gzFilePathInMockFileSystem);
		await file.WriteAsync(bytes, 0, bytes.Length, CancellationToken.None);
		await file.FlushAsync();
		
		_sut = new ArchiveUtilities(_fileSystemMock);
		destinationDirectoryName = Path.Combine($"{DriveName}:", destinationDirectoryName);
		
		// Act
		await _sut.UnpackGzFile(gzFilePathInMockFileSystem, destinationDirectoryName);
		
		//Assert
		
		var realFiles = Directory.GetFiles("ZipFiles\\MrktApolloAppFolder","*.*", SearchOption.AllDirectories);

		foreach (string realFile in realFiles) {
			string fileInMockFileSystem = realFile.Replace("ZipFiles\\MrktApolloAppFolder", destinationDirectoryName);;
			
			var realContent = await File.ReadAllBytesAsync(realFile, CancellationToken.None);
			var mockContent = await _fileSystemMock.File.ReadAllBytesAsync(fileInMockFileSystem, cancellationToken: CancellationToken.None);
			mockContent.Should().BeEquivalentTo(realContent);
		}
	}
	
}