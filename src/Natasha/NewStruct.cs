using Natasha.Operator;
using System;

namespace Natasha
{
    public class NewStruct
    {

        public static (CompilationException Exception, Type Type) Create(Action<OopOperator> action, int classIndex = 1, int namespaceIndex = 1)
        {

            OopOperator builder = new OopOperator();
            builder.ChangeToStruct();
            action(builder);
            var result = builder.GetType(classIndex, namespaceIndex);
            return (builder.Complier.Exception, result);

        }

    }
}
