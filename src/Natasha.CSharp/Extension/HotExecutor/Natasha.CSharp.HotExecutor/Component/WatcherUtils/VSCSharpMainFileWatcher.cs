using Natasha.CSharp.Compiler.Component;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Transactions;

namespace Natasha.CSharp.Extension.HotExecutor
{
    internal class VSCSharpMainFileWatcher
    {
        private FileSystemWatcher _mainWatcher;
        public Func<bool> PreFunction;
        public Func<Task> AfterFunction;
        public Action<string> DeleteFileAction;
        public Action<string> CreateFileAction;
        public Action<string> ChangeFileAction;
        private readonly HESpinLock _compileLock;
        public VSCSharpMainFileWatcher()
        {
            _mainWatcher = new();
            _compileLock = new();
            PreFunction = () => false;
            AfterFunction = () => Task.CompletedTask;
            DeleteFileAction = (str) => { };
            CreateFileAction = (str) => { };
            ChangeFileAction = (str) => { };
        }
        public void StartMonitor()
        {
            _mainWatcher.EnableRaisingEvents = true;
        }
        public void DeployMonitor()
        {
            _mainWatcher = new FileSystemWatcher
            {
                Path = VSCSharpProjectInfomation.MainCsprojPath,
                Filter = "*",
                EnableRaisingEvents = false,
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime | NotifyFilters.LastWrite
            };

            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                _mainWatcher.Dispose();
            };

            _mainWatcher.Created += async (sender, e) =>
            {
                if (PreFunction())
                {
                    return;
                }
                if (VSCSharpProjectInfomation.IsXamlAndResxFile(e.FullPath))
                {
                    await ExecuteAfterFunction();

                }
                else if (VSCSharpProjectInfomation.CheckFileAvailiable(e.FullPath))
                {
#if DEBUG
                    HEProxy.ShowMessage($"Created: {e.FullPath}");
#endif

                    _compileLock.GetAndWaitLock();
                    CreateFileAction(e.FullPath);
                    _compileLock.ReleaseLock();

                    await ExecuteAfterFunction();
                }

            };

            _mainWatcher.Deleted += async (sender, e) =>
            {
                if (PreFunction())
                {
                    return;
                }
                if (VSCSharpProjectInfomation.IsXamlAndResxFile(e.FullPath))
                {
                    await ExecuteAfterFunction();

                }
                else if (VSCSharpProjectInfomation.CheckFileAvailiable(e.FullPath))
                {
#if DEBUG
                    HEProxy.ShowMessage($"Deleted: {e.FullPath}");
#endif
                    _compileLock.GetAndWaitLock();
                    DeleteFileAction(e.FullPath);
                    _compileLock.ReleaseLock();
                    await ExecuteAfterFunction();
                }

            };

            _mainWatcher.Renamed += async (sender, e) =>
            {
                if (PreFunction())
                {
                    return;
                }
                if (VSCSharpProjectInfomation.IsXamlAndResxFile(e.OldFullPath) || VSCSharpProjectInfomation.IsXamlAndResxFile(e.FullPath))
                {
                    await ExecuteAfterFunction();

                }else if (e.OldFullPath.EndsWith(".cs"))
                {
                    if (e.FullPath.EndsWith(".cs"))
                    {
#if DEBUG
                        HEProxy.ShowMessage($"Renamed: {e.OldFullPath} -> {e.FullPath}");
#endif
                        _compileLock.GetAndWaitLock();
                        if (VSCSharpProjectInfomation.CheckFileAvailiable(e.FullPath))
                        {
                            CreateFileAction(e.FullPath);
                        }
                        if (VSCSharpProjectInfomation.CheckFileAvailiable(e.OldFullPath))
                        {
                            DeleteFileAction(e.OldFullPath);
                        }
                        _compileLock.ReleaseLock();
                        await ExecuteAfterFunction();
                    }
                    else if (e.FullPath.StartsWith(e.OldFullPath) && e.FullPath.EndsWith(".TMP"))
                    {
#if DEBUG
                        HEProxy.ShowMessage($"Renamed: {e.OldFullPath} -> {e.FullPath}");
#endif
                        if (VSCSharpProjectInfomation.CheckFileAvailiable(e.OldFullPath))
                        {
                            _compileLock.GetAndWaitLock();
                            ChangeFileAction(e.OldFullPath);
                            _compileLock.ReleaseLock();
                            await ExecuteAfterFunction();
                        }
                    }

                }
            };
            _mainWatcher.Error += Error;
        }
        private async Task ExecuteAfterFunction()
        {
            HEProxy.ShowMessage($"准备执行热编译...");
            try
            {
                if (_compileLock.GetLock())
                {
                    await AfterFunction();
                    _compileLock.ReleaseLock();
                }
#if DEBUG
                else
                {
                    HEProxy.ShowMessage($"争抢编译，做出让步！");
                }
#endif
            }
            catch
            {


            }
        }

        private static void Error(object sender, ErrorEventArgs e)
        {
            PrintException(e.GetException());
        }
        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                HEProxy.ShowMessage($"Message: {ex.Message}");
                HEProxy.ShowMessage("Stacktrace:");
                HEProxy.ShowMessage(ex.StackTrace);
                HEProxy.ShowMessage("");
                PrintException(ex.InnerException);
            }
        }
    }
}
