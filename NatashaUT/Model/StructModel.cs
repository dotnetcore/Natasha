using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatashaUT.Model
{
    public struct FieldStruct
    {
        public ulong ValueField;
        public static ulong StaticValueField;

        public string RefField;
        public static string StaticRefField;
    }

    public struct PropertyStruct
    {
        public ulong ValueProperty { get; set; }
        public static ulong StaticValueProperty { get; set; }

        public string RefProperty { get; set; }
        public static string StaticRefProeprty { get; set; }
    }

    public struct MethodStruct
    {
        public ulong GetULongMax()
        {
            return ulong.MaxValue;
        }

        public ulong GetULongMin()
        {
            return ulong.MinValue;
        }

        public string GetString(string a, string b)
        {
            return a + b;
        }
    }



    //public delegate ulong DGetULongMax();
    //public delegate ulong DGetULongMin();
    //public class DelegateStruct
    //{
    //    public DGetULongMax GetMax;

    //    public DGetULongMin GetMin;


    //    public DelegateClass()
    //    {
    //        GetMax = () => { return ulong.MaxValue; };
    //        GetMin = () => { return ulong.MinValue; };
    //    }
    //}
}
