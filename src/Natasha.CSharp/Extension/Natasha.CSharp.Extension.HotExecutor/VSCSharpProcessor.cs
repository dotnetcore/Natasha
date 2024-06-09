using System;
using System.Diagnostics;
using System.Text;

namespace Natasha.CSharp.Extension.HotExecutor
{
    public class VSCSharpProcessor
    {
        private Process? _process;
        private readonly ProcessStartInfo _builder;
        private string _outpuNewAppFolder;
        private string _outputNewExeFile;

        public VSCSharpProcessor()
        {
            _outpuNewAppFolder = string.Empty;
            _outputNewExeFile = string.Empty;
            _builder = new ProcessStartInfo()
            {
                FileName = "dotnet.exe",
                WorkingDirectory = VSCSharpFolder.SlnPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            ResetOutputInfo();
        }

        private void ResetOutputInfo()
        {
            _outpuNewAppFolder = Path.Combine(VSCSharpFolder.BinPath, "T" + Guid.NewGuid().ToString("N"));
            _outputNewExeFile = Path.Combine(_outpuNewAppFolder, VSCSharpFolder.ExecuteName);
            _builder.Arguments = $"build \"{VSCSharpFolder.MainCsprojPath}\" -c {(ProjectDynamicProxy.IsRelease?"Release":"Debug")} --nologo -o \"{_outpuNewAppFolder}\"";
        }


        public ValueTask<bool> BuildProject()
        {
            try
            {
                ResetOutputInfo();
                Encoding utf8 = Encoding.UTF8;
                using var process = new Process();
                process.StartInfo = _builder;
                process.EnableRaisingEvents = true;
                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        Console.WriteLine(utf8.GetString(utf8.GetBytes(e.Data)));
                    }
                };
                process.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        Console.WriteLine(utf8.GetString(utf8.GetBytes(e.Data)));
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
#if DEBUG
                Console.WriteLine($"正在执行 {_builder.FileName} {_builder.Arguments}");
#endif
                process.WaitForExit();
                return new ValueTask<bool>(process.ExitCode == 0);
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"执行出错 {ex.Message}");
#endif
            }
            return new ValueTask<bool>(false);
        }
         
        public ValueTask<bool> Run()
        {
            try
            {
                if (_process != null)
                {
                    _process.Kill();
                    _process.Dispose();
                    
                }

                _process = new Process()
                {
                    StartInfo = new()
                    {
                        FileName = VSCSharpFolder.ExecuteName,
                        WorkingDirectory = _outpuNewAppFolder,
                        CreateNoWindow = true,
                        UseShellExecute = true,
                    }
                };
#if DEBUG
                Console.WriteLine($"执行: {Path.Combine(_outpuNewAppFolder, VSCSharpFolder.ExecuteName)}");
#endif
                _process.Start();
                // 等待一小段时间，让进程有机会启动
                System.Threading.Thread.Sleep(1000);
                Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(_outputNewExeFile));
                if (processes.Length > 0)
                {
                    foreach (var item in processes)
                    {
                        try
                        {
                            if (item.HasExited == false)
                            {
                                return new ValueTask<bool>(true);
                            }
                        }
                        catch
                        { 
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error starting the process: " + e.Message);
            }
            return new ValueTask<bool>(false);
        }
    }
}
