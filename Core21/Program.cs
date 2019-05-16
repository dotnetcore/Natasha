using Natasha;
using System;

namespace Core21
{
    class Program
    {
        static void Main(string[] args)
        {
            ScriptBuilder maker = new ScriptBuilder();

            var delegateAction = maker
                .Namespace(typeof(Console))
                .Param<string>("str1")
                .Param<string>("str2")
                .Body(@"
                    string result = str1 +"" ""+ str2;
                    Console.WriteLine(result);
                                               ")
                .Return()
                .Create();

            ((Action<string, string>)delegateAction)("Hello", "World!");

            ScriptBuilder maker2 = new ScriptBuilder();
            var delegateAction2 = maker2
                .Namespace(typeof(Console))
                .Param<string>("str1")
                .Param<string>("str2")
                .Body(@"
                    string result = str1 +"" ""+ str2;
                    Console.WriteLine(result);
                                               ")
                .Return()
                .Create<Action<string, string>>();

            delegateAction2("Hello", "World!");

            ClassBuilder classBuilder = new ClassBuilder();
            string text = @"using System;
using System.Collections;
using System.Linq;
using System.Text;
 
namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello, World!"");
        }
    }
}";
            Type type = classBuilder.GetType(text);
            Console.WriteLine(type.Name);
            Console.ReadKey();
        }
    }
}
