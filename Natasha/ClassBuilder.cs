using System;
using System.Reflection;

namespace Natasha
{
    public class ClassBuilder
    {
        static ClassBuilder()
        {
            
        }
        public static Type GetType(string content)
        {
            Assembly assembly = null;
            string className = ScriptComplier.GetClassName(content);
            assembly = ScriptComplier.Complier(content, className);
            return AssemblyOperator.Loader(assembly)[className];
        }
    }
}
