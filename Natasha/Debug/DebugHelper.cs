using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Natasha.Debug
{
    
    public static class DebugHelper
    {
        static StreamWriter writer=null;
        
        static DebugHelper()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\IL_Debug.txt";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            writer = new StreamWriter(path, true, Encoding.UTF8);
        }

        [Conditional("DEBUG")]
        public static void WriteLine(string recoder)
        {
            writer.WriteLine(recoder);
            System.Diagnostics.Debug.WriteLine(recoder);
        }
        [Conditional("DEBUG")]
        public static void WriteLine(string recoder,string valueName)
        {
            string splitSpace = "\t\t\t";
            if (recoder.Length > 7)
            {
                splitSpace = "\t";
            }
            else if (recoder.Length > 3)
            {
                splitSpace = "\t\t";
            }
            writer.WriteLine(recoder+ splitSpace + valueName);
            System.Diagnostics.Debug.WriteLine(recoder + splitSpace + valueName);
        }
        [Conditional("DEBUG")]
        public static void WriteLine(string recoder, int result)
        {
            string splitSpace = "\t\t\t";
            if (recoder.Length > 7)
            {
                splitSpace = "\t";
            }
            else if (recoder.Length > 3)
            {
                splitSpace = "\t\t";
            }
            writer.WriteLine(recoder + splitSpace + result.ToString());
            System.Diagnostics.Debug.WriteLine(recoder + splitSpace + result.ToString());
        }
        [Conditional("DEBUG")]
        public static void Write(string recoder)
        {
            writer.Write(recoder);
            System.Diagnostics.Debug.Write(recoder);
        }
        [Conditional("DEBUG")]
        public static void End()
        {
            writer.WriteLine("\r\n*********************************\r\n\r\n");
            System.Diagnostics.Debug.WriteLine("\r\n*********************************\r\n\r\n");
        }
        [Conditional("DEBUG")]
        public static void Start(string name)
        {
            writer.WriteLine("\r\n**************", name + " *************\r\n");
            System.Diagnostics.Debug.WriteLine("\r\n**************", name + " *************\r\n");
        }

        public static void Close()
        {
            writer.Close();
        }
    }
}
