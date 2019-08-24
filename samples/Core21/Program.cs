using Natasha;
using System;
using System.Collections.Generic;

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

         
            Console.WriteLine(typeof(List<int>[]).GetDevelopName());
            Console.WriteLine(typeof(List<int>[,]).GetDevelopName());
            Console.WriteLine(typeof(int[,]).GetDevelopName());
            Console.WriteLine(typeof(int[][]).GetDevelopName());
            Console.WriteLine(typeof(int[][,,,]).GetDevelopName());
            Console.ReadKey();
        }
    }
}
