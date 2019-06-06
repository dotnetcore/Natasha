using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Natasha
{

    public static class NDebug
    {
        public static readonly StreamWriter SucceedWriter;
        public static readonly StreamWriter ErrorWriter;
        public static readonly ConcurrentQueue<string> SucceedQueue;
        public static readonly ConcurrentQueue<string> ErrorQueue;
        static NDebug() {
            SucceedQueue = new ConcurrentQueue<string>();
            ErrorQueue = new ConcurrentQueue<string>();
            SucceedWriter = new StreamWriter("Succeed.log", true, Encoding.UTF8);
            ErrorWriter = new StreamWriter("Error.log",true,Encoding.UTF8);
        }
        [Conditional("DEBUG")]
        public static void Show(string msg)
        {
            Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ")+msg);
        }

        [Conditional("DEBUG")]
        public static void SucceedRecoder(string title,string msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"\r\n\r\n==================={title}===================\r\n");
            sb.AppendLine(msg);
            sb.AppendLine("==========================================================");
            SucceedQueue.Enqueue(sb.ToString());
            SucceedWrite();
        }

        [Conditional("DEBUG")]
        public static void SucceedRecoder(string msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"\r\n\r\n============================================\r\n");
            sb.AppendLine(msg);
            sb.AppendLine("============================================");
            SucceedQueue.Enqueue(sb.ToString());
            SucceedWrite();
        }
        [Conditional("DEBUG")]
        internal static void SucceedWrite()
        {
            while (SucceedQueue.Count > 0)
            {
                if (SucceedQueue.TryDequeue(out string result))
                {
                    SucceedWriter.WriteLine(result);
                    SucceedWriter.Flush();
                } 
            }
        }


        [Conditional("DEBUG")]
        public static void ErrorRecoder(string title, string msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"\r\n\r\n==================={title}===================\r\n");
            sb.AppendLine(msg);
            sb.AppendLine("==========================================================");
            ErrorQueue.Enqueue(sb.ToString());
            ErrorWrite();
        }

        [Conditional("DEBUG")]
        public static void ErrorRecoder(string msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"\r\n\r\n============================================\r\n");
            sb.AppendLine(msg);
            sb.AppendLine("============================================");
            ErrorQueue.Enqueue(sb.ToString());
            ErrorWrite();
        }
        [Conditional("DEBUG")]
        internal static void ErrorWrite()
        {
            while (ErrorQueue.Count > 0)
            {
                if (ErrorQueue.TryDequeue(out string result))
                {
                    ErrorWriter.WriteLine(result);
                    ErrorWriter.Flush();
                }
            }
        }


        
    }
}
