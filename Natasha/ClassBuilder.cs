using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Natasha
{
    public class ClassBuilder
    {
        public static ConcurrentDictionary<string, Assembly> ClassScriptMapping;
        static ClassBuilder()
        {
            ClassScriptMapping = new ConcurrentDictionary<string, Assembly>();
        }
        public static Type GetType(string content)
        {
            Assembly assembly = null;
            string className = ScriptComplier.GetClassName(content);
            if (!ClassScriptMapping.ContainsKey(content))
            {
                assembly = ScriptComplier.Complier(content, className);
                ClassScriptMapping[content] = assembly;
            }
           
            return AssemblyOperator.Loader(assembly)[className];
        }
    }
}
