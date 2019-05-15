using System;
using System.Reflection;

namespace Natasha
{
    public class ClassBuilder
    {
        public Type GetType(string content)
        {
           string className = ScriptComplier.GetClassName(content);
           Assembly assembly = ScriptComplier.Complier(content, className);
           return AssemblyOperator.Loader(assembly)[className];
        }
    }
}
