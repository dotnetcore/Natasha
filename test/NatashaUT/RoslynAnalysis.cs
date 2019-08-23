using Natasha.Complier;
using Xunit;

namespace NatashaUT
{
    [Trait("Roslyn", "类")]
    public class RoslynAnalysis
    {
        [Fact(DisplayName = "类名获取")]
        public void Test1()
        {
            string text = @"using System;
using System.Collections;
using System.Linq;
using System.Text;
 
namespace HelloWorld
{
    public class TestIndex1
    {
        public string Name;
        public int Age{get;set;}
    }
    public class TestIndex2
    {
        public string Name;
        public int Age{get;set;}
    }

    public class TestIndex3
    {
        public string Name;
        public int Age{get;set;}
    }
}

namespace HelloWorld{
    public class TestIndex4
    {
        public string Name;
        public int Age{get;set;}
    }
}

";
            var (_, typeNames, _,_) = text;
            Assert.Equal("TestIndex1", typeNames[0]);
            Assert.Equal("TestIndex2", typeNames[1]);
            Assert.Equal("TestIndex3", typeNames[2]);
            Assert.Equal("TestIndex4", typeNames[3]);
        }
    }
}
