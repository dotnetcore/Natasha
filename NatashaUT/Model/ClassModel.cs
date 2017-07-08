namespace NatashaUT.Model
{
    public class FieldClass
    {
        public ulong ValueField;
        public static ulong StaticValueField;

        public string RefField;
        public static string StaticRefField;
    }

    public class PropertyClass
    {
        public ulong ValueProperty { get; set; }
        public static ulong StaticValueProperty { get; set; }

        public string RefProperty { get; set; }
        public static string StaticRefProeprty { get; set; }
    }

    public class MethodClass
    {
        public ulong GetULongMax()
        {
            return ulong.MaxValue;
        }

        public ulong GetULongMin()
        {
            return ulong.MinValue;
        }

        public string GetString(string a,string b)
        {
            return a + b;
        }
    }



    public delegate ulong DGetULongMax();
    public delegate ulong DGetULongMin();
    public class DelegateClass
    {
        public DGetULongMax GetMax;

        public DGetULongMin GetMin;

        
        public DelegateClass()
        {
            GetMax = () => { return ulong.MaxValue; };
            GetMin = () => { return ulong.MinValue; };
        }
    }
}
