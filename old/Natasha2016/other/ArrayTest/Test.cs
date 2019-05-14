using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayTest
{
    public class ClassProperty
    {
        public string PublicName { get; set; }
        public int PublicAge { get; set; }

        private string PrivateName { get; set; }
        private int PrivateAge { get; set; }
    }
    public class ClassField
    {
        public string PublicName;
        public int PublicAge;

        private string PrivateName;
        private int PrivateAge;
    }
    public struct StructProperty
    {
        public string PublicName { get; set; }
        public int PublicAge { get; set; }

        private string PrivateName { get; set; }
        private int PrivateAge { get; set; }
    }
    public struct StructField
    {
        public string PublicName;
        public int PublicAge;

        private string PrivateName;
        private int PrivateAge;
    }

    public enum DocumentEnum
    {
        Age,
        Address,
        Description,
        ID
    }

    public class TestClass
    {
        public const int A = 10;
        public TestStruct[] TestArray;
        private string PrivatePName { get; set; }
        private int PrivatePAge { get; set; }

        private string PrivateFName;
        private int PrivateFAge;
        public string Name { get; set; }
        public string Name1 { private get { return PrivateFName; } set { PrivateFName = null; } }
        public int Age;

        public readonly string Ref_NormalField;
        public int NormalField;
        public int NormalProperty { get; set; }
        public string Ref_NormalProperty { get; set; }

        public TestStruct FieldNext;
        public TestStruct PropertyNext1 { get; set; }
        public TestClass PropertyNext { get; set; }

        public static int StaticField;
        public static string Ref_StaticField;
        public static int StaticProperty { get; set; }
        public static string Ref_StaticProperty { get; set; }
        public TestInterface PInterface;

        public TestClass()
        {
            Age = 1;
            PrivatePName = "PrivatePName";
            PrivatePAge = 222;
            PrivateFName = "PrivateFName";
            PrivateFAge = 333;
        }
        public void ShowInfo(string name, int age)
        {
            Console.WriteLine("Name is :{0}, Age is :{1}", Name, Age);
        }
        public static void StaticShowInfo(string name, int age)
        {
            Console.WriteLine("Name is :{0}, Age is :{1}", name, age);
        }
    }
    public class TestInterfaceModel : TestInterface
    {
        public bool Shut
        {
            get;
            set;
        }

        public void Show()
        {
            Console.WriteLine(1);
        }
    }
    public interface TestInterface
    {
        bool Shut { get; set; }
        void Show();
    }
    public struct TestStruct
    {
        public TestEnum TEnum;
        public string Name { get; set; }
        public int Age;
        private int PrivatePAge { get; set; }
        private string PrivatePName { get; set; }

        private int PrivateFAge;
        private string PrivateFName;
        public int Age1 { get; set; }
        public string Name1;
        public void Set()
        {
            PrivatePAge = 60;
            PrivatePName = "600";
            PrivateFAge = 606;
            PrivateFName = "66";
        }
        public void Show()
        {
            Console.WriteLine(PrivatePAge);
            Console.WriteLine(PrivatePName);
            Console.WriteLine(PrivateFAge);
            Console.WriteLine(PrivateFName);
        }
    }

    public enum TestEnum
    {
        Name,
        Age,
        Description,
        Address
    }
}
