using Natasha.Complier;
using System;

namespace Natasha
{
    public static class ClassOperator
    {
        public static Type GetType(string name)
        {
           return AssemblyOperator.Loader(ScriptComplieEngine.ClassMapping[name])[name];
        }
    }
}
