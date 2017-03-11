using Natasha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodTest
{
    class Program
    {
        static Func<string, bool> func;
        static void Main(string[] args)
        {
            TestType();
            func = (str) =>
              {
                  if (str == "1")
                  {
                      return true;
                  }
                  return false;
              };
            TestList();
            //TestFunc(() => { return 1; });
            Console.ReadKey();
        }

        public static void TestList()
        {
            List<string> list = new List<string>();
            
            bool shut = list.All(func);
            Console.WriteLine(shut);

            Func<List<string>, Func<string, bool>, bool> ShowDelegate = (Func<List<string>, Func<string, bool>, bool>)EHandler.CreateMethod<List<string>, Func<string, bool>, bool>((il) =>
               {
                   EVar elist = EVar.CreateVarFromParameter(0);
                   EVar efunc = EVar.CreateVarFromParameter(1);
                   EMethod.Load(elist)
                   .Use(typeof(Enumerable))
                   .AddGenricType<string>()
                   .ExecuteMethod<Func<string, bool>>("All", efunc);
               }).Compile();

            Console.WriteLine(ShowDelegate(list, (str) =>
            {
                if (str == "1")
                {
                    return true;
                }
                return false;
            }));

           Action ShowDelegate1 = (Action)EHandler.CreateMethod<ENull>((il) =>
            {
                EModel model = EModel.CreateModel<Student>().UseDefaultConstructor();
                EMethod.Load(model).ExecuteMethod("Show");
                EMethod.Load(model).ExecuteMethod<string>("Show","test");
            }).Compile();
            ((Action)ShowDelegate1)();
        }

        public static void TestFunc(List<string> list ,Func<string, bool> c)
        {
            Console.WriteLine(list.All(c));
        }

        public static void TestType()
        {
            Console.WriteLine(typeof(string));
        }
    }
}
