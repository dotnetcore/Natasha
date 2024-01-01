using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Threading;


internal static class StopwatchExtension
{
    internal static bool _enableMemroryMonitor;
    private static long _preThreadId;
    private static readonly object _lock;
    private static readonly ConcurrentDictionary<ScoreRange, ConsoleColor> _colorCache;
    private static readonly ConcurrentDictionary<ScoreRange, ConsoleColor> _memoryCache;
    private static long _preMemorySize;
    static StopwatchExtension()
    {
        _preMemorySize = 0;
        _lock = new object();
        _colorCache = new ConcurrentDictionary<ScoreRange, ConsoleColor>();
        _memoryCache = new ConcurrentDictionary<ScoreRange, ConsoleColor>();
        
        _colorCache[new ScoreRange(0, 20)] = ConsoleColor.Green;
        _memoryCache[new ScoreRange(0, 5)] = ConsoleColor.Green;

        _colorCache[new ScoreRange(20, 100)] = ConsoleColor.Cyan;
        _memoryCache[new ScoreRange(5, 10)] = ConsoleColor.Cyan;

        _colorCache[new ScoreRange(100, 500)] = ConsoleColor.Yellow;
        _memoryCache[new ScoreRange(10, 50)] = ConsoleColor.Yellow;

        _colorCache[new ScoreRange(500, 1000)] = ConsoleColor.Magenta;
        _memoryCache[new ScoreRange(50, 100)] = ConsoleColor.Magenta;

        _colorCache[new ScoreRange(1000, 100000)] = ConsoleColor.Red;
        _memoryCache[new ScoreRange(100, 100000)] = ConsoleColor.Magenta;
    }


    internal static void EnableMemoryMonitor()
    {
        _enableMemroryMonitor = true;
    }

    internal static void StartMonitor(this Stopwatch stopwatch)
    {
        if (!_enableMemroryMonitor)
        {
            stopwatch.Start();
        }
    }

    private static long GetMemorySize()
    {
        return Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024;
    }


    internal static void StopAndShowCategoreInfo(this Stopwatch stopwatch, string nodeName, string info, int level)
    {
        if (_enableMemroryMonitor)
        {
            long currentMemorySize = GetMemorySize();
            while (true)
            {
                Thread.Sleep(3000);
                if (currentMemorySize != GetMemorySize())
                {
                    currentMemorySize = GetMemorySize();
                }
                else
                {
                    break;
                }
            }
            ShowCategoreInfo(stopwatch, nodeName, info, level, currentMemorySize);
        }
        else
        {
            stopwatch.Stop();
            ShowCategoreInfo(stopwatch, nodeName, info, level);
        }
    }


    internal static void RestartAndShowCategoreInfo(this Stopwatch stopwatch, string nodeName, string info, int level)
    {
        if (_enableMemroryMonitor)
        {
            long currentMemorySize = GetMemorySize();
            while (true)
            {
                Thread.Sleep(3000);
                if (currentMemorySize != GetMemorySize())
                {
                    currentMemorySize = GetMemorySize();
                }
                else
                {
                    break;
                }
            }
            ShowCategoreInfo(stopwatch, nodeName, info, level, currentMemorySize);
        }
        else
        {
            stopwatch.Stop();
            ShowCategoreInfo(stopwatch, nodeName, info, level);
            stopwatch.Restart();
        }
    }



    internal static void ShowCategoreInfo(Stopwatch stopwatch, string nodeName, string info, int level, long currentMemorySize = 0)
    {
        if (_enableMemroryMonitor)
        {
            var realSize = 0L;
            lock (_lock)
            {
                realSize = currentMemorySize - _preMemorySize;
                _preMemorySize = currentMemorySize;
            }
            foreach (var item in _memoryCache)
            {
                if (item.Key.IsInRange(realSize))
                {
                    var showInfo = new StringBuilder();
                    for (int i = 0; i < level; i += 1)
                    {
                        showInfo.Append("\t");
                    }
                    showInfo.Append($"└─ {nodeName}\t{info} : {(realSize > 0 ? "+" : "")}{realSize}M / {currentMemorySize}M (change/total)");
                    ShowWithThreadInfo(showInfo, item.Value);
                    break;
                }
            }

            
           
        }
        else
        {
            foreach (var item in _colorCache)
            {
                if (item.Key.IsInRange(stopwatch.ElapsedMilliseconds))
                {
                    var showInfo = new StringBuilder();
                    for (int i = 0; i < level; i += 1)
                    {
                        showInfo.Append("\t");
                    }
                    showInfo.Append($"└─ {nodeName}\t{info} : {stopwatch.ElapsedMilliseconds}ms");
                    ShowWithThreadInfo(showInfo, item.Value);
                    break;
                }
            }
        }
    }

    private static void ShowWithThreadInfo(StringBuilder message, ConsoleColor color)
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;
        message.Append($" (Thread:{threadId})");
        lock (_lock)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(threadId == _preThreadId ? message : $"{Environment.NewLine}{message}");
            Console.ForegroundColor = oldColor;
            _preThreadId = threadId;
        }
       
    }


    /// <summary>
    /// 设置颜色等级
    /// </summary>
    /// <param name="stopwatch"></param>
    /// <param name="scoreRange"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    internal static Stopwatch SetLevel(this Stopwatch stopwatch, ScoreRange scoreRange, ConsoleColor color)
    {
        _colorCache[scoreRange] = color;
        return stopwatch;
    }
}

/// <summary>
/// 分数模型
/// </summary>
internal class ScoreRange
{
    private readonly long _min;
    private readonly long _max;

    internal ScoreRange(long min, long max)
    {
        _min = min;
        _max = max;
    }
    /// <summary>
    /// 判断得分是否在范围内
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    internal bool IsInRange(long score)
    {
        return _min <= score && score <= _max;
    }
}

