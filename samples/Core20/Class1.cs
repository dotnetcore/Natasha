using Natasha.CSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core20
{
    public static class ValueExtension
    {
        public static Type GetValue(this ValueTuple value)
        {
            return value.GetType();
        }
    }

    public class Test2 : Test {

        public override string Get()
        {
            return A;
        }
    }
    public class Test
    {
        public readonly string A;
        public Test()
        {
            A = Guid.NewGuid().ToString();
        }
        public void Show()
        {
            Console.WriteLine(A);
        }


        public void CreateScript(string script = default)
        {
            //GetValue = FastMethodOperator
            //    .RandomDomain()
            //    .Param<Test>("parameter")
            //    .Return<string>()
            //    .Body("return parameter.A;")
            //    .Compile<Func<string>>(this);
            var domain = DomainManagement.Default;
            var subClass = NClass
                .UseDomain(domain)
                .InheritanceAppend<Test>()
                .Body($"public override string Get(){{ {script} }}")
                .GetType();

            var methodInfo = subClass.GetMethod("Get");
            this.GetValue = (Func<string>)(methodInfo.CreateDelegate(typeof(Func<string>), this));

        }

        public virtual string Get()
        {
            Console.WriteLine("a");
            return GetValue();
        } 

        public Func<string> GetValue;
    }
}
