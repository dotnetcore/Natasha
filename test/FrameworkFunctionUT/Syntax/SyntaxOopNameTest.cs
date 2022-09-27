using Microsoft.CodeAnalysis;
using Natasha.CSharp.Extension.Inner;
using Xunit;

namespace FrameworkFunctionUT.Syntax
{
    [Trait("基础功能测试", "语法")]
    public class SyntaxOopNameTest
    {
        [Fact(DisplayName = "获取存储结构名")]
        public void GetOOPName1()
        {

            string text = @"using System;
using System.Collections;
using System.Linq;
using System.Text;
 
namespace HelloWorld
{
    public class TestClass1
    {
        public string Name;
        public int Age{get;set;}
    }
    public class TestClass2
    {
        public string Name;
        public int Age{get;set;}
    }
}

namespace HelloWorld{
    public struct TestStruct1{}
    public struct TestStruct2
    {
        public string Name;
        public int Age{get;set;}
    }
}
namespace HelloWorld2{
    public enum TestEnum1{}
    public enum TestEnum2
    {
        a,
        b
    }
}
namespace HelloWorld3{
    public interface TestInterface1{}
    public interface TestInterface2{}
}
namespace HelloWorld4{
    public record TestRecord1{}
    public record TestRecord2{}
}
";
            //根据脚本创建动态类
            AssemblyCSharpBuilder builder = new();
            builder.Add(text);
            var tree = builder.SyntaxTrees[0];

            for (int i = 0; i < 5; i++)
            {
                AssertOopName(tree, i);
            }
        }

        private void AssertOopName(SyntaxTree tree, int namespaceIndex)
        {
            var node = tree.NamespaceNode(namespaceIndex);
            if (node.GetClassName() != string.Empty)
            {
                Assert.Equal("TestClass1", node.GetClassName(0));
                Assert.Equal("TestClass2", node.GetClassName(1));

            }
            else if (node.GetStructName() != string.Empty)
            {
                Assert.Equal("TestStruct1", node.GetStructName(0));
                Assert.Equal("TestStruct2", node.GetStructName(1));
            }
            else if (node.GetInterfaceName() != string.Empty)
            {
                Assert.Equal("TestInterface1", node.GetInterfaceName(0));
                Assert.Equal("TestInterface2", node.GetInterfaceName(1));
            }
            else if (node.GetEnumName() != string.Empty)
            {
                Assert.Equal("TestEnum1", node.GetEnumName(0));
                Assert.Equal("TestEnum2", node.GetEnumName(1));
            }
            else if (node.GetRecordName() != string.Empty)
            {
                Assert.Equal("TestRecord1", node.GetRecordName(0));
                Assert.Equal("TestRecord2", node.GetRecordName(1));
            }


        }



        [Fact(DisplayName = "获取方法")]
        public void RunClassName2()
        {
            //ScriptCompiler.Init();
            string text = @"using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Collections.Generic;
namespace HelloWorld
{
    public class TestIndex1
    {
        public void M1(){}
        public void M2(){}
    }
    public class TestIndex2
    {
        public void M3(){}
        public void M4(){}
        public void M5(){}
    }
}

namespace HelloWorld{

    public struct TestStruct1
    {
        public void M6(){}
        public void M7(){}
    }
}
namespace HelloWorld2{

    public interface TestInterface1
    {
        public void M8(){}
        public void M9(){}
    }
}
namespace HelloWorld3{

    public record TestRecord1
    {
        public void M10(){}
        public void M11(){}
        public void M12(){}
    }
}
";


            //根据脚本创建动态类
            AssemblyCSharpBuilder builder = new();
            builder.Add(text);
            var tree = builder.SyntaxTrees[0];

            var namspaceIndex = 0;
            var methodIndex = 0;
            var node = tree.NamespaceNode(namspaceIndex);
            for (int i = 0; i < 12; i++)
            {
                var result = node.GetMethodName(methodIndex);
                if (result == string.Empty)
                {
                    namspaceIndex += 1;
                    node = tree.NamespaceNode(namspaceIndex);
                    methodIndex = 0;
                    result = node.GetMethodName(methodIndex);
                }
                methodIndex += 1;
                Assert.Equal($"M{i+1}", result);
            }
        }



    }
}
