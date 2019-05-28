using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha.Complier
{
    public class DefaultComplier :IComplier
    {
        public string Script;

        public override string Builder()
        {
            return Script;
        }


        public Delegate Complie((string Flag, IEnumerable<Type> Types, string Script, Type Delegate) info,string className=null)
        {
            Script = info.Script;
            return Complie(className, info.Flag, info.Delegate);
        }


        public Delegate Complie(string className,string methodName,Type delegateType)
        {
            Assembly assembly = GetAssemblyByScript(className);

            if (assembly == null)
            {
                return null;
            }

            return AssemblyOperator
                .Loader(assembly)[className]
                .GetMethod(methodName)
                .CreateDelegate(delegateType);
        }
    }
}
