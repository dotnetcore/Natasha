using Natasha;
using System;
using Xunit;

namespace NatashaUT
{
    [Trait("快速构建","完整类")]
    public class DynamicClassTest
    {


        [Fact(DisplayName = "动态类生成测试")]
        public static void RunClassName0()
        {
            //ScriptComplier.Init();
            string text = @"
 
namespace HelloWorld
{
    public class Test
    {
        public Test(){
            Name=""111"";
        }

        public string Name;
        public int Age{get;set;}
    }
}";
            //根据脚本创建动态类
            Type type = RuntimeComplier.GetType(text);
            Assert.Equal("Test", type.Name);
        }



        [Fact(DisplayName = "选择类")]
        public static void RunClassName1()
        {
            //ScriptComplier.Init();
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
            //根据脚本创建动态类
            Type type = RuntimeComplier.GetType(text,3);
            Assert.Equal("TestIndex3", type.Name);
        }



        [Fact(DisplayName = "选择命名空间+类")]
        public static void RunClassName2()
        {
            //ScriptComplier.Init();
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
            //根据脚本创建动态类
            Type type = RuntimeComplier.GetType(text, 1,2);
            Assert.Equal("TestIndex4", type.Name);
        }
    }
}
