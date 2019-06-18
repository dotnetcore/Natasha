using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Natasha
{
    public class NWarning { }
    public class NError { }
    public class NSucceed { }
    public static class NScriptLogWriter<T>
    {


        public static readonly StreamWriter LogWriter;
        public static readonly ConcurrentQueue<string> LogQueue;


        static NScriptLogWriter()
        {
            LogQueue = new ConcurrentQueue<string>();
            LogWriter = new StreamWriter($"{typeof(T).Name}.log", true, Encoding.UTF8);
        }

        public static void Show(string msg)
        {
            Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + msg);
        }

        public static void Recoder(string title, string msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"\r\n\r\n==================={title}===================\r\n");
            sb.AppendLine(msg);
            sb.AppendLine("==========================================================");
            LogQueue.Enqueue(sb.ToString());
            lock (LogWriter)
            {
                while (LogQueue.Count > 0)
                {
                    if (LogQueue.TryDequeue(out string result))
                    {
                        LogWriter.WriteLine(result);
                        LogWriter.Flush();
                    }
                }
            }
        }
    }
}
