using System;
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

        public static readonly string YearPath;
        public static readonly string DayPath;
        private static readonly string _logFile;
        private static readonly object _lock;
        static NScriptLogWriter()
        {
            _lock = new object();
           

            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log/");
            if (!Directory.Exists(logPath))
            {

                Directory.CreateDirectory(logPath);

            }


            YearPath = Path.Combine(logPath, DateTime.Now.Year.ToString());
            if (!Directory.Exists(YearPath))
            {

                Directory.CreateDirectory(YearPath);

            }


            DayPath = Path.Combine(YearPath, DateTime.Now.ToString("MM月dd日"));
            if (!Directory.Exists(DayPath))
            {

                Directory.CreateDirectory(DayPath);

            }

            _logFile = Path.Combine(DayPath, $"{typeof(T).Name}.log");
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
            lock (_lock)
            {

                using (StreamWriter LogWriter = new StreamWriter(_logFile, true, Encoding.UTF8))
                {

                    LogWriter.WriteLine(sb);

                }

            }

        }

    }

}