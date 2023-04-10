using System.Diagnostics;
using System.Text;

namespace Github.NET.Sdk
{


    public static class CLIHelper
    {
        public static async Task<(bool, string)> ExecuteBashCommand(string command, string workPath)
        {
            StringBuilder textInfo = new();
            using Process process = new Process();
            var info = process.StartInfo;
            if (OperatingSystem.IsWindows())
            {
                info.FileName = "cmd"; 
            }
            else
            {
                info.FileName = "/bin/bash";
            }

            info.WorkingDirectory = workPath;
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.StandardErrorEncoding = Encoding.UTF8;
            info.StandardOutputEncoding = Encoding.UTF8;
            process.StartInfo = info;
            process.OutputDataReceived += (sender, e) =>
            {
                textInfo.AppendLine(e.Data);
#if DEBUG
            if (!string.IsNullOrEmpty(e.Data))
                {
                    Console.WriteLine(e.Data);
                }
#endif
        };
            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    textInfo.AppendLine(e.Data);
#if DEBUG
                Console.WriteLine(e.Data);
#endif
            }
            };
            if (OperatingSystem.IsWindows())
            {
                info.Arguments = "/c \"" + command + " \"";
            }
            else
            {
                info.Arguments = "-c \"" + command + " \"";
            }
            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
            await process.WaitForExitAsync();
            return (process.ExitCode == 0, textInfo.ToString());
        }
    }
}

