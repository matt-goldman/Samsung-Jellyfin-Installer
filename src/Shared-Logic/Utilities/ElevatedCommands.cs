// TODO: Technical Debt - ElevatedCommands has direct UI dependencies
// This utility class violates separation of concerns by:
// 1. Taking InstallingWindow as a parameter (WPF View dependency)
// 2. Directly calling UI methods like SetStatusText()
// 
// Actions needed:
// 1. Create an abstraction like IStatusReporter interface
// 2. Remove direct dependency on WPF views
// 3. Use dependency injection or callback patterns for status updates
// 4. Move to shared utilities after removing UI dependencies

using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace Samsung_Jellyfin_Installer.Shared.Utilities
{
    public interface IStatusReporter
    {
        void SetStatusText(string message);
    }

    public class ElevatedCommands
    {
        public static async Task<string?> RunElevatedAndCaptureOutputAsync(
            string filePath,
            string arguments,
            string workingDir,
            IStatusReporter statusReporter,
            string actionDescription)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("filePath cannot be null or empty", nameof(filePath));
            }

            if (string.IsNullOrEmpty(workingDir))
            {
                workingDir = Environment.CurrentDirectory;
            }

            var tempFile = Path.Combine(Path.GetTempPath(), $"tizen_ext_{Guid.NewGuid():N}.txt");

            // Safely encode message for echo

            // Construct full command: show message, then run tool, capture output to temp file
            var fullCommand = $"echo  === Checking Tizen Packages activation status === && \"{filePath}\" {arguments} > \"{tempFile}\" 2>&1";

            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c {fullCommand}",
                WorkingDirectory = workingDir,
                UseShellExecute = true,
                Verb = "runas",
                CreateNoWindow = false // Set to false so the user sees the echo
            };

            try
            {
                using var process = Process.Start(startInfo);
                if (process == null)
                {
                    statusReporter.SetStatusText($"Failed to start elevated process: {actionDescription}");
                    return null;
                }

                await process.WaitForExitAsync();

                if (File.Exists(tempFile))
                {
                    var output = await File.ReadAllTextAsync(tempFile);
                    File.Delete(tempFile);
                    return output;
                }

                statusReporter.SetStatusText("Output file not found after elevation.");
                return null;
            }
            catch (Win32Exception ex) when (ex.NativeErrorCode == 1223)
            {
                statusReporter.SetStatusText("Operation cancelled by user.");
                return null;
            }
            catch (Exception ex)
            {
                statusReporter.SetStatusText($"Error during elevated run: {ex.Message}");
                return null;
            }
        }
    }
}
