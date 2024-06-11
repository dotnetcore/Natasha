using Natasha.CSharp.Compiler.Component;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

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
            _mainWatcher = new FileSystemWatcher
            {
                Path = VSCSharpFolder.MainCsprojPath,
                Filter = "*.cs",
                EnableRaisingEvents = true,
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
#if DEBUG
       Console.WriteLine($"Created: {e.FullPath}");
#endif
               
                if (CheckFileAvailiable(e.FullPath))
                {
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
#if DEBUG
       Console.WriteLine($"Deleted: {e.FullPath}");
#endif
                if (CheckFileAvailiable(e.FullPath))
                {
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
#if DEBUG
        Console.WriteLine($"Renamed: {e.OldFullPath} -> {e.FullPath}");
#endif
                if (e.OldFullPath.EndsWith(".cs"))
                {
                    if (e.FullPath.EndsWith(".cs"))
                    {
                        _compileLock.GetAndWaitLock();
                        if (CheckFileAvailiable(e.FullPath))
                        {
                            CreateFileAction(e.FullPath);
                        }
                        if (CheckFileAvailiable(e.OldFullPath))
                        {
                            DeleteFileAction(e.OldFullPath);
                        }
                        _compileLock.ReleaseLock();
                        await ExecuteAfterFunction();
                    }
                    else if (e.FullPath.StartsWith(e.OldFullPath) && e.FullPath.EndsWith(".TMP"))
                    {
                       
                        if (CheckFileAvailiable(e.OldFullPath))
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
            if (_compileLock.GetLock())
            {
                await AfterFunction();
                _compileLock.ReleaseLock();
            }
#if DEBUG
        else{
            Console.WriteLine($"争抢编译，做出让步！");
        }
#endif
        }
        public bool CheckFileAvailiable(string file)
        {
            if (file.StartsWith(VSCSharpFolder.ObjPath) || file.StartsWith(VSCSharpFolder.BinPath))
            {
                return false;
            }
            return true;
        }
        private static void Error(object sender, ErrorEventArgs e)
        {
            PrintException(e.GetException());
        }
        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }
    }
}
