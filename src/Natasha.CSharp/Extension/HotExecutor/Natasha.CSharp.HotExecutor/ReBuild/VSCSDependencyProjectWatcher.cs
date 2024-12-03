using Natasha.CSharp.HotExecutor.Utils;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Xml;

namespace Natasha.CSharp.HotExecutor.Core
{
    internal class VSCSDependencyProjectWatcher
    {
        private Func<Task>? _execute;
        private readonly HashSet<string> _mainExecuteCache;
        private long _timeStamp;
        private bool _needExecuted;
        private readonly ConcurrentDictionary<string, (FileSystemWatcher csprojWatcher, FileSystemWatcher csFileWatcher)> _projctWatcherCache;
        public VSCSDependencyProjectWatcher(string csprojPath)
        {
            _projctWatcherCache = [];
            _mainExecuteCache = [];
            _mainExecuteCache.Add(VSCSProjectInfoHelper.DebugPath);
            _mainExecuteCache.Add(VSCSProjectInfoHelper.ReleasePath);
            _mainExecuteCache.Add(VSCSProjectInfoHelper.ExecutePath);
            _ = new VSCSharpProjectFileInternalWatcher(csprojPath, _projctWatcherCache, Notify);
            _projctWatcherCache[csprojPath].csFileWatcher.Dispose();
        }
        public void SetExecute(Func<Task> execute)
        {
            _execute = execute;
        }

        public void Notify()
        {
#if DEBUG
            Debug.WriteLine($"接收重建通知！");
#endif
            _timeStamp = DateTime.UtcNow.Ticks / 10000;
            _needExecuted = true;
        }

        public void StartMonitor()
        {
            
            Task.Run(async () => {

                while (true)
                {
                    if (_needExecuted == true)
                    {
                        Clean();
#if DEBUG
                        Debug.WriteLine($"时间戳差值: {DateTime.UtcNow.Ticks / 10000 - _timeStamp}");
#endif
                        if (DateTime.UtcNow.Ticks / 10000 - _timeStamp > 2000)
                        {
                            _needExecuted = false;
                            if (_execute!=null)
                            {
                                await _execute();
                            }
                           
                        }
                        await Task.Delay(800);
                    }
                    else
                    {
                        await Task.Delay(800);
                    }
                }
            });

        }


        private void Clean()
        {
            var currentProcessExePath = Process.GetCurrentProcess().MainModule.FileName;
            Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(VSCSProjectInfoHelper.ExecuteName));
            if (processes.Length > 1)
            {
                for (int i = 0; i < processes.Length; i++)
                {
                    try
                    {
                        var processor = processes[i];
                        if (processes[i].MainModule.FileName != currentProcessExePath)
                        {
                            processor.Kill();
                            processor.Dispose();
                        }
                    }
                    catch
                    {
                    }
                }
            }
            else if (processes.Length == 1)
            {
                var folders = Directory.GetDirectories(VSCSProjectInfoHelper.BinPath);
                foreach (var folder in folders)
                {
                    if (!_mainExecuteCache.Contains(folder))
                    {
                        Directory.Delete(folder, true);
                    }
                }
            }
        }

    }

    internal class VSCSharpProjectFileInternalWatcher
    {
        private Action? _notify;
        public VSCSharpProjectFileInternalWatcher(string csprojPath, ConcurrentDictionary<string, (FileSystemWatcher csprojWatcher, FileSystemWatcher csFileWatcher)> watcherCache, Action? notifyAction = null)
        {
            _notify = notifyAction;
            if (!watcherCache.ContainsKey(csprojPath))
            {
                if (File.Exists(csprojPath))
                {
                    (FileSystemWatcher csprojWatcher, FileSystemWatcher csFileWatcher) watcher = new();
                    if (!watcherCache.TryAdd(csprojPath, watcher))
                    {
                        return;
                    }

#if DEBUG
                    HEProxy.ShowMessage($"部署对 {csprojPath} 的监控. ");
#endif
                    var folder = Path.GetDirectoryName(csprojPath);
                    var fileName = Path.GetFileName(csprojPath);
                    var binFolder = Path.Combine(folder, "bin");
                    var objFolder = Path.Combine(folder, "obj");
                    FileSystemEventHandler csFileHandler = (sender, e) => {

                        if (VSCSProjectInfoHelper.CheckFileAvivaliable(e.FullPath, binFolder, objFolder))
                        {
                            _notify?.Invoke();
                        }

                    };
                    RenamedEventHandler csRenameHandler = (sender, e) => {

                        if (VSCSProjectInfoHelper.CheckFileAvivaliable(e.FullPath, binFolder, objFolder))
                        {
                            _notify?.Invoke();
                        }
                    };
                    FileSystemEventHandler csprojFileHandler = (sender, e) => {

                        if (e.FullPath == csprojPath)
                        {
                            _notify?.Invoke();
                        }

                    };
                    RenamedEventHandler csprojRenameHandler = (sender, e) => {

                        if (e.FullPath == csprojPath)
                        {
                            _notify?.Invoke();
                        }
                    };

                    var csfileWatcher = new FileSystemWatcher()
                    {
                        Path = folder,
                        Filter = "*",
                        EnableRaisingEvents = true,
                        IncludeSubdirectories = true,
                    };
                    csfileWatcher.Changed += csFileHandler;
                    csfileWatcher.Deleted += csFileHandler;
                    csfileWatcher.Created += csFileHandler;
                    csfileWatcher.Renamed += csRenameHandler;

                    var csprojWatcher = new FileSystemWatcher()
                    {
                        Path = folder,
                        Filter = fileName,
                        EnableRaisingEvents = true,
                        IncludeSubdirectories = false,
                        NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.LastAccess
                    };

                    csprojWatcher.Changed += csprojFileHandler;
                    csprojWatcher.Renamed += csprojRenameHandler;
                    csprojWatcher.Deleted += (sender, e) =>
                    {
                        if (e.FullPath == csprojPath)
                        {
                            csprojWatcher.Dispose();
                            csfileWatcher.Dispose();
                            watcherCache.TryRemove(csprojPath, out var _);
                        }
                    };
                    watcherCache[csprojPath] = (csprojWatcher, csfileWatcher);
                    MonitorDependencyProject(folder, csprojPath, watcherCache, notifyAction);
                }
                else
                {
                    throw new Exception($"未找到 {csprojPath} 文件");
                }
            }
        }

        private void MonitorDependencyProject(string foldPath, string csprojPath, ConcurrentDictionary<string, (FileSystemWatcher csprojWatcher, FileSystemWatcher csFileWatcher)> watcherList, Action? notifyAction)
        {
            try
            {
                XmlDocument doc = new();
                doc.Load(csprojPath);

                XmlNodeList projectReferenceNodes = doc.SelectNodes("//ProjectReference");

                if (projectReferenceNodes != null)
                {
                    foreach (XmlNode node in projectReferenceNodes)
                    {
                        var dependencyFilePath = node.Attributes["Include"].Value;
                        if (dependencyFilePath.EndsWith(".csproj"))
                        {
                            var newCsprojFilePath = Path.GetFullPath(Path.Combine(foldPath, dependencyFilePath));
                            new VSCSharpProjectFileInternalWatcher(newCsprojFilePath, watcherList, notifyAction);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                HEProxy.ShowMessage("错误: " + e.Message);
            }
        }
    }
}
