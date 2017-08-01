using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace System.Reflection.Emit
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
        #if DEBUG
                    writer = new StreamWriter(path, true, Encoding.UTF8);
        #endif

        }

        [Conditional("DEBUG")]
        public static void WriteLine(string recoder)
        {
            writer.WriteLine("\t" + recoder);
            Debug.WriteLine("\t" + recoder);
        }
        [Conditional("DEBUG")]
        public static void WriteLine(OpCode recoder)
        {
            WriteLine(recoder.Name);
        }
        [Conditional("DEBUG")]
        public static void WriteLine(OpCode recoder, string value)
        {
            WriteLine(recoder.Name, value);
        }
        [Conditional("DEBUG")]
        public static void WriteLine(string recoder,string value)
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
            writer.WriteLine("\t"+recoder+ splitSpace + value);
            System.Diagnostics.Debug.WriteLine("\t" + recoder + splitSpace + value);
        }
        [Conditional("DEBUG")]
        public static void WriteLine<T>(OpCode recoder, T result)
        {
            WriteLine(recoder.Name, result);
        }
        [Conditional("DEBUG")]
        public static void WriteLine<T>(string recoder, T result)
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
            writer.WriteLine("\t" + recoder + splitSpace + result.ToString());
            Debug.WriteLine("\t" + recoder + splitSpace + result.ToString());
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
            writer.WriteLine("\r\n****************************************\r\n\r\n");
            System.Diagnostics.Debug.WriteLine("\r\n****************************************\r\n\r\n");
        }
        [Conditional("DEBUG")]
        public static void Start(string name)
        {
            writer.WriteLine("\r\n************** " + name + " *************\r\n");
            Debug.WriteLine("\r\n************** " + name + " *************\r\n");
        }

        public static void Close()
        {
            writer.Close();
        }


        #region Emit扩展方法


        public static void REmit(this ILGenerator il, OpCode code) {
            il.Emit(code);
            WriteLine(code);
        }
        public static void REmit(this ILGenerator il, OpCode code,string value)
        {
            il.Emit(code, value);
            WriteLine(code, value);
        }
        public static void REmit(this ILGenerator il, OpCode code, char value)
        {
            il.Emit(code, value);
            WriteLine(code, value);
        }
        public static void REmit(this ILGenerator il, OpCode code, sbyte value)
        {
            il.Emit(code, value);
            WriteLine(code, value);
        }
        public static void REmit(this ILGenerator il, OpCode code, byte value)
        {
            il.Emit(code, value);
            WriteLine(code, value);
        }
        public static void REmit(this ILGenerator il, OpCode code, short value)
        {
            il.Emit(code, value);
            WriteLine(code, value);
        }
        public static void REmit(this ILGenerator il, OpCode code, ushort value)
        {
            il.Emit(code, value);
            WriteLine(code, value);
        }
        public static void REmit(this ILGenerator il, OpCode code, int value)
        {
            il.Emit(code, value);
            WriteLine(code, value);
        }
        public static void REmit(this ILGenerator il, OpCode code, uint value)
        {
            il.Emit(code, value);
            WriteLine(code, value);
        }
        public static void REmit(this ILGenerator il, OpCode code, long value)
        {
            il.Emit(code, value);
            WriteLine(code, value);
        }
        public static void REmit(this ILGenerator il, OpCode code, ulong value)
        {
            il.Emit(code, value);
            WriteLine(code, value);
        }
        public static void REmit(this ILGenerator il, OpCode code, float value)
        {
            il.Emit(code, value);
            WriteLine(code, value);
        }
        public static void REmit(this ILGenerator il, OpCode code, double value)
        {
            il.Emit(code, value);
            WriteLine(code, value);
        }
        public static void REmit(this ILGenerator il, OpCode code,Label label)
        {
            il.Emit(code, label);
            WriteLine(code, label.ToString());
        }
        public static void REmit(this ILGenerator il, OpCode code, MethodInfo info)
        {
            il.Emit(code, info);
            WriteLine(code, info.DeclaringType.Name +"'s " +info.Name);
        }
        public static void REmit(this ILGenerator il, OpCode code, FieldInfo info)
        {
            il.Emit(code, info);
            WriteLine(code, info.Name);
        }
        public static void REmit(this ILGenerator il, OpCode code, Type info)
        {
            il.Emit(code, info);
            WriteLine(code, info.Name);
        }
        public static void REmit(this ILGenerator il, OpCode code, ConstructorInfo info)
        {
            il.Emit(code, info);
            WriteLine(code, info.Name);
        }
        public static void REmit(this ILGenerator il, OpCode code, LocalBuilder info)
        {
            il.Emit(code, info);
            WriteLine(code, info.LocalIndex);
        }

        #endregion
    }
}
