using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Natasha.Log
{


    public static class NWriter<T>
    {

        private static string _logPath;
        private static readonly object _lock;
        private static Func<string> _log_name;
        private static Func<string> _log_folder;
        public static int FolderDeepth
        {

            set
            {
                string suffix = "-"+typeof(T).Name + ".log";
                _logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"log");
                if (!Directory.Exists(_logPath))
                {

                    Directory.CreateDirectory(_logPath);

                }

                if (value == 1)
                {

                    _log_name = () => DateTime.Now.ToString("yyyy-MM-dd")+ suffix;
                    _log_folder = () => default;

                }


                if (value == 2)
                {

                    _log_name = () => DateTime.Now.ToString("MM-dd") + suffix;
                    _log_folder = () =>
                    {
                        var temp = Path.Combine(_logPath, DateTime.Now.ToString("yyyy"));
                        if (!Directory.Exists(temp))
                        {

                            Directory.CreateDirectory(temp);

                        }
                        return temp;
                    };
                    

                }


                if (value == 3)
                {

                    _log_name = () => DateTime.Now.ToString("dd") + suffix;
                    _log_folder = () =>
                    {
                        var temp = Path.Combine(_logPath, DateTime.Now.ToString("yyyy-MM"));
                        if (!Directory.Exists(temp))
                        {

                            Directory.CreateDirectory(temp);

                        }
                        return temp;
                    };

                }
                
            }

        }




        static NWriter()
        {

            FolderDeepth = 3;   //日志定位到 ‘月/日’文件夹
            _lock = new object();

        }




        public static void Show(string msg)
        {

            Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + msg);

        }

        public static void Recoder(StringBuilder buffer)
        {

            lock (_lock)
            {
                var filePath = Path.Combine(_log_folder(), _log_name());
                using (StreamWriter LogWriter = new StreamWriter(filePath, true, Encoding.UTF8))
                {

                    LogWriter.WriteLine(buffer);

                }

            }

        }

    }

}