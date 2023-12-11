using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Threading;


internal static class StopwatchExtension
{
    internal static bool _enableMemroryMonitor;
    private static readonly object _lock;
    private static readonly ConcurrentDictionary<ScoreRange, ConsoleColor> _colorCache;
    private static long _preMemorySize;
    static StopwatchExtension()
    {
        _preMemorySize = 0;
        _lock = new object();
        _colorCache = new ConcurrentDictionary<ScoreRange, ConsoleColor>();
        _colorCache[new ScoreRange(0, 20)] = ConsoleColor.Green;
        _colorCache[new ScoreRange(20, 100)] = ConsoleColor.Cyan;
        _colorCache[new ScoreRange(100, 500)] = ConsoleColor.Yellow;
        _colorCache[new ScoreRange(500, 1000)] = ConsoleColor.Magenta;
        _colorCache[new ScoreRange(1000, 100000)] = ConsoleColor.Red;
    }

    internal static void EnableMemoryMonitor()
    {
        _enableMemroryMonitor = true;
    }

    private static long GetMemorySize()
    {
        return Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024;
    }


    internal static void StopAndShowCategoreInfo(this Stopwatch stopwatch, string nodeName, string info, int level)
    {
        stopwatch.Stop();
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
            ShowCategoreInfo(stopwatch, nodeName, info, level);
        }

       
    }


    internal static void RestartAndShowCategoreInfo(this Stopwatch stopwatch, string nodeName, string info, int level)
    {
        stopwatch.Stop();
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
            ShowCategoreInfo(stopwatch, nodeName, info, level);
        }
        
        stopwatch.Restart();
    }



    internal static void ShowCategoreInfo(Stopwatch stopwatch, string nodeName, string info, int level, long currentMemorySize = 0)
    {
        var realSize = 0L;
        var color = Console.ForegroundColor;
        foreach (var item in _colorCache)
        {
            if (item.Key.IsInRange(stopwatch.ElapsedMilliseconds))
            {
                var showInfo = new StringBuilder();
                for (int i = 0; i < level; i += 1)
                {
                    showInfo.Append("\t");
                }
                if (_enableMemroryMonitor)
                {
                    lock (_lock)
                    {
                        realSize = currentMemorySize - _preMemorySize;
                        _preMemorySize = currentMemorySize;
                    }
                    showInfo.Append($"---{nodeName}\t{info} : {stopwatch.ElapsedMilliseconds}ms / {(realSize > 0 ? "+" : "")}{realSize}M / {currentMemorySize}M (execute/change/total) (Thread:{Thread.CurrentThread.ManagedThreadId})");
                    //Console.WriteLine($"---{nodeName}\t{info} : {stopwatch.ElapsedMilliseconds}ms / {(realSize > 0 ? "+" : "")}{realSize}M / {currentMemorySize}M (execute/change/total) (Thread:{Thread.CurrentThread.ManagedThreadId})");
                }
                else
                {
                    showInfo.Append($"---{nodeName}\t{info} : {stopwatch.ElapsedMilliseconds}ms (Thread:{Thread.CurrentThread.ManagedThreadId})");
                    //Console.WriteLine($"---{nodeName}\t{info} : {stopwatch.ElapsedMilliseconds}ms (Thread:{Thread.CurrentThread.ManagedThreadId})");
                }
                Console.ForegroundColor = item.Value;
                Console.WriteLine(showInfo);
                break;
            }
        }
        Console.ForegroundColor = color;
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

