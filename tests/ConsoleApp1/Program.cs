using System.Diagnostics;
using System.Text;

namespace ConsoleApp1;

class Program {

	static void Main(string[] args){
		try {
			string baseDir = AppDomain.CurrentDomain.BaseDirectory;

			// Validate executable path exists
			// if (!System.IO.File.Exists(FileName)) {
			// 	SetError("Executable not found");
			// 	return true;
			// }
				
			ProcessStartInfo startInfo = new ProcessStartInfo {
				FileName = "clio",
				Arguments = "pushw -u http://k_krylov_nb.tscrm.com:40033/ -l Supervisor -p Supervisor",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				CreateNoWindow = false,
				WorkingDirectory = @"C:\inetpub\wwwroot\clio\avidia\Terrasoft.WebApp\conf\tide\LabKirill"
			};

			using (System.Diagnostics.Process process = new System.Diagnostics.Process()) {
				process.StartInfo = startInfo;
				StringBuilder errorBuilder = new StringBuilder();
				process.ErrorDataReceived += (sender, e) => {
					if (e.Data != null) {
						errorBuilder.AppendLine(e.Data);
					}
				};
				
				process.OutputDataReceived += (sender, e) => {
					if (e.Data != null) {
						Console.Out.WriteLine(e.Data);
					}
				};
				
				
				
				process.Start();
				process.BeginErrorReadLine();
				process.BeginOutputReadLine();
				process.WaitForExit();
				if (process.ExitCode == 0) {
					return;
				}
				string error = errorBuilder.ToString();
				Console.WriteLine($"ProcessError failed with error code {process.ExitCode} {error}");
					
			}
		} catch (Exception ex) {
			Console.WriteLine(ex.Message);
		}
	}

}