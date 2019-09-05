using Natasha.Operator;
using System;

namespace Natasha
{
    public class NewClass
    {

        public static (CompilationException Exception, Type Type) Create(Action<OopOperator> action, int classIndex = 1, int namespaceIndex = 1)
        {

            OopOperator builder = new OopOperator();
            builder.Public.ChangeToClass();
            action(builder);
            var result = builder.GetType(classIndex, namespaceIndex);
            return (builder.Complier.ComplieException, result);

        }

    }
}
