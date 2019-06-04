using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Natasha
{

    public static class NDebug
    {
        public static readonly StreamWriter Writer;
        public static readonly ConcurrentQueue<string> LogQueue;
        public static readonly StringBuilder LogRecoder;
        static NDebug() {
            LogQueue = new ConcurrentQueue<string>();
            Writer = new StreamWriter("Debug.log", true, Encoding.UTF8);
            LogRecoder = new StringBuilder();
        }
        [Conditional("DEBUG")]
        public static void Show(string msg)
        {
            Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ")+msg);
        }

        [Conditional("DEBUG")]
        public static void FileRecoder(string title,string msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"\r\n\r\n==================={title}===================\r\n");
            sb.AppendLine(msg);
            sb.AppendLine("==========================================================");
            sb.Replace(";\r\n", ";").Replace(";",";\r\n").Replace("get;\r\n", "get;").Replace("set;\r\n", "set;");
            LogQueue.Enqueue(sb.ToString());
            Write();
        }

        [Conditional("DEBUG")]
        public static void FileRecoder(string msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"\r\n\r\n============================================\r\n");
            sb.AppendLine(msg);
            sb.AppendLine("============================================");
            sb.Replace(";\r\n", ";").Replace(";", ";\r\n").Replace("get;\r\n","get;").Replace("set;\r\n", "set;");
            LogQueue.Enqueue(sb.ToString());
            Write();
        }
        [Conditional("DEBUG")]
        internal async static void Write()
        {
            while (LogQueue.Count > 0)
            {
                if (LogQueue.TryDequeue(out string result))
                {
                    await Writer.WriteLineAsync(result);
                    Writer.Flush();
                } 
            }
        }

    }
}
