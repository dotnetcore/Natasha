using Natasha.Operator;
using System;

namespace Natasha
{
    public static class NewMethod
    {

        public static (CompilationException Exception, Delegate Method) Create(Action<FastMethodOperator> action)
        {

            FastMethodOperator builder = FastMethodOperator.RandomDomain;
            action(builder);
            var result = builder.Complie();
            return (builder.Complier.ComplieException, result);

        }



        public static (CompilationException Exception, T Method) Create<T>(Action<FastMethodOperator> action) where T: Delegate
        {

            FastMethodOperator builder = FastMethodOperator.RandomDomain;
            action(builder);
            var result = builder.Complie<T>();
            return (builder.Complier.ComplieException, result);

        }

    }
}
