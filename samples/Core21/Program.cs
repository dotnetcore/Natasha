using Natasha;
using System;

namespace Core21
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

            string text = @"using System;
using System.Collections;
using System.Linq;
using System.Text;
using ClassLibrary1;
 
namespace HelloWorld
{
    public class Test
    {
        public Test(){
            Name=""111"";
        }

        public string Name;
        public int Age{get;set;}

        public override string ToString(){

            Class1 a = new Class1();
            a.Show1();
            Class1.Show2();
            return ""11"";

        }
    }
}";
            //根据脚本创建动态类
            OopComplier oop = new OopComplier();
            oop.LoadFile(@"D:\Project\IlTest\ClassLibrary1\bin\Debug\netstandard2.0\ClassLibrary1.dll");
            Type type = oop.GetClassType(text);
            var a = Activator.CreateInstance(type);
            Console.WriteLine(a.ToString());
            Console.ReadKey();
        }
    }
}
