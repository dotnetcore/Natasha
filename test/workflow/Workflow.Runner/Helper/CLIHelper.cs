using Github.NET.Sdk;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal static class CLIHelper
{
    public static async ValueTask<bool> Output(string key, string value)
    {
        return await ExecuteBashCommand($"echo \"{key}={value}\" >> $GITHUB_OUTPUT");
    }

    public static async ValueTask<bool> Enviroment(string key, string value)
    {
        return await ExecuteBashCommand($"echo \"{key}={value}\" >> $GITHUB_ENV");

    }

    public static async ValueTask<bool> ExecuteBashCommand(string command)
    {
        using Process process = new Process();
        var info = process.StartInfo;
        info.FileName = "/bin/bash";
        info.WorkingDirectory = SolutionInfo.Root;
        info.CreateNoWindow = true;
        info.UseShellExecute = false;
        info.RedirectStandardOutput = true;
        info.RedirectStandardError = true;
        info.StandardErrorEncoding = Encoding.UTF8;
        info.StandardOutputEncoding = Encoding.UTF8;
        process.StartInfo = info;
        process.OutputDataReceived += (sender, e) =>
        {
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
#if DEBUG
                Console.WriteLine(e.Data);
#endif
            }
        };

        info.Arguments = "-c \"" + command + " \"";
        process.Start();
        process.BeginErrorReadLine();
        process.BeginOutputReadLine();
        await process.WaitForExitAsync();
        return process.ExitCode == 0;
    }
}

