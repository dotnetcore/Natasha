using PluginBase;
using System;
using System.Runtime.CompilerServices;

namespace PluginSample
{
    class Program
    {
        static void Main(string[] args)
        {

            NatashaInitializer.InitializeAndPreheating();
            var domain = DomainManagement.Random;
            var assmebly = domain.LoadPlugin(@"E:\OpenSource\NCCGroup\Natasha\samples\Plugin\NatashaPluginSample\bin\Debug\net5.0\ImplementPlugin.dll");
            var type = NDelegate.UseDomain(domain).Func<Type>("return typeof(APTest);")();
            Console.WriteLine(type == typeof(APTest));
            Console.Read();


        }

        public class A
        {
            [SkipLocalsInit]
            public static void Show()
            {

            }
        }
    }
}
