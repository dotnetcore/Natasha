using Natasha;
using System;

namespace Core20
{
    class Program
    {
        static void Main(string[] args)
        {


            string text = @"namespace HelloWorld
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
            AssemblyCompiler oop = new AssemblyCompiler("test");
            oop.Add(text);
            Type type = oop.GetType("Test");

            Console.WriteLine(type.Name);

            var action = NDelegate.Random().Action("");
            var a = action.Method;
            Console.WriteLine(action.Method.Module.Assembly);


            Console.ReadKey();
        }
    }
}
