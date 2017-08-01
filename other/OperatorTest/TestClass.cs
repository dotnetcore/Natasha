using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatorTest
{
    public class TestClass
    {
        public int Field;
        public int Property { get; set; }

        public string PropertyName { get; set; }

        public TestStruct Next;
    }

    public struct TestStruct
    {
        public int Field;
        public int Property { get; set; }
    }
}
