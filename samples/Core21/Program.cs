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

        public static int Get(int temp){

            switch (temp)
            {
                case 100:
                    temp = 1;
                    break;
                case 200:
                    temp = 333;
                    break;
                case 300:
                    temp = 645;
                    break;
                case 400:
                    temp = 1412;
                    break;
                case 500:
                    temp = 653;
                    break;
                case 600:
                    temp = 2988;
                    break;

                default:
                    temp = 2019;
                    break;
            }
            return temp;
        }
    }
}";
            //根据脚本创建动态类
            var domain =  AssemblyManagment.Create("Default");
            domain.LoadFile(@"D:\Project\IlTest\ClassLibrary1\bin\Debug\netstandard2.0\ClassLibrary1.dll");

            OopComplier oop = new OopComplier();
            oop.Domain = domain;
            Type type = oop.GetClassType(text);
            var a = Activator.CreateInstance(type);
            Console.WriteLine(a.ToString());
            Console.ReadKey();
        }
    }
}
