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
            _process = Process.GetCurrentProcess();
            _outpuNewAppFolder = string.Empty;
            _outputNewExeFile = string.Empty;
            _builder = new ProcessStartInfo()
            {
                FileName = "dotnet.exe",
                WorkingDirectory = VSCSharpProjectInfomation.SlnPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            _builder.StandardOutputEncoding = Encoding.UTF8;
            _builder.StandardErrorEncoding = Encoding.UTF8;
            ResetOutputInfo();
        }



        private void ResetOutputInfo()
        {
            _outpuNewAppFolder = Path.Combine(VSCSharpProjectInfomation.BinPath, "T" + Guid.NewGuid().ToString("N"));
            _outputNewExeFile = Path.Combine(_outpuNewAppFolder, VSCSharpProjectInfomation.ExecuteName);
            _builder.Arguments = $"build \"{VSCSharpProjectInfomation.MainCsprojPath}\" -c {(HEProxy.IsRelease?"Release":"Debug")} --nologo -o \"{_outpuNewAppFolder}\"";
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
                        HEProxy.ShowMessage(utf8.GetString(utf8.GetBytes(e.Data)));
                    }
                };
                process.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        HEProxy.ShowMessage(utf8.GetString(utf8.GetBytes(e.Data)));
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
#if DEBUG
                HEProxy.ShowMessage($"正在执行 {_builder.FileName} {_builder.Arguments}");
#endif
                process.WaitForExit();
                return new ValueTask<bool>(process.ExitCode == 0);
            }
            catch (Exception ex)
            {
#if DEBUG
                HEProxy.ShowMessage($"执行出错 {ex.Message}");
#endif
            }
            return new ValueTask<bool>(false);
        }
         
        public ValueTask<bool> Run()
        {
            try
            {

                var process = new Process()
                {
                    StartInfo = new()
                    {
                        FileName = VSCSharpProjectInfomation.ExecuteName,
                        WorkingDirectory = _outpuNewAppFolder,
                        CreateNoWindow = true,
                        UseShellExecute = true,
                    }
                };
#if DEBUG
                HEProxy.ShowMessage($"执行: {Path.Combine(_outpuNewAppFolder, VSCSharpProjectInfomation.ExecuteName)}");
#endif
                process.Start();
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
                                //释放前一个进程
                                if (_process != null)
                                {
                                    _process.Kill();
                                    _process.Dispose();
                                    _process = process;
                                }
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
                HEProxy.ShowMessage("Error starting the process: " + e.Message);
            }
            return new ValueTask<bool>(false);
        }
    }
}
