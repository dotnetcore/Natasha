using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplexTest
{
    public class SingleModel
    {
        public TestStruct[] TestStructArray;
        public TestClass[] TestClassArray;
        public TestEnum[] TestEnumArray;
        public string Name { get; set; }
        public int Age;
        private string PName { get; set; }
        private int PAge;

        private TestStruct FieldFStruct;
        private TestStruct FieldPStruct { get; set; }

        public TestStruct PropertyFStruct;
        public TestStruct PropertyPStruct { get; set; }
        public void Set()
        {
            PAge = 111;
            PName = "222";
        }

        public void Show()
        {
            Console.WriteLine(PAge);
            Console.WriteLine(PName);
        }
    }
}
