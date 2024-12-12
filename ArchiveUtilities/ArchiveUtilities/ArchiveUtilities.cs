using System.Buffers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Abstractions;
using System.IO.Compression;
using System.Security;
using System.Text;

namespace ArchiveUtilities;

public interface IArchiveUtilities {

	#region Methods: Public

	/// <summary>
	///  Asynchronously extracts the contents of a ZIP archive to a specified directory with security and validation checks.
	/// </summary>
	/// <param name="sourceArchiveFileName">The full path to the source ZIP archive file.</param>
	/// <param name="destinationDirectoryName">The directory where the contents should be extracted.</param>
	/// <param name="overwrite">When true, existing files will be overwritten; when false, existing files will be skipped.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when sourceArchiveFileName or destinationDirectoryName is null or empty.</exception>
	/// <exception cref="FileNotFoundException">Thrown when the source archive file does not exist.</exception>
	/// <exception cref="SecurityException">Thrown when a potential directory traversal attack is detected.</exception>
	/// <exception cref="IOException">Thrown when file operations fail or access is denied.</exception>
	/// <exception cref="InvalidDataException">Thrown when the ZIP archive is invalid or corrupted.</exception>
	public Task UnpackZipFileAsync(string sourceArchiveFileName, string destinationDirectoryName, bool overwrite);
	
	Task UnpackGzFile(string sourceArchiveFileName, string destinationDirectoryName, bool overwrite=true);

	Task CompressDirectoryToGzFile(string sourceDirectory, string destinationFileName, bool overwrite=true);
	
	#endregion

}

/// <summary>
///  Provides secure and robust functionality for working with archive files, particularly ZIP archives.
/// </summary>
/// <remarks>
///  This implementation includes security measures against directory traversal attacks,
///  proper error handling, and preservation of file metadata.
/// </remarks>
public class ArchiveUtilities(IFileSystem fileSystem) : IArchiveUtilities {

	#region Constructors: Public

	/// <summary>
	///  Initializes a new instance of the ArchiveUtilities class with the default file system implementation.
	/// </summary>
	public ArchiveUtilities()
		: this(new FileSystem()){ }

	#endregion

	#region Methods: Public

	/// <summary>
	///  Asynchronously extracts the contents of a ZIP archive to a specified directory with comprehensive security and
	///  validation checks.
	/// </summary>
	/// <param name="sourceArchiveFileName">The full path to the source ZIP archive file.</param>
	/// <param name="destinationDirectoryName">The directory where the contents should be extracted.</param>
	/// <param name="overwrite">When true, existing files will be overwritten; when false, existing files will be skipped.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when sourceArchiveFileName or destinationDirectoryName is null or empty.</exception>
	/// <exception cref="FileNotFoundException">Thrown when the source archive file does not exist.</exception>
	/// <exception cref="SecurityException">Thrown when a potential directory traversal attack is detected.</exception>
	/// <exception cref="IOException">Thrown when file operations fail or access is denied.</exception>
	/// <exception cref="InvalidDataException">Thrown when the ZIP archive is invalid or corrupted.</exception>
	/// <remarks>
	///  <para>This method implements several security and robustness features:</para>
	///  <list type="bullet">
	///   <item>
	///    <description>
	///     Security Features:
	///     <list type="bullet">
	///      <item>
	///       <description>Protection against directory traversal attacks</description>
	///      </item>
	///      <item>
	///       <description>Path normalization and validation</description>
	///      </item>
	///      <item>
	///       <description>Secure handling of file operations</description>
	///      </item>
	///     </list>
	///    </description>
	///   </item>
	///   <item>
	///    <description>
	///     Data Integrity:
	///     <list type="bullet">
	///      <item>
	///       <description>Preserves original file timestamps</description>
	///      </item>
	///      <item>
	///       <description>Proper handling of directory structures</description>
	///      </item>
	///      <item>
	///       <description>Validation of archive integrity</description>
	///      </item>
	///     </list>
	///    </description>
	///   </item>
	///   <item>
	///    <description>
	///     Error Handling:
	///     <list type="bullet">
	///      <item>
	///       <description>Detailed error messages with context</description>
	///      </item>
	///      <item>
	///       <description>Proper exception hierarchy</description>
	///      </item>
	///      <item>
	///       <description>Resource cleanup on failure</description>
	///      </item>
	///     </list>
	///    </description>
	///   </item>
	///  </list>
	/// </remarks>
	/// <example>
	///  This example shows how to safely extract a ZIP archive:
	///  <code>
	/// try
	/// {
	///     var archiveUtils = new ArchiveUtilities();
	///     await archiveUtils.UnpackZipFileAsync(
	///         sourceArchiveFileName: @"C:\Downloads\archive.zip",
	///         destinationDirectoryName: @"C:\Extracted",
	///         overwrite: true
	///     );
	/// }
	/// catch (SecurityException ex)
	/// {
	///     // Handle potential security violations
	///     Console.WriteLine($"Security error: {ex.Message}");
	/// }
	/// catch (IOException ex)
	/// {
	///     // Handle I/O errors
	///     Console.WriteLine($"I/O error: {ex.Message}");
	/// }
	/// </code>
	/// </example>
	public async Task UnpackZipFileAsync(string sourceArchiveFileName, string destinationDirectoryName, bool overwrite){
		// Input validation
		ArgumentException.ThrowIfNullOrWhiteSpace(sourceArchiveFileName, nameof(sourceArchiveFileName));
		ArgumentException.ThrowIfNullOrWhiteSpace(destinationDirectoryName, nameof(destinationDirectoryName));

		// Ensure source file exists
		if (!fileSystem.File.Exists(sourceArchiveFileName)) {
			throw new FileNotFoundException("The specified archive file was not found.", sourceArchiveFileName);
		}

		// Normalize paths to prevent directory traversal attacks
		destinationDirectoryName = Path.GetFullPath(destinationDirectoryName);

		try {
			await using FileSystemStream fileStream = fileSystem.File.OpenRead(sourceArchiveFileName);
			using ZipArchive zipArchive = new(fileStream, ZipArchiveMode.Read);
			ReadOnlyCollection<ZipArchiveEntry> entries = zipArchive.Entries;

			// Create destination directory if it doesn't exist
			if (!fileSystem.Directory.Exists(destinationDirectoryName)) {
				fileSystem.Directory.CreateDirectory(destinationDirectoryName);
			}

			foreach (ZipArchiveEntry entry in entries) {
				string destinationFileName = Path.GetFullPath(Path.Combine(destinationDirectoryName, entry.FullName));

				// Security check: Ensure the final path is within the destination directory
				if (!destinationFileName.StartsWith(destinationDirectoryName)) {
					throw new SecurityException($"Potential directory traversal detected for entry: {entry.FullName}");
				}

				if (entry.FullName.EndsWith('/')) {
					fileSystem.Directory.CreateDirectory(destinationFileName);
					continue;
				}

				// Create directory structure for file if it doesn't exist
				string? directoryName = Path.GetDirectoryName(destinationFileName);
				if (directoryName != null && !fileSystem.Directory.Exists(directoryName)) {
					fileSystem.Directory.CreateDirectory(directoryName);
				}

				// Check if file exists and handle overwrite parameter
				if (fileSystem.File.Exists(destinationFileName)) {
					if (!overwrite) {
						continue; // Skip this file if overwrite is false
					}
					fileSystem.File.Delete(destinationFileName); // Delete existing file if overwrite is true
				}

				// Extract file with proper error handling
				try {
					await using Stream entryStream = entry.Open();
					await using FileSystemStream destinationFileStream = fileSystem.File.Create(destinationFileName);
					await entryStream.CopyToAsync(destinationFileStream);
					destinationFileStream.Close();
					
					// Preserve the original timestamp
					fileSystem.File.SetLastWriteTime(destinationFileName, entry.LastWriteTime.DateTime);
					
				} catch (IOException ex) {
					throw new IOException($"Failed to extract entry '{entry.FullName}': {ex.Message}", ex);
				}
			}
		} catch (InvalidDataException ex) {
			throw new InvalidDataException("The ZIP archive appears to be corrupted or invalid.", ex);
		} catch (Exception ex) when (ex is not SecurityException && ex is not ArgumentNullException) {
			throw new IOException($"Failed to unpack archive '{sourceArchiveFileName}': {ex.Message}", ex);
		}
	}
	
	public async Task UnpackGzFile(string sourceArchiveFileName, string destinationDirectoryName, bool overwrite=true){
		// Input validation
		ArgumentException.ThrowIfNullOrWhiteSpace(sourceArchiveFileName, nameof(sourceArchiveFileName));
		ArgumentException.ThrowIfNullOrWhiteSpace(destinationDirectoryName, nameof(destinationDirectoryName));
		
		if(!fileSystem.Directory.Exists(destinationDirectoryName)){
			fileSystem.Directory.CreateDirectory(destinationDirectoryName);
		}
		
		await using FileSystemStream fileStream = fileSystem.File.OpenRead(sourceArchiveFileName);
		await using GZipStream zipStream = new (fileStream, CompressionMode.Decompress, true);
		MemoryStream newStream = new ();
		await zipStream.CopyToAsync(newStream); //Unpacking happens here
		fileStream.Close();
		newStream.Seek(0, SeekOrigin.Begin);
		
		while (newStream.Position != newStream.Length) {
			string filename = await GetFileNameFromStreamAsync(newStream);
			string subDir = Path.Combine(destinationDirectoryName, Path.GetDirectoryName(filename) ?? string.Empty);
			int contentLength = await GetContentLength(newStream);
			byte[] contentBuffer = ArrayPool<byte>.Shared.Rent(contentLength);
			await ReadNBytesAsync(newStream, contentLength, contentBuffer);
			if(!fileSystem.Directory.Exists(subDir)){
				fileSystem.Directory.CreateDirectory(subDir);
			}
			
			string destFileName = Path.Combine(destinationDirectoryName, filename);
			FileSystemStream fs = fileSystem.File.Create(destFileName);
			await fs.WriteAsync(contentBuffer, 0, contentLength, CancellationToken.None);
			await fs.FlushAsync(CancellationToken.None);
			fs.Close();
		}
	}
	
	public async Task CompressDirectoryToGzFile(string sourceDirectory, string destinationFileName, bool overwrite=true){
		// Input validation
		ArgumentException.ThrowIfNullOrWhiteSpace(sourceDirectory, nameof(sourceDirectory));
		ArgumentException.ThrowIfNullOrWhiteSpace(destinationFileName, nameof(destinationFileName));
		
		await using FileSystemStream fileStream = fileSystem.File.Create(destinationFileName);
		await using GZipStream zipStream = new (fileStream, CompressionMode.Compress, false);
		
		string[] allFiles = fileSystem.Directory.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories);
		foreach (string file in allFiles) {
			string relativeFilePath  = Path.GetRelativePath(sourceDirectory, file);
			int fileNameLength = relativeFilePath.Length;
			byte[] fileNameLengthInBytes = BitConverter.GetBytes(fileNameLength);
			byte[] fileNameInBytes = Encoding.Unicode.GetBytes(relativeFilePath); 
			byte[] content = await fileSystem.File.ReadAllBytesAsync(file, CancellationToken.None);
			byte[] contentLength = BitConverter.GetBytes(content.Length);
			var array = fileNameLengthInBytes
				.Concat(fileNameInBytes)
				.Concat(contentLength)
				.Concat(content).ToArray();
			await zipStream.WriteAsync(array, 0, array.Length);
		}
		await zipStream.FlushAsync();
		zipStream.Close();
	}
	
	private async Task<string> GetFileNameFromStreamAsync(MemoryStream stream){
		byte[] nameLengthBytes = ArrayPool<byte>.Shared.Rent(sizeof(int));
		await ReadNBytesAsync(stream, sizeof(int), nameLengthBytes);
		int fileNameLength = BitConverter.ToInt32(nameLengthBytes, 0);
		ArrayPool<byte>.Shared.Return(nameLengthBytes, true);
		
		
		var fileNameBuffer = ArrayPool<byte>.Shared.Rent(fileNameLength * sizeof(char));
		await ReadNBytesAsync(stream, fileNameLength * sizeof(char), fileNameBuffer);
		
		StringBuilder sb = new();
		for(int i = 0; i < fileNameLength; i++){
			ReadOnlySpan<byte> span = new(fileNameBuffer, i*2, sizeof(char));
			char character = BitConverter.ToChar(span);
			sb.Append(character);
		}
		ArrayPool<byte>.Shared.Return(fileNameBuffer, true);
		return sb.ToString();
	}
	private async Task<int> GetContentLength(Stream stream){
		byte[] sizeLengthBytes = ArrayPool<byte>.Shared.Rent(sizeof(int));
		await ReadNBytesAsync(stream, sizeof(int), sizeLengthBytes);
		int contentLength = BitConverter.ToInt32(sizeLengthBytes, 0);
		ArrayPool<byte>.Shared.Return(sizeLengthBytes, true);
		return contentLength;
	}
	
	private static async Task ReadNBytesAsync(Stream stream, int bytesToRead, byte[] buffer){
		int totalRead = 0;
		do {
			int read = await stream.ReadAsync(buffer, totalRead, bytesToRead - totalRead, CancellationToken.None);
			totalRead += read;
		}
		while(totalRead < bytesToRead);
	}
	#endregion

}