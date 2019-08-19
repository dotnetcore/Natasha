using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Natasha.Log
{


    public static class NWriter<T>
    {

        private static readonly string _logFile;
        private static readonly object _lock;
        public static bool Enabled;


        static NWriter()
        {

            Enabled = true;
            _lock = new object();
           

            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log/");
            if (!Directory.Exists(logPath))
            {

                Directory.CreateDirectory(logPath);

            }


            var YearPath = Path.Combine(logPath, DateTime.Now.Year.ToString());
            if (!Directory.Exists(YearPath))
            {

                Directory.CreateDirectory(YearPath);

            }


            var DayPath = Path.Combine(YearPath, DateTime.Now.ToString("MM月dd日"));
            if (!Directory.Exists(DayPath))
            {

                Directory.CreateDirectory(DayPath);

            }


            var HoursPath = Path.Combine(DayPath, DateTime.Now.ToString("HH时mm分"));
            if (!Directory.Exists(HoursPath))
            {

                Directory.CreateDirectory(HoursPath);

            }


            _logFile = Path.Combine(HoursPath, $"{typeof(T).Name}.log");
        }




        public static void Show(string msg)
        {

            Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + msg);

        }

        public static void Recoder(StringBuilder buffer)
        {
            if (Enabled)
            {
                
                lock (_lock)
                {

                    using (StreamWriter LogWriter = new StreamWriter(_logFile, true, Encoding.UTF8))
                    {

                        LogWriter.WriteLine(buffer);

                    }

                }
            }

        }

    }

}