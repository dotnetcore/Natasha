using Natasha;
using System;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             *   在此之前，你需要右键，选择工程文件，在你的.csproj里面 
             *   
             *   写上这样一句浪漫的话： 
             *   
             *      <PreserveCompilationContext>true</PreserveCompilationContext>
             */
            ScriptBuilder maker = new ScriptBuilder();

            var delegateAction = maker
                .Namespace(typeof(Console))
                .Param<string>("str1")
                .Param<string>("str2")
                .Body(@"
                    string result = str1 +"" ""+ str2;
                    Console.WriteLine(result);
                                               ")
                .Return();

            ((Action<string, string>)delegateAction)("Hello", "World!");


            Console.ReadKey();
        }
    }
}
